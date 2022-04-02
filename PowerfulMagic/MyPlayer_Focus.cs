using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		public static bool CanItemBeFocused( Item item ) {
			switch( item.type ) {
			case ItemID.ChargedBlasterCannon:
			case ItemID.BookStaff:
				return false;
			}

			return item.magic;
		}



		////////////////

		private void UpdateFocusManaRegen_If() {
			Item heldItem = this.player.HeldItem;
			if( heldItem?.active != true ) {
				return;
			}
			
			//

			var config = PowerfulMagicConfig.Instance;

			//

			if( Main.mouseRight && PowerfulMagicPlayer.CanItemBeFocused(heldItem) ) {
				float focusChargePerSec = config.Get<float>( nameof(config.FocusManaChargeRatePerSecond) );

				this.FocusPercent += focusChargePerSec / 60f;
				if( this.FocusPercent > 1f ) {
					this.FocusPercent = 1f;
				}
			} else {
				this.FocusPercent = 0f;
			}

			//

			if( this.FocusPercent > 0f && (Main.GameUpdateCount % 60) == 0 ) {
				float focusChargeRate = config.Get<float>( nameof(config.FocusManaChargeMaxRatePerSecond) );
				int amt = (int)(this.FocusPercent * focusChargeRate);

				if( amt > 0 ) {
					int missingMana = this.player.statManaMax2 - this.player.statMana;
					if( missingMana < amt ) {
						amt = missingMana;
					}

					//

					this.player.statMana += amt;

					//

					CombatText.NewText( this.player.getRect(), CombatText.HealMana, amt );
				}
			}
		}


		////////////////

		private void ApplyFocusMovementBehavior() {
			this.ApplyFocusMovementChanges();

			this.ApplyFocusMovementInterruptions_If();
		}

		////

		private void ApplyFocusMovementChanges() {
			var config = PowerfulMagicConfig.Instance;

			this.player.maxRunSpeed *= config.Get<float>( nameof(config.FocusMoveSpeedScale) );
			this.player.accRunSpeed = this.player.maxRunSpeed;
			this.player.moveSpeed *= config.Get<float>( nameof(config.FocusMoveSpeedScale) );

			//

			float jumpScale = config.Get<float>( nameof(config.FocusJumpScale) );
			int maxJump = (int)( (float)Player.jumpHeight * jumpScale );

			if( this.player.jump > maxJump ) {
				this.player.jump = maxJump;
			}
		}


		private void ApplyFocusMovementInterruptions_If() {
			if( Math.Abs(this.player.velocity.Y) < 1f ) {   // was 0.1
				return;
			}

			//

			var config = PowerfulMagicConfig.Instance;
			if( !config.Get<bool>(nameof(config.FocusInterruptsOnMove) ) ) {
				return;
			}

			//

			this.FocusPercent -= 1f / 5f;   // was 1/3

			if( this.FocusPercent < 0f ) {
				this.FocusPercent = 0f;
			}
		}
	}
}
