using System;
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



		////////////////

		public override GlobalItem NewInstance( Item item ) {
			return base.NewInstance( item );
		}

		////

		public override void SetDefaults( Item item ) {
			if( PowerfulMagicConfig.Instance.RemoveItemArcanePrefix ) {
				while( item.prefix == PrefixID.Arcane ) {//?
					item.Prefix( -1 );
				}
			}
		}


		////////////////

		public override void HoldStyle( Item item, Player player ) {
			this.UpdateFocusHoldStyle( item, player );
		}


		////////////////

		public override void ModifyManaCost( Item item, Player player, ref float reduce, ref float mult ) {
			reduce *= PowerfulMagicConfig.Instance.WeaponManaConsumeMulitplier;

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

			healValue = (int)((float)healValue * config.ManaHealScale);
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
