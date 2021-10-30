using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsGeneral.Libraries.Misc;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			string modName = "[c/FFFF88:" + PowerfulMagicMod.Instance.DisplayName + "] - ";

			int tipIdx = 1;

			this.ModifyTooltips_MagicIf( modName, item, tooltips, ref tipIdx );

			switch( item.type ) {
			case ItemID.SpaceGun:
			case ItemID.LaserRifle:
				this.ModifyTooltips_Lasers( modName, item, tooltips, ref tipIdx );
				this.ModifyTooltips_TemperatureIf( modName, item, tooltips, ref tipIdx );
				break;
			case ItemID.MeteorHelmet:
			case ItemID.MeteorSuit:
			case ItemID.MeteorLeggings:
				this.ModifyTooltips_TemperatureIf( modName, item, tooltips, ref tipIdx );
				break;
			}
		}


		////////////////

		private void ModifyTooltips_MagicIf(
					string modName,
					Item item,
					List<TooltipLine> tooltips,
					ref int tipIdx ) {
			if( !item.magic ) {
				return;
			}

			float? dmgScale = PowerfulMagicItem.GetItemDamageScale( item, 0 );
			if( !dmgScale.HasValue ) {
				return;
			}

			var config = PowerfulMagicConfig.Instance;
			if( config == null ) {
				return;
			}

			//int newDmg = Main.LocalPlayer.GetWeaponDamage( item );
			int dmgPercent = (int)(dmgScale * 100f);
			int manaPercent = (int)(config.Get<float>( nameof(PowerfulMagicConfig.WeaponManaConsumeMulitplier) ) * 100f);

			string tip1Text = modName + "Magic increased " + dmgPercent + "% of base amount";
			var tip1 = new TooltipLine( this.mod, "PowerfulMagicDmgUpTip", tip1Text );
			//tip1.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			string tip2Text = modName + "Mana use increased " + manaPercent + "% of base amount";
			var tip2 = new TooltipLine( this.mod, "PowerfulMagicManaUpTip", tip2Text );
			//tip2.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			tooltips.Insert( tipIdx++, tip1 );
			tooltips.Insert( tipIdx++, tip2 );
		}


		////

		private void ModifyTooltips_Lasers(
					string modName,
					Item item,
					List<TooltipLine> tooltips,
					ref int tipIdx ) {
			string tipText = modName + "Warning: May overheat if overused with Meteor Armor!";
			var tip = new TooltipLine( this.mod, "PowerfulMagicLaserWarning", tipText );

			tooltips.Insert( tipIdx++, tip );
		}

		private void ModifyTooltips_TemperatureIf(
					string modName,
					Item item,
					List<TooltipLine> tooltips,
					ref int tipIdx ) {
			//var myplayer = Main.LocalPlayer.GetModPlayer<PowerfulMagicPlayer>();
			//float temp = myplayer.MeteorArmorTemperature;
			float temp = this.Temperature;
			if( temp <= 0f ) {
				return;
			}

			Color color = Color.Lerp( Color.Lime, Color.Red, Math.Min(temp / 100f, 1f) );
			string clrHex = MiscLibraries.RenderColorHex( color );

			string tipText = modName + "Temperature percent until overheat: [c/"+clrHex+":" + (int)temp + "%]";
			var tip = new TooltipLine( this.mod, "PowerfulMagicTemp", tipText );

			tooltips.Insert( tipIdx++, tip );
		}
	}
}
