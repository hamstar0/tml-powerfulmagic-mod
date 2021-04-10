using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		private bool IsMagicItemAllowedForUse() {
			var config = PowerfulMagicConfig.Instance;
			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[manaSicknessBuffIdx]
				: 0;
			int manaSickMaxTicks = config.Get<int>(
				nameof(PowerfulMagicConfig.ManaSicknessMaximumTicksAllowedToEnableAttacks)
			);

			return manaSicknessTicks < manaSickMaxTicks;
		}


		////////////////

		private void ModifyMagicWeaponDamage( Item item, ref float afterScale ) {
			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[ manaSicknessBuffIdx ]
				: 0;

			afterScale *= PowerfulMagicItem.GetItemDamageScale( item, manaSicknessTicks )
				?? 1f;
			//afterScale *= 1f - ((manaSicknessTicks / 300) * PowerfulMagicConfig.Instance.MaxManaSicknessDamageScale);
		}
	}
}
