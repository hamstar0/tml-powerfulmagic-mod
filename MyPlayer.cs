using System;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicPlayer : ModPlayer {
		public override void PreUpdate() {
			if( this.player.whoAmI == Main.LocalPlayer.whoAmI ) {
				var mymod = (PowerfulMagicMod)this.mod;
				mymod.RunOscillation();
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
				float mul = config?.ManaReduceScale ?? 1f;

				this.player.manaRegenCount -= (int)((float)this.player.manaRegen * mul );
			}
		}
	}
}
