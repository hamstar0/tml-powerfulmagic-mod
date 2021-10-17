using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Services.Timers;


namespace PowerfulMagic {
	partial class PowerfulMagicPlayer : ModPlayer {
		private static void MessageAboutFocus() {
			Messages.MessagesAPI.AddMessagesCategoriesInitializeEvent( () => {
				string id = "PowerfulMagicFocus";

				Messages.MessagesAPI.AddMessage(
					title: "Use \"focus\" to recharge mana",
					description: "Hold right-click when equipping a magic weapon to \"focus\" and recharge mana.",
					modOfOrigin: PowerfulMagicMod.Instance,
					id: id,
					parentMessage: Messages.MessagesAPI.HintsTipsCategoryMsg,
					alertPlayer: Messages.MessagesAPI.IsUnread(id),
					isImportant: false
				);
			} );
		}



		////////////////

		internal int ClaimNextMagicProjectile = 0;


		////////////////
		
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

			if( Main.netMode != NetmodeID.Server ) {
				if( this.player.whoAmI == Main.myPlayer ) {
					this.PreUpdateLocal();
				}
			}

			this.UpdateMeteorArmorIf();
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

			if( ModLoader.GetMod( "Messages" ) != null ) {
				PowerfulMagicPlayer.MessageAboutFocus();
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
				this.ApplyFocusMovementBehavior();
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

				this.ApplyMeteorArmorShootBehaviorIf( item );
			}

			int manaSicknessBuffIdx = this.player.FindBuffIndex( BuffID.ManaSickness );
			int manaSicknessTicks = manaSicknessBuffIdx != -1
				? this.player.buffTime[manaSicknessBuffIdx]
				: 0;
			float? scale = PowerfulMagicItem.GetItemDamageScale( item, manaSicknessTicks );
			if( scale.HasValue ) {
				this.ClaimNextMagicProjectile++;
			}

			return base.Shoot( item, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack );
		}


		////////////////

		public override void ModifyDrawInfo( ref PlayerDrawInfo drawInfo ) {
			this.ApplyMeteorArmorAppearanceIf( ref drawInfo );
		}
	}
}
