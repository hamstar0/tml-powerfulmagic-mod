using PowerfulMagic.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	class PowerfulMagicNPC : GlobalNPC {
		public override void SetupShop( int type, Chest shop, ref int nextSlot ) {
			if( type == NPCID.Merchant ) {
				if( PowerfulMagicMod.Instance.Config.RemoveMerchantLesserPotions ) {
					this.SetupMerchantShop( shop );
				}
			} else if( type == NPCID.Wizard ) {
				this.SetupWizardShop( shop );
			}
		}


		private void SetupMerchantShop( Chest shop ) {
			bool lmpRemoved = false;

			for( int i = 0; i < shop.item.Length; i++ ) {
				Item item = shop.item[i];

				if( !lmpRemoved ) {
					if( item == null || item.IsAir || item.type != ItemID.LesserManaPotion ) {
						continue;
					}

					lmpRemoved = true;
					shop.item[i] = new Item();
				} else {
					if( i < shop.item.Length - 1 ) {
						shop.item[i] = shop.item[i + 1];
					}
				}
			}
		}

		private void SetupWizardShop( Chest shop ) {
			for( int i = 0; i < shop.item.Length; i++ ) {
				Item item = shop.item[i];
				if( item == null || item.IsAir || item.type != ItemID.GreaterManaPotion ) {
					continue;
				}

				shop.item[i] = new Item();
				shop.item[i].SetDefaults( ModContent.ItemType<ManaPotionConcentrateItem>() );
				break;
			}
		}
	}
}
