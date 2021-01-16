using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PowerfulMagic.Items;


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
			var config = PowerfulMagicConfig.Instance;

			if( type == NPCID.Merchant ) {
				if( config.Get<bool>( nameof(config.RemoveMerchantLesserPotions) ) ) {
					PowerfulMagicNPC.FilterShop( shop.item, new HashSet<int> { ItemID.LesserManaPotion }, ref nextSlot );
				}
			} else if( type == NPCID.Wizard ) {
				if( config.Get<bool>( nameof(config.ReplaceWizardGreaterPotions) ) ) {
					PowerfulMagicNPC.FilterShop( shop.item, new HashSet<int> { ItemID.GreaterManaPotion }, ref nextSlot );

					var concentrateItem = new Item();
					concentrateItem.SetDefaults( ModContent.ItemType<ManaPotionConcentrateItem>() );
					shop.item[ nextSlot++ ] = concentrateItem;
				}
			}
		}
	}
}
