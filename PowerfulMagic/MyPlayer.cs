using System;
using Microsoft.Xna.Framework;
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

		public override void PostUpdateRunSpeeds() {
			if( this.FocusPercent > 0f ) {
				this.ApplyFocusMovementEffects();
			}
		}


		////////////////

		public override void ModifyWeaponDamage( Item item, ref float directScale, ref float afterScale, ref float flat ) {
			if( !item.magic ) {
				return;
			}

			this.ModifyMagicWeaponDamage( item, ref afterScale );
		}

		////

		public override bool Shoot( Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack ) {
			if( item.magic ) {
				this.FocusPercent = 0f;
			}
			return base.Shoot( item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack );
		}
	}
}
