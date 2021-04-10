using System;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		public void UpdateManaRegen() { //UpdateLifeRegen
			var config = PowerfulMagicConfig.Instance;

			this.player.nebulaManaCounter -= this.player.nebulaLevelMana / 2;

			if( this.player.manaRegenCount > 0 ) {
				float mul = config.Get<float>( nameof(PowerfulMagicConfig.ManaRegenScale) );
				mul = 1f - mul;

				if( PowerfulMagicConfig.Instance.DebugModeInfo ) {
					//DebugHelpers.Print( "manaregen", "Old mana regen amount: "+this.player.manaRegenCount );
				}

				this.player.manaRegenCount -= (int)( (float)this.player.manaRegen * mul );
			}
		}

		////////////////

		private void RecentManaStarPickup() {
			var config = PowerfulMagicConfig.Instance;

			PowerfulMagicItem.OnManaPickup( this.player, this.ManaBeforePickup );

			for( int idx = 0; idx < Main.combatText.Length; idx++ ) {
				CombatText txt = Main.combatText[idx];
				if( txt == null || !txt.active ) {
					continue;
				}
				if( !txt.text.Equals("100") ) {
					continue;
				}

				if( PowerfulMagicConfig.Instance.DebugModeInfo ) {
					Main.NewText( "Old mana heal? amount from recent pickup: 100" );
				}

				float manaHealScale = config.Get<float>( nameof(PowerfulMagicConfig.ManaHealScale) );

				txt.text = (int)( (float)100 * manaHealScale ) + "";
				break;
			}
		}
	}
}
