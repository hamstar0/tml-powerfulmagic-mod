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
				this.FocusPercent += config.Get<float>( nameof(PowerfulMagicConfig.FocusPercentChargeRatePerTick) );
				if( this.FocusPercent > 1f ) {
					this.FocusPercent = 1f;
				}
			} else {
				this.FocusPercent = 0f;
			}
			
			if( this.FocusPercent > 0f && (Main.GameUpdateCount % 60) == 0 ) {
				float focusChargeRate = config.Get<float>( nameof(PowerfulMagicConfig.FocusManaChargeMaxRatePerSecond) );
				int amt = (int)( this.FocusPercent * focusChargeRate );

				if( amt > 0 ) {
					this.player.statMana += amt;
					CombatText.NewText( this.player.getRect(), CombatText.HealMana, amt );
				}
			}
		}


		////////////////

		private void ApplyFocusMovementEffects() {
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
	}
}
