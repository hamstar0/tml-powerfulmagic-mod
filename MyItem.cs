using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( !item.magic ) {
				return;
			}

			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return;
			}

			//int newDmg = Main.LocalPlayer.GetWeaponDamage( item );
			int dmgPercent = (int)(config.DamageScale * 100f);
			int manaPercent = (int)(config.WeaponManaConsumeMulitplier * 100f);

			var tip1 = new TooltipLine( this.mod, "PowerfulMagicDmgUpTip", "Magic increased "+dmgPercent+"% of base amount" );
			tip1.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );
			var tip2 = new TooltipLine( this.mod, "PowerfulMagicManaUpTip", "Mana use increased "+manaPercent+"% of base amount" );
			tip2.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			tooltips.Insert( 1, tip1 );
			tooltips.Insert( 2, tip2 );
		}


		////////////////
		public override void ModifyManaCost( Item item, Player player, ref float reduce, ref float mult ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return;
			}

			reduce *= config.WeaponManaConsumeMulitplier;
		}


		////////////////

		public override void GetHealMana( Item item, Player player, bool quickHeal, ref int healValue ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			float manaMul = 1f - config.ManaHealScale;

			healValue = (int)((float)healValue * manaMul);
		}
		
		//public override bool ConsumeItem( Item item, Player player ) {
		public override void OnConsumeMana( Item item, Player player, int manaConsumed ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return;// false;
			}
			
			if( item.healMana > 0 ) {
				/*float manaMul = 1f - config.ManaScale;

				player.statMana -= (int)( (float)item.healMana * manaMul );
				player.statMana = player.statMana < 0 ? 0 : player.statMana;*/

				for( int idx=0; idx < Main.combatText.Length; idx++ ) {
					CombatText txt = Main.combatText[idx];
					if( txt == null || !txt.active ) { continue; }
				
					if( txt.text.Equals(item.healMana+"") ) {
						txt.text = (int)((float)item.healMana * config.ManaHealScale) + "";
						break;
					}
				}
			}
			//return true;
		}


		////

		public override bool OnPickup( Item item, Player player ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return false;
			}

			if( item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum ) {
				float manaMul = 1f - config.ManaHealScale;
				
				player.statMana -= (int)(100f * manaMul);
				player.statMana = player.statMana < 0 ? 0 : player.statMana;

				var myplayer = player.GetModPlayer<PowerfulMagicPlayer>();
				myplayer.RecentPickup = true;
			}

			return true;
		}
	}
}
