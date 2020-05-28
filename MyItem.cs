using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public override void SetDefaults( Item item ) {
			if( PowerfulMagicConfig.Instance.RemoveItemArcanePrefix ) {
				while( item.prefix == PrefixID.Arcane ) {
					item.Prefix( -1 );
				}
			}
		}


		////////////////

		public override void ModifyManaCost( Item item, Player player, ref float reduce, ref float mult ) {
			var config = PowerfulMagicConfig.Instance;
			if( config == null ) {
				return;
			}

			reduce *= config.WeaponManaConsumeMulitplier;
		}


		////////////////

		public override void GetHealMana( Item item, Player player, bool quickHeal, ref int healValue ) {
			var config = PowerfulMagicConfig.Instance;
			if( config == null ) {
				return;
			}

			if( config.DebugModeInfo ) {
				Main.NewText("Old mana heal value for "+item.Name+": "+healValue);
			}

			healValue = (int)((float)healValue * config.ManaHealScale);
		}
		
		//public override bool ConsumeItem( Item item, Player player ) {
		public override void OnConsumeMana( Item item, Player player, int manaConsumed ) {
			var config = PowerfulMagicConfig.Instance;
			if( config == null ) {
				return;
			}
			
			if( item.healMana > 0 ) {
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

						txt.text = (int)((float)item.healMana * config.ManaHealScale) + "";
						break;
					}
				}
			}
			//return true;
		}


		////

		public override bool OnPickup( Item item, Player player ) {
			var config = PowerfulMagicConfig.Instance;
			if( config == null ) {
				return base.OnPickup( item, player );
			}

			if( item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum ) {
				if( config.DebugModeInfo ) {
					Main.NewText( "Old mana heal amount on pickup of " + item.Name + ": 100" );
				}

				var myplayer = player.GetModPlayer<PowerfulMagicPlayer>();
				myplayer.RecentPickup = true;
				myplayer.ManaBeforePickup = player.statMana;
			}

			return base.OnPickup( item, player );
		}
	}
}
