using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Players;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		private void UpdateFocusHoldStyle( Item item, Player player ) {
			var myplayer = player.GetModPlayer<PowerfulMagicPlayer>();
			if( myplayer.FocusPercent > 0f ) {
				if( !this.IsFocusing ) {
					this.IsFocusing = true;
					this.OldHoldStyle = item.holdStyle;

					item.holdStyle = ItemHoldStyleID.HoldingOut;
				}
			} else {
				if( this.IsFocusing ) {
					this.IsFocusing = false;

					item.holdStyle = this.OldHoldStyle;
				}
			}

			if( this.IsFocusing ) {
				Vector2 heldOffset = player.itemLocation - player.MountedCenter;

				switch( item.type ) {
				case ItemID.SpaceGun:
				case ItemID.LaserRifle:
				case ItemID.BeeGun:
				case ItemID.WaspGun:
				case ItemID.BubbleGun:
				case ItemID.RainbowGun:
				case ItemID.ChargedBlasterCannon:
					heldOffset.X += Math.Abs( heldOffset.X );
					//heldOffset.Y += Math.Abs( heldOffset.Y );
					//heldOffset.X += 20;
					heldOffset.Y += 12;
					break;
				}

				player.itemLocation -= heldOffset;
				//player.itemLocation -= heldOffset * 0.75f;	// hold item 75% closer
			}
		}
	}
}
