using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		private int OldHoldStyle = 0;

		private bool DestroyMe = false;

		internal float Temperature = 0f;


		////////////////

		public bool IsFocusing { get; private set; } = false;


		////

		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => false;



		////////////////

		public override GlobalItem Clone( Item item, Item itemClone ) {
			var myitem = (PowerfulMagicItem)base.Clone( item, itemClone );
			myitem.OldHoldStyle = this.OldHoldStyle;
			myitem.DestroyMe = this.DestroyMe;
			myitem.IsFocusing = this.IsFocusing;

			return myitem;
		}

		public override void SetDefaults( Item item ) {
			switch( item.type ) {
			case ItemID.Star:
			case ItemID.SoulCake:
			case ItemID.SugarPlum:
				this.SetDefaultsForManaPickup( item );
				break;
			default:
				this.SetDefaultsForPrefix( item );
				break;
			}
		}

		////

		private void SetDefaultsForPrefix( Item item ) {
			var config = PowerfulMagicConfig.Instance;
			if( !config.Get<bool>( nameof(config.RemoveItemArcanePrefix) ) ) {
				return;
			}

			while( item.prefix == PrefixID.Arcane ) {//?
				item.Prefix( -1 );
			}
		}

		private void SetDefaultsForManaPickup( Item item ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			int mainIdx = Array.FindIndex( Main.item, i => i == item );
			if( mainIdx == -1 ) {
				return;
			}

			var config = PowerfulMagicConfig.Instance;

			float manaStarDropPerc = config.Get<float>( nameof(config.ManaStarDropChancePercentOfVanilla) );
			bool iAmAir = Main.rand.NextFloat() > manaStarDropPerc;
			
			if( iAmAir ) {
				this.DestroyMe = true;
			}
		}


		////////////////

		public override void NetSend( Item item, BinaryWriter writer ) {
			switch( item.type ) {
			case ItemID.Star:
			case ItemID.SoulCake:
			case ItemID.SugarPlum:
				writer.Write( this.DestroyMe );
				break;
			}
		}

		public override void NetReceive( Item item, BinaryReader reader ) {
			switch( item.type ) {
			case ItemID.Star:
			case ItemID.SoulCake:
			case ItemID.SugarPlum:
				this.DestroyMe = reader.ReadBoolean();
				break;
			}
		}


		////////////////

		public override void Update( Item item, ref float gravity, ref float maxFallSpeed ) {
			if( this.DestroyMe ) {
				int mainIdx = Array.FindIndex( Main.item, i => i == item );

				if( Main.item[mainIdx].active && Main.item[mainIdx].type == item.type ) {
					item.active = false;
					Main.item[mainIdx] = new Item();
				}
			}
		}


		////////////////

		public override void HoldStyle( Item item, Player player ) {
			this.UpdateFocusHoldStyle( item, player );
		}


		////////////////

		public override void ModifyManaCost( Item item, Player player, ref float reduce, ref float mult ) {
			if( !item.magic ) {
				return;
			}

			reduce *= PowerfulMagicConfig.Instance.Get<float>( nameof(PowerfulMagicConfig.WeaponManaConsumeMulitplier) );

			if( player.controlUseItem && player.whoAmI == Main.myPlayer ) {
				int manaUse = (int)( (float)item.mana * reduce * mult );

				if( manaUse > 0 && manaUse <= player.statMana ) {
					int buffIdx = player.FindBuffIndex( BuffID.ManaSickness );
					int manaSicknessDuration = buffIdx < 0 ? 0 : player.buffTime[buffIdx];

					float dmgScale = PowerfulMagicItem.GetItemDamageScale( item, manaSicknessDuration )
						?? 1f;
					int damage = (int)( (float)item.damage * dmgScale );

					this.ApplyPlayerSpellFx( damage );
				}
			}
		}


		////////////////

		public override void GetHealMana( Item item, Player player, bool quickHeal, ref int healValue ) {
			var config = PowerfulMagicConfig.Instance;

			if( config.DebugModeInfo ) {
				Main.NewText("Old mana heal value for "+item.Name+": "+healValue);
			}

			healValue = (int)( (float)healValue * config.Get<float>(nameof(PowerfulMagicConfig.ManaHealScale)) );
		}

		//public override bool ConsumeItem( Item item, Player player ) {
		public override void OnConsumeMana( Item item, Player player, int manaConsumed ) {
			if( item.healMana > 0 && player.whoAmI == Main.myPlayer ) {
				this.OnConsumeManaMessage( item );
			}
		}


		////

		public override bool CanPickup( Item item, Player player ) {
			return !this.DestroyMe;
		}

		public override bool OnPickup( Item item, Player player ) {
			if( item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum ) {
				this.OnManaPickup( item, player );
			}

			return base.OnPickup( item, player );
		}


		////////////////

		public override Color? GetAlpha( Item item, Color lightColor ) {
			if( this.Temperature > 0 ) {
				return PowerfulMagicItem.GetTemperatureColor( lightColor, this.Temperature );
			}

			return base.GetAlpha( item, lightColor );
		}
		//public override bool PreDrawInInventory( Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale ) {
		//}
	}
}
