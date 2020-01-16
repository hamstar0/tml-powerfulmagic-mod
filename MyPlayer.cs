using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicPlayer : ModPlayer {
		public bool RecentPickup { get; internal set; } = false;
		public int ManaBeforePickup { get; internal set; } = -1;

		////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.player.whoAmI == Main.LocalPlayer.whoAmI ) {
				this.PreUpdateLocal();
			}
		}


		private void PreUpdateLocal() {
			var mymod = (PowerfulMagicMod)this.mod;
			mymod.RunOscillation();

			if( this.RecentPickup ) {
				this.RecentPickup = false;

				PowerfulMagicItem.OnManaPickup( this.player, this.ManaBeforePickup );

				for( int idx = 0; idx < Main.combatText.Length; idx++ ) {
					CombatText txt = Main.combatText[idx];
					if( txt == null || !txt.active ) { continue; }

					if( txt.text.Equals( "100" ) ) {
						if( PowerfulMagicMod.Instance.Config.DebugModeInfo ) {
							Main.NewText( "Old mana heal? amount from recent pickup: 100" );
						}

						txt.text = (int)( (float)100 * mymod.Config.ManaHealScale ) + "";
						break;
					}
				}
			}
		}


		////////////////

		public override bool PreItemCheck() {
			Item heldItem = this.player.HeldItem;
			if( heldItem == null || heldItem.IsAir || !heldItem.magic ) {
				return base.PreItemCheck();
			}

			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[manaSicknessBuffIdx]
				: 0;

			return manaSicknessTicks < PowerfulMagicMod.Instance.Config.ManaSicknessMaximumTicksAllowedToEnableAttacks;
		}


		////////////////

		public override void PreUpdateBuffs() {
			this.UpdateManaRegen();
		}


		////////////////

		public override void ModifyWeaponDamage( Item item, ref float directScale, ref float afterScale, ref float flat ) {
			if( !item.magic ) {
				return;
			}
			if( item.type == ItemID.SpaceGun || item.type == ItemID.LaserRifle ) {
				if( item.owner != -1 && Main.player[item.owner]?.active == true ) {
					Player plr = Main.player[item.owner];

					// Laser weapons + meteor armor = no mana = no damage increase
					if( plr.armor[0].type == ItemID.MeteorHelmet
						&& plr.armor[1].type == ItemID.MeteoriteChest
						&& plr.armor[2].type == ItemID.MeteorLeggings ) {
						return;
					}
				}
			}

			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;

			if( config != null ) {
				int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
				int manaSicknessTicks = manaSicknessBuffIdx != -1
					? this.player.buffTime[ manaSicknessBuffIdx ]
					: 0;

				afterScale *= config.DamageScale;
				afterScale *= 1f - ((manaSicknessTicks / 300) * config.MaxManaSicknessDamageScale);
			}
		}

		//public override void UpdateLifeRegen() {
		public void UpdateManaRegen() {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;

			this.player.nebulaManaCounter -= this.player.nebulaLevelMana / 2;

			if( this.player.manaRegenCount > 0 ) {
				float mul = config?.ManaRegenScale ?? 1f;
				mul = 1f - mul;

				if( PowerfulMagicMod.Instance.Config.DebugModeInfo ) {
					//DebugHelpers.Print( "manaregen", "Old mana regen amount: "+this.player.manaRegenCount );
				}

				this.player.manaRegenCount -= (int)( (float)this.player.manaRegen * mul );
			}
		}
	}
}
