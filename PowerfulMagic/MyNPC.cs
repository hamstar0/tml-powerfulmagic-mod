using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicNPC : GlobalNPC {
		public static void FilterShop( Item[] shop, ISet<int> itemBlacklist, ref int nextSlot ) {
			for( int i = 0; i < shop.Length; i++ ) {
				Item item = shop[i];
				if( item == null || item.IsAir ) {
					continue;
				}

				if( itemBlacklist.Contains(item.type) ) {
					for( int j = i; j < shop.Length - 1; j++ ) {
						shop[j] = shop[j + 1];
					}
					shop[shop.Length - 1] = new Item();

					nextSlot--;
					i--;
				}
			}
		}



		////////////////

		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			if( type == NPCID.Merchant ) {
				if( PowerfulMagicConfig.Instance.Get<bool>( nameof(PowerfulMagicConfig.RemoveMerchantLesserPotions) ) ) {
					PowerfulMagicNPC.FilterShop( shop.item, new HashSet<int> { ItemID.LesserManaPotion }, ref nextSlot );
				}
			} else if( type == NPCID.Wizard ) {
				PowerfulMagicNPC.FilterShop( shop.item, new HashSet<int> { ItemID.GreaterManaPotion }, ref nextSlot );
			}
		}
	}
}
