using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Services.Timers;


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

			if( !this.IsMagicItemAllowedForUse() ) {
				if( this.player.controlUseItem ) {
					if( Timers.GetTimerTickDuration( "PowerfulMagicManaSicknessAlert" ) == 0 ) {
						Main.NewText( "Too much mana sickness.", Color.Yellow );
					}
					Timers.SetTimer( "PowerfulMagicManaSicknessAlert", 60 * 5, true, () => false );
				}

				return false;
			}
			return true;
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

			if( this.player.velocity.Y != 0 ) {
				var config = PowerfulMagicConfig.Instance;

				if( config.Get<bool>( nameof(config.FocusInterruptsOnMove) ) ) {
					this.FocusPercent = 0f;
				}
			}
		}
		

		////////////////

		public override void PostHurt( bool pvp, bool quiet, double damage, int hitDirection, bool crit ) {
			if( !quiet ) {
				var config = PowerfulMagicConfig.Instance;
				
				if( config.Get<bool>( nameof(config.FocusInterruptsOnHurt) ) ) {
					this.FocusPercent = 0f;
				}
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
