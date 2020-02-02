using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public class PowerfulMagicItem : GlobalItem {
		public static float GetItemDamageScale( Item item ) {
			if( !item.magic ) {
				return 1f;
			}

			var mymod = PowerfulMagicMod.Instance;
			var config = mymod.Config;

			if( item.type == ItemID.SpaceGun ) {
				if( item.owner != -1 && Main.player[item.owner]?.active == true ) {
					Player plr = Main.player[item.owner];

					// Laser weapons + meteor armor = no mana = no damage increase
					if( plr.armor[0].type == ItemID.MeteorHelmet
						&& plr.armor[1].type == ItemID.MeteorSuit
						&& plr.armor[2].type == ItemID.MeteorLeggings ) {
						return 1f;
					}
				}
			}

			var itemDef = new ItemDefinition( item.type );

			if( config.PerItemDamageScale.ContainsKey(itemDef) ) {
				return config.PerItemDamageScale[itemDef];
			}

			return config.DamageScale;
		}


		////////////////

		internal static void OnManaPickup( Player player, int manaBeforePickup ) {
			var mymod = PowerfulMagicMod.Instance;
			var config = mymod.Config;

			int newMana = (int)( manaBeforePickup + ( 100f * config.ManaHealScale ) );
			player.statMana = Math.Min( newMana, player.statManaMax2 );
		}



		////////////////
		
		public override void SetDefaults( Item item ) {
			if( PowerfulMagicMod.Instance.Config.RemoveItemArcanePrefix ) {
				while( item.prefix == PrefixID.Arcane ) {
					item.Prefix( -1 );
				}
			}
		}


		////////////////

		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( !item.magic ) {
				return;
			}

			float dmgScale = PowerfulMagicItem.GetItemDamageScale( item );
			if( dmgScale == 1f ) {
				return;
			}

			var mymod = (PowerfulMagicMod)this.mod;
			var config = mymod.Config;
			if( config == null ) {
				return;
			}

			//int newDmg = Main.LocalPlayer.GetWeaponDamage( item );
			int dmgPercent = (int)(dmgScale * 100f);
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

			if( config.DebugModeInfo ) {
				Main.NewText("Old mana heal value for "+item.Name+": "+healValue);
			}

			healValue = (int)((float)healValue * config.ManaHealScale);
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
			var mymod = (PowerfulMagicMod)this.mod;
			if( mymod.Config == null ) {
				return base.OnPickup( item, player );
			}

			if( item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum ) {
				if( mymod.Config.DebugModeInfo ) {
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
