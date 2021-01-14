using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		private int OldHoldStyle = 0;


		////////////////

		public bool IsFocusing { get; private set; } = false;


		////

		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances => false;



		////////////////

		public override void SetDefaults( Item item ) {
			if( !this.SetDefaultsForManaPickup(item) ) {
				this.SetDefaultsForPrefix( item );
			}
		}

		////

		private void SetDefaultsForPrefix( Item item ) {
			var config = PowerfulMagicConfig.Instance;

			if( config.Get<bool>( nameof( config.RemoveItemArcanePrefix ) ) ) {
				while( item.prefix == PrefixID.Arcane ) {//?
					item.Prefix( -1 );
				}
			}
		}

		private bool SetDefaultsForManaPickup( Item item ) {
			if( item.type != ItemID.Star && item.type != ItemID.SoulCake && item.type != ItemID.SugarPlum ) {
				return true;
			}
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return true;
			}
			if( Main.item.Any(i => i == item) ) {
				return true;
			}

			var config = PowerfulMagicConfig.Instance;

			float manaStarDropPerc = config.Get<float>( nameof( config.ManaStarDropChancePercentOfVanilla ) );
			bool iAmAir = manaStarDropPerc <= Main.rand.NextFloat();

			if( iAmAir ) {
				item.TurnToAir();
			}
			return !iAmAir;
		}


		////////////////

		public override void HoldStyle( Item item, Player player ) {
			this.UpdateFocusHoldStyle( item, player );
		}


		////////////////

		public override void ModifyManaCost( Item item, Player player, ref float reduce, ref float mult ) {
			reduce *= PowerfulMagicConfig.Instance.Get<float>( nameof(PowerfulMagicConfig.WeaponManaConsumeMulitplier) );

			if( player.controlUseItem && player.whoAmI == Main.myPlayer ) {
				int manaUse = (int)( (float)item.mana * reduce * mult );
				if( manaUse > 0 && manaUse <= player.statMana ) {
					int buffIdx = player.FindBuffIndex( BuffID.ManaSickness );
					int manaSicknessDuration = buffIdx < 0 ? 0 : player.buffTime[buffIdx];

					float dmgScale = PowerfulMagicItem.GetItemDamageScale( item, manaSicknessDuration );
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

		public override bool OnPickup( Item item, Player player ) {
			if( item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum ) {
				this.OnManaPickup( item, player );
			}

			return base.OnPickup( item, player );
		}
	}
}
