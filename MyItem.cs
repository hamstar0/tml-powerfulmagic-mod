using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( !item.magic ) {
				return;
			}

			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;

			int newDmg = (int)((config?.DamageScale ?? 1f) * (float)Main.LocalPlayer.GetWeaponDamage(item));
			int percent = (int)(config?.DamageScale ?? 1f) * 100;

			var tip = new TooltipLine( this.mod, "PowerfulMagicTip", "Magic scaled "+percent+"x: "+newDmg );
			tip.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			tooltips.Insert( 0, tip );
		}


		////////////////

		public override bool ConsumeItem( Item item, Player player ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;

			if( item.healMana > 0 ) {
				float dmgMul = config?.DamageScale ?? 1f;
				float manaMul = config?.ManaScale ?? 1f;

				player.AddBuff( BuffID.ManaSickness, 60 * 5 * (int)( dmgMul * 0.5f ) );
				player.statMana -= (int)( (float)item.healMana * manaMul );
			}
			return true;
		}

		public override bool OnPickup( Item item, Player player ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;

			if( item.type == ItemID.Star ) {
				float manaMul = config?.ManaScale ?? 1f;

				player.statMana -= (int)(100f * manaMul);
			}

			return true;
		}
	}
}
