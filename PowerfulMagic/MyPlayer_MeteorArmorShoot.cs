using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		private void UpdateTemperatureAndEffects() {
			if( this.MeteorArmorTemperature > 0f ) {
				this.MeteorArmorTemperature -= 2.5f / 60f;

				if( this.MeteorArmorTemperature < 0f ) {
					this.MeteorArmorTemperature = 0f;
				}
			}
//DebugLibraries.Print( "temp", "temp:"+this.MeteorArmorTemperature );

			//

			Item head = this.player.armor[0];
			Item body = this.player.armor[1];
			Item legs = this.player.armor[2];

			if( head.active && head.type == ItemID.MeteorHelmet
					&& body.active && body.type == ItemID.MeteorSuit
					&& legs.active && legs.type == ItemID.MeteorLeggings ) {
				this.UpdateTemperatureItems();
			}
		}

		private void UpdateTemperatureItems() {
			if( this.MeteorArmorTemperature >= 100f ) {
				this.player.AddBuff( BuffID.Burning, 2 );
			}

			Item head = this.player.armor[0];
			Item body = this.player.armor[1];
			Item legs = this.player.armor[2];

			head.GetGlobalItem<PowerfulMagicItem>().Temperature = this.MeteorArmorTemperature;
			body.GetGlobalItem<PowerfulMagicItem>().Temperature = this.MeteorArmorTemperature;
			legs.GetGlobalItem<PowerfulMagicItem>().Temperature = this.MeteorArmorTemperature;

			Item heldItem = this.player.HeldItem;

			switch( heldItem.type ) {
			case ItemID.SpaceGun:
			case ItemID.LaserRifle:
				heldItem.GetGlobalItem<PowerfulMagicItem>().Temperature = this.MeteorArmorTemperature;
				break;
			}
		}


		////

		private void ApplyMeteorArmorShootBehaviorIf( Item shootItem ) {
			bool isSpaceGun = shootItem.type == ItemID.SpaceGun;
			bool isLaserRifle = shootItem.type == ItemID.LaserRifle;
			if( !isSpaceGun && !isLaserRifle ) {
				return;
			}

			Item head = this.player.armor[0];
			Item body = this.player.armor[1];
			Item legs = this.player.armor[2];

			if( !head.active || head.type != ItemID.MeteorHelmet
					|| !body.active || body.type != ItemID.MeteorSuit
					|| !legs.active || legs.type != ItemID.MeteorLeggings ) {
				return;
			}

			this.MeteorArmorTemperature += 2f;
			if( this.MeteorArmorTemperature > 125f ) {
				this.MeteorArmorTemperature = 125f;
			}
		}


		////////////////

		private void ApplyMeteorArmorAppearanceIf( ref PlayerDrawInfo drawInfo ) {
			Item head = this.player.armor[0];
			Item body = this.player.armor[1];
			Item legs = this.player.armor[2];

			if( !head.active || head.type != ItemID.MeteorHelmet
					|| !body.active || body.type != ItemID.MeteorSuit
					|| !legs.active || legs.type != ItemID.MeteorLeggings ) {
				return;
			}

			float temp = this.MeteorArmorTemperature;
			drawInfo.lowerArmorColor = PowerfulMagicItem.GetTemperatureColor( drawInfo.lowerArmorColor, temp );
			drawInfo.middleArmorColor = PowerfulMagicItem.GetTemperatureColor( drawInfo.middleArmorColor, temp );
			drawInfo.upperArmorColor = PowerfulMagicItem.GetTemperatureColor( drawInfo.upperArmorColor, temp );
		}
	}
}
