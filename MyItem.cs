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

			//int newDmg = (int)((config?.DamageScale ?? 1f) * (float)Main.LocalPlayer.GetWeaponDamage(item));
			int newDmg = Main.LocalPlayer.GetWeaponDamage( item );
			int percent = (int)config.DamageScale * 100;

			var tip = new TooltipLine( this.mod, "PowerfulMagicTip", "Magic scaled "+percent+"%: "+newDmg );
			tip.overrideColor = Color.Lerp( Color.Black, Color.Lime, 0.8f + (0.2f * mymod.Oscillate) );

			tooltips.Insert( 1, tip );
		}


		////////////////

		public override bool ConsumeItem( Item item, Player player ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return false;
			}
			
			if( item.healMana > 0 ) {
				//float dmgMul = config?.DamageScale ?? 1f;
				float manaMul = 1f - config.ManaScale;

				//player.AddBuff( BuffID.ManaSickness, 60 * 5 * (int)( dmgMul * 0.5f ) );
				player.statMana -= (int)( (float)item.healMana * manaMul );

				for( int idx=0; idx < Main.combatText.Length; idx++ ) {
					CombatText txt = Main.combatText[idx];
					if( txt == null || !txt.active ) { continue; }
				
					if( txt.text.Equals(item.healMana+"") ) {
						txt.text = (int)((float)item.healMana * config.ManaScale) + "";
						break;
					}
				}
			}

			return true;
		}


		////

		public override bool OnPickup( Item item, Player player ) {
			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return false;
			}

			if( item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum ) {
				float manaMul = 1f - config.ManaScale;
				
				player.statMana -= (int)(100f * manaMul);

				var myplayer = player.GetModPlayer<PowerfulMagicPlayer>();
				myplayer.RecentPickup = true;
			}

			return true;
		}
	}
}
