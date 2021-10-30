using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
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

			string modName = "[c/FFFF88:" + PowerfulMagicMod.Instance.DisplayName + "] - ";

			int tipIdx = 1;

			this.ModifyTooltips_Magic( modName, tooltips, dmgScale.Value, ref tipIdx );

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

		private void ModifyTooltips_Magic(
					string modName,
					List<TooltipLine> tooltips,
					float dmgScale,
					ref int tipIdx ) {
			var config = PowerfulMagicConfig.Instance;

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
			if( this.Temperature <= 0f ) {
				return;
			}

			string tipText = modName + "Temperature percent until overhead: " + (int)this.Temperature;
			var tip = new TooltipLine( this.mod, "PowerfulMagicTemp", tipText );

			tip.overrideColor = Color.Lerp( Color.Lime, Color.Red, this.Temperature );

			tooltips.Insert( tipIdx++, tip );
		}
	}
}
