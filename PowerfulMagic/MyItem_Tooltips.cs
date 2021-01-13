using System;
using System.Collections.Generic;
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

			string modName = "[c/FFFF88:" + PowerfulMagicMod.Instance.DisplayName + "] - ";
			//int newDmg = Main.LocalPlayer.GetWeaponDamage( item );
			int dmgPercent = (int)(dmgScale * 100f);
			int manaPercent = (int)(config.Get<float>( nameof(PowerfulMagicConfig.WeaponManaConsumeMulitplier) ) * 100f);

			string tip1Text = modName + "Magic increased " + dmgPercent + "% of base amount";
			var tip1 = new TooltipLine( this.mod, "PowerfulMagicDmgUpTip", tip1Text );
			//tip1.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			string tip2Text = modName + "Mana use increased " + manaPercent + "% of base amount";
			var tip2 = new TooltipLine( this.mod, "PowerfulMagicManaUpTip", tip2Text );
			//tip2.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			tooltips.Insert( 1, tip1 );
			tooltips.Insert( 2, tip2 );
		}
	}
}
