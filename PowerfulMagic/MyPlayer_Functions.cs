using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		private bool IsMagicItemAllowedForUse() {
			var config = PowerfulMagicConfig.Instance;
			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[manaSicknessBuffIdx]
				: 0;
			int manaSickMaxTicks = config.Get<int>(
				nameof(PowerfulMagicConfig.ManaSicknessMaximumTicksAllowedToEnableAttacks)
			);

			return manaSicknessTicks < manaSickMaxTicks;
		}


		////////////////

		private void ModifyMagicWeaponDamage( Item item, ref float afterScale ) {
			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[ manaSicknessBuffIdx ]
				: 0;

			afterScale *= PowerfulMagicItem.GetItemDamageScale( item, manaSicknessTicks );
			//afterScale *= 1f - ((manaSicknessTicks / 300) * PowerfulMagicConfig.Instance.MaxManaSicknessDamageScale);
		}


		////////////////

		public void UpdateManaRegen() { //UpdateLifeRegen
			var config = PowerfulMagicConfig.Instance;

			this.player.nebulaManaCounter -= this.player.nebulaLevelMana / 2;

			if( this.player.manaRegenCount > 0 ) {
				float mul = config.Get<float>( nameof(PowerfulMagicConfig.ManaRegenScale) );
				mul = 1f - mul;

				if( PowerfulMagicConfig.Instance.DebugModeInfo ) {
					//DebugHelpers.Print( "manaregen", "Old mana regen amount: "+this.player.manaRegenCount );
				}

				this.player.manaRegenCount -= (int)( (float)this.player.manaRegen * mul );
			}
		}

		////////////////

		private void UpdateFocusManaRegen() {
			var config = PowerfulMagicConfig.Instance;

			if( Main.mouseRight && this.player.HeldItem?.magic == true ) {
				this.FocusPercent += config.Get<float>( nameof(PowerfulMagicConfig.FocusPercentChargeRatePerTick) );
				if( this.FocusPercent > 1f ) {
					this.FocusPercent = 1f;
				}
			} else {
				this.FocusPercent = 0f;
			}
			
			if( this.FocusPercent > 0f && (Main.GameUpdateCount % 60) == 0 ) {
				float focusChargeRate = config.Get<float>( nameof(PowerfulMagicConfig.FocusManaChargeMaxRatePerSecond) );
				int amt = (int)( this.FocusPercent * focusChargeRate );

				if( amt > 0 ) {
					this.player.statMana += amt;
					CombatText.NewText( this.player.getRect(), CombatText.HealMana, amt );
				}
			}
		}


		////////////////

		private void RecentManaStarPickup() {
			var config = PowerfulMagicConfig.Instance;

			PowerfulMagicItem.OnManaPickup( this.player, this.ManaBeforePickup );

			for( int idx = 0; idx < Main.combatText.Length; idx++ ) {
				CombatText txt = Main.combatText[idx];
				if( txt == null || !txt.active ) {
					continue;
				}
				if( !txt.text.Equals("100") ) {
					continue;
				}

				if( PowerfulMagicConfig.Instance.DebugModeInfo ) {
					Main.NewText( "Old mana heal? amount from recent pickup: 100" );
				}

				float manaHealScale = config.Get<float>( nameof(PowerfulMagicConfig.ManaHealScale) );

				txt.text = (int)( (float)100 * manaHealScale ) + "";
				break;
			}
		}


		private void ApplyFocusMovementEffects() {
			var config = PowerfulMagicConfig.Instance;

			this.player.maxRunSpeed *= config.Get<float>( nameof(PowerfulMagicConfig.FocusMoveSpeedScale) );
			this.player.accRunSpeed = this.player.maxRunSpeed;
			this.player.moveSpeed *= config.Get<float>( nameof(PowerfulMagicConfig.FocusMoveSpeedScale) );

			float jumpScale = config.Get<float>( nameof(PowerfulMagicConfig.FocusJumpScale) );
			int maxJump = (int)( (float)Player.jumpHeight * jumpScale );
			if( this.player.jump > maxJump ) {
				this.player.jump = maxJump;
			}
		}
	}
}
