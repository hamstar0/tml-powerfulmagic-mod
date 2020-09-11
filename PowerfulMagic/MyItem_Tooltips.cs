using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( !item.magic ) {
				return;
			}

			float dmgScale = PowerfulMagicItem.GetItemDamageScale( item, 0 );
			if( dmgScale == 1f ) {
				return;
			}

			var mymod = (PowerfulMagicMod)this.mod;
			var config = PowerfulMagicConfig.Instance;
			if( config == null ) {
				return;
			}

			//int newDmg = Main.LocalPlayer.GetWeaponDamage( item );
			int dmgPercent = (int)(dmgScale * 100f);
			int manaPercent = (int)(config.WeaponManaConsumeMulitplier * 100f);

			var tip1 = new TooltipLine( this.mod, "PowerfulMagicDmgUpTip", "Magic increased "+dmgPercent+"% of base amount" );
			tip1.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );
			var tip2 = new TooltipLine( this.mod, "PowerfulMagicManaUpTip", "Mana use increased "+manaPercent+"% of base amount" );
			tip2.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			tooltips.Insert( 1, tip1 );
			tooltips.Insert( 2, tip2 );
		}
	}
}
