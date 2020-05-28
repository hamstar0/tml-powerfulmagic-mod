using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
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
				return config.PerItemDamageScale[itemDef].Scale;
			}

			return config.BaseDamageScale;
		}


		////////////////

		internal static void OnManaPickup( Player player, int manaBeforePickup ) {
			var mymod = PowerfulMagicMod.Instance;
			var config = mymod.Config;

			int newMana = (int)( manaBeforePickup + ( 100f * config.ManaHealScale ) );
			player.statMana = Math.Min( newMana, player.statManaMax2 );
		}
	}
}
