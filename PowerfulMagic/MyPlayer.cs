using System;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		public float FocusPercent { get; private set; } = 0f;

		public bool RecentPickup { get; internal set; } = false;

		public int ManaBeforePickup { get; internal set; } = -1;


		////////////////

		public override bool CloneNewInstances => false;



		////////////////

		public override void PreUpdate() {
			if( this.RecentPickup ) {
				this.RecentPickup = false;
				this.RecentManaStarPickup();
			}

			if( this.player.whoAmI == Main.myPlayer ) {
				this.PreUpdateLocal();
			}
		}

		private void PreUpdateLocal() {
			this.UpdateFocusManaRegen();
		}


		////////////////

		public override bool PreItemCheck() {
			Item heldItem = this.player.HeldItem;
			if( heldItem == null || heldItem.IsAir || !heldItem.magic ) {
				return base.PreItemCheck();
			}

			return this.IsMagicItemAllowedForUse();
		}


		////////////////

		public override void PreUpdateBuffs() {
			this.UpdateManaRegen();
		}


		////////////////

		public override void ModifyWeaponDamage( Item item, ref float directScale, ref float afterScale, ref float flat ) {
			if( !item.magic ) {
				return;
			}

			this.ModifyMagicWeaponDamage( item, ref afterScale );
		}
	}
}
