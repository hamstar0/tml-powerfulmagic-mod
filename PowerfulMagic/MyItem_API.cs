using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public static float? GetItemDamageScale( Item item, int manaSicknessTicks ) {
			if( !item.magic ) {
				return null;
			}

			//

			// Laser weapons + meteor armor = no mana = no damage increase
			PowerfulMagicItem.IsPoweredUp( item, out bool isTreatedAsSpecialSpaceWeapon );
			if( isTreatedAsSpecialSpaceWeapon ) {
				return null;
			}

			//

			var config = PowerfulMagicConfig.Instance;

			float maxManaSickDmgScale = config.Get<float>( nameof(config.MaxManaSicknessDamageScale) );

			float scale = ((float)manaSicknessTicks / 300f) * maxManaSickDmgScale;
			scale = 1f - scale;

			if( scale <= 0.25f ) {
				return 0.25f;
			}

			//

			var itemDef = new ItemDefinition( item.type );
			var perItemDmgScale = config.Get<Dictionary<ItemDefinition, ItemMagicScale>>(
				nameof(PowerfulMagicConfig.PerItemDamageScale)
			);

			if( perItemDmgScale.ContainsKey(itemDef) ) {
				return perItemDmgScale[itemDef].Scale * scale;
			}

			//

			return config.Get<float>(nameof(PowerfulMagicConfig.BaseDamageScale)) * scale;
		}


		////////////////

		internal static void OnManaPickup( Player player, int manaBeforePickup ) {
			var config = PowerfulMagicConfig.Instance;

			float manaHealScale = config.Get<float>( nameof(PowerfulMagicConfig.ManaHealScale) );
			int newMana = (int)( manaBeforePickup + ( 100f * manaHealScale ) );
			player.statMana = Math.Min( newMana, player.statManaMax2 );
		}
	}
}
