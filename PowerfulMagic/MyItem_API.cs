using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public static float GetItemDamageScale( Item item, int manaSicknessTicks ) {
			if( !item.magic ) {
				return 1f;
			}

			var config = PowerfulMagicConfig.Instance;

			float scale = ((float)manaSicknessTicks / 300f) * config.MaxManaSicknessDamageScale;
			scale = 1f - scale;
			if( scale <= 0.25f ) {
				return 0.25f;
			}

			if( item.type == ItemID.SpaceGun ) {
				if( item.owner != -1 && Main.player[item.owner]?.active == true ) {
					Player plr = Main.player[item.owner];

					// Laser weapons + meteor armor = no mana = no damage increase
					if( plr.armor[0].type == ItemID.MeteorHelmet
						&& plr.armor[1].type == ItemID.MeteorSuit
						&& plr.armor[2].type == ItemID.MeteorLeggings ) {
						return scale;
					}
				}
			}

			var itemDef = new ItemDefinition( item.type );
			if( config.PerItemDamageScale.ContainsKey(itemDef) ) {
				return config.PerItemDamageScale[itemDef].Scale * scale;
			}

			return config.BaseDamageScale * scale;
		}


		////////////////

		internal static void OnManaPickup( Player player, int manaBeforePickup ) {
			var config = PowerfulMagicConfig.Instance;

			int newMana = (int)( manaBeforePickup + ( 100f * config.ManaHealScale ) );
			player.statMana = Math.Min( newMana, player.statManaMax2 );
		}
	}
}
