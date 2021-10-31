using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		private void UpdateFocusManaRegen() {
			var config = PowerfulMagicConfig.Instance;
			Item heldItem = this.player.HeldItem;
			bool usesAlt = false;

			switch( heldItem?.type ) {
			case ItemID.ChargedBlasterCannon:
			case ItemID.BookStaff:
				usesAlt = true;
				break;
			}

			if( Main.mouseRight && heldItem?.magic == true && !usesAlt ) {
				float focusChargePerSec = config.Get<float>( nameof(PowerfulMagicConfig.FocusManaChargeRatePerSecondIncrease) );

				this.FocusPercent += focusChargePerSec / 60f;
				if( this.FocusPercent > 1f ) {
					this.FocusPercent = 1f;
				}
			} else {
				this.FocusPercent = 0f;
			}
			
			if( this.FocusPercent > 0f && (Main.GameUpdateCount % 60) == 0 ) {
				float focusChargeRate = config.Get<float>( nameof(PowerfulMagicConfig.FocusManaChargeMaxRatePerSecond) );
				int amt = (int)(this.FocusPercent * focusChargeRate);

				if( amt > 0 ) {
					int missingMana = this.player.statManaMax2 - this.player.statMana;
					if( missingMana < amt ) {
						amt = missingMana;
					}

					this.player.statMana += amt;
					CombatText.NewText( this.player.getRect(), CombatText.HealMana, amt );
				}
			}
		}


		////////////////

		private void ApplyFocusMovementBehavior() {
			this.ApplyFocusMovementChanges();

			this.ApplyFocusMovementInterruptionsIf();
		}

		////

		private void ApplyFocusMovementChanges() {
			var config = PowerfulMagicConfig.Instance;

			this.player.maxRunSpeed *= config.Get<float>( nameof(PowerfulMagicConfig.FocusMoveSpeedScale) );
			this.player.accRunSpeed = this.player.maxRunSpeed;
			this.player.moveSpeed *= config.Get<float>( nameof(PowerfulMagicConfig.FocusMoveSpeedScale) );

			float jumpScale = config.Get<float>( nameof(PowerfulMagicConfig.FocusJumpScale) );
			int maxJump = (int)( (float)Player.jumpHeight * jumpScale );
			if( this.player.jump > maxJump ) {
				this.player.jump = maxJump;
			}
		}


		private void ApplyFocusMovementInterruptionsIf() {
			if( Math.Abs(this.player.velocity.Y) < 1f ) {   // was 0.1
				return;
			}

			var config = PowerfulMagicConfig.Instance;
			if( !config.Get<bool>(nameof(config.FocusInterruptsOnMove) ) ) {
				return;
			}

			this.FocusPercent -= 1f / 5f;   // was 1/3
			if( this.FocusPercent < 0f ) {
				this.FocusPercent = 0f;
			}
		}
	}
}
