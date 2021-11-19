using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicProjectile : GlobalProjectile {
		public static bool IsPoweredUp( Projectile proj, out bool isTreatedAsSpecialSpaceWeapon ) {
			if( proj.npcProj ) {
				isTreatedAsSpecialSpaceWeapon = false;
				return false;
			}

			switch( proj.type ) {
			case ProjectileID.GreenLaser:
			case ProjectileID.PurpleLaser:
				if( proj.owner < 0 || proj.owner >= 255 ) {
					isTreatedAsSpecialSpaceWeapon = false;
					return false;
				}
				Player ownerPlr = Main.player[ proj.owner ];
				if( ownerPlr?.active != true ) {
					isTreatedAsSpecialSpaceWeapon = false;
					return false;
				}

				//

				Item headItem = ownerPlr.armor[0];
				Item bodyItem = ownerPlr.armor[1];
				Item legsItem = ownerPlr.armor[2];

				isTreatedAsSpecialSpaceWeapon =
					headItem?.active == true && headItem.type == ItemID.MeteorHelmet
					&& bodyItem?.active == true && bodyItem.type == ItemID.MeteorSuit
					&& legsItem?.active == true && legsItem.type == ItemID.MeteorLeggings;

				return !isTreatedAsSpecialSpaceWeapon;
			}

			isTreatedAsSpecialSpaceWeapon = false;
			return proj.magic;
		}



		////////////////

		public const int DramaticFxTrailDetail = 20;



		////////////////

		private Vector2[] TrailPositions;
		private float[] TrailRotations;

		private int CurrentTrailLength = 0;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override void SetDefaults( Projectile projectile ) {
			bool isSpecial;
			
			if( PowerfulMagicProjectile.IsPoweredUp(projectile, out isSpecial) && !isSpecial ) {
				int len = PowerfulMagicProjectile.DramaticFxTrailDetail;

				this.TrailPositions = new Vector2[ len ];
				this.TrailRotations = new float[ len ];

				/*foreach( Player plr in Main.player ) {
					if( plr?.active == true && !plr.dead ) {
						var myplayer = plr.GetModPlayer<PowerfulMagicPlayer>();

						if( myplayer.ClaimNextMagicProjectile >= 1 ) {
							myplayer.ClaimNextMagicProjectile--;

							projectile.scale *= 2f;
						}
					}
				}*/
			}
		}


		////////////////

		public override bool PreDraw( Projectile projectile, SpriteBatch sb, Color lightColor ) {
			if( this.TrailPositions != null ) {		//if( PowerfulMagicProjectile.IsPoweredUp(projectile, out _) ) {
				this.UpdateFx( projectile );

				this.RenderTrail( sb, projectile, lightColor );
			}

			return base.PreDraw( projectile, sb, lightColor );
		}
	}
}
