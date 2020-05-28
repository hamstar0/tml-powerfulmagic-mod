using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		private bool IsMagicItemAllowedForUse() {
			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[manaSicknessBuffIdx]
				: 0;

			return manaSicknessTicks < PowerfulMagicConfig.Instance.ManaSicknessMaximumTicksAllowedToEnableAttacks;
		}


		////////////////
		
		private void ApplyFocusMode() {

		}


		////////////////

		private void ModifyMagicWeaponDamage( Item item, ref float afterScale ) {
			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[ manaSicknessBuffIdx ]
				: 0;

			afterScale *= PowerfulMagicItem.GetItemDamageScale( item );
			afterScale *= 1f - ((manaSicknessTicks / 300) * PowerfulMagicConfig.Instance.MaxManaSicknessDamageScale);
		}


		////////////////

		public void UpdateManaRegen() { //UpdateLifeRegen
			this.player.nebulaManaCounter -= this.player.nebulaLevelMana / 2;

			if( this.player.manaRegenCount > 0 ) {
				float mul = PowerfulMagicConfig.Instance.ManaRegenScale;
				mul = 1f - mul;

				if( PowerfulMagicConfig.Instance.DebugModeInfo ) {
					//DebugHelpers.Print( "manaregen", "Old mana regen amount: "+this.player.manaRegenCount );
				}

				this.player.manaRegenCount -= (int)( (float)this.player.manaRegen * mul );
			}
		}


		////////////////

		private void RecentManaStarPickup() {
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

				txt.text = (int)( (float)100 * PowerfulMagicConfig.Instance.ManaHealScale ) + "";
				break;
			}
		}
	}
}
