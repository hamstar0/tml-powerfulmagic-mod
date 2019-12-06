using System;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicPlayer : ModPlayer {
		public bool RecentPickup { get; internal set; } = false;

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

				for( int idx = 0; idx < Main.combatText.Length; idx++ ) {
					CombatText txt = Main.combatText[idx];
					if( txt == null || !txt.active ) { continue; }

					if( txt.text.Equals( "100" ) ) {
						txt.text = (int)( (float)100 * mymod.Config.ManaScale ) + "";
						Main.NewText( "! OnPickup " + txt.text );
						break;
					}
				}
			}
		}


		////////////////

		public override void ModifyWeaponDamage( Item item, ref float directScale, ref float afterScale, ref float flat ) {
			if( item.magic ) {
				var mymod = (PowerfulMagicMod)this.mod;
				var config = mymod.Config;

				afterScale *= config?.DamageScale ?? 1f;
			}
		}

		public override void UpdateLifeRegen() {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;

			this.player.nebulaManaCounter -= this.player.nebulaLevelMana / 2;

			if( this.player.manaRegenCount > 0 ) {
				float mul = config?.ManaScale ?? 1f;
				mul = 1f - mul;

				this.player.manaRegenCount -= (int)((float)this.player.manaRegen * mul);
			}
		}
	}
}
