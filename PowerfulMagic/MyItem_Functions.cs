using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.CameraAnimation;
using HamstarHelpers.Helpers.Players;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		private void OnConsumeManaMessage( Item item ) {
			var config = PowerfulMagicConfig.Instance;
			/*float manaMul = 1f - config.ManaScale;

			player.statMana -= (int)( (float)item.healMana * manaMul );
			player.statMana = player.statMana < 0 ? 0 : player.statMana;*/

			for( int idx=0; idx < Main.combatText.Length; idx++ ) {
				CombatText txt = Main.combatText[idx];
				if( txt == null || !txt.active ) { continue; }
				
				if( txt.text.Equals(item.healMana+"") ) {
					if( config.DebugModeInfo ) {
						Main.NewText( "Old mana heal amount on consume of " + item.Name + ": " + item.healMana );
					}
					
					float manaHealScale = config.Get<float>( nameof(PowerfulMagicConfig.ManaHealScale) );
					txt.text = (int)((float)item.healMana * manaHealScale) + "";
					break;
				}
			}
			//return true;
		}


		private void OnManaPickup( Item item, Player player ) {
			if( PowerfulMagicConfig.Instance.DebugModeInfo ) {
				Main.NewText( "Old mana heal amount on pickup of " + item.Name + ": 100" );
			}

			var myplayer = player.GetModPlayer<PowerfulMagicPlayer>();
			myplayer.RecentPickup = true;
			myplayer.ManaBeforePickup = player.statMana;
		}


		////////////////

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
				player.itemLocation -= heldOffset * 0.75f;
			}
		}


		////////////////

		private void ApplyPlayerSpellFx( int damage ) {
			if( !PlayerMovementHelpers.IsOnFloor( Main.LocalPlayer ) ) {
				return;
			}

			float shakePower = (float)damage / 30f;
			float magnitude = MathHelper.Clamp( shakePower, 1f, 15f ) * 0.5f;

			var curr = CameraShaker.Current;
			if( curr != null ) {
				float percent = (float)curr.TicksElapsed / (float)curr.TotalTickDuration;
				percent = 1f - percent;

				if( (percent * curr.ShakePeakMagnitude) > magnitude ) {
					return;
				}
			}

			CameraShaker.Current = new CameraShaker(
				name: "PowerfulMagicFx",
				peakMagnitude: magnitude,
				toDuration: 0,
				lingerDuration: 1,
				froDuration: 10 + (int)( (magnitude - 0.5f) * 10f ),
				isSmoothed: false
			);
		}
	}
}
