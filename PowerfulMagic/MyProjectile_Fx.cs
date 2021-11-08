using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;
using ModLibsGeneral.Libraries.Projectiles;


namespace PowerfulMagic {
	partial class PowerfulMagicProjectile : GlobalProjectile {
		/*public static Color GetRenderColors( Projectile proj, Color drawColor, int intensity ) {
			Color baseColor = proj.GetAlpha( drawColor );

			float avg = (float)( baseColor.R + baseColor.G + baseColor.B + baseColor.A ) / 4f;
			float scale = intensity / avg;

			return Color.Multiply( baseColor, scale );
		}*/



		////////////////

		private void UpdateFx( Projectile proj ) {
			for( int i = this.CurrentTrailLength; i >= 1; i-- ) {
				this.TrailPositions[i] = this.TrailPositions[i - 1];
				this.TrailRotations[i] = this.TrailRotations[i - 1];
			}

			this.TrailPositions[0] = proj.Center;
			this.TrailRotations[0] = proj.rotation;

			if( this.CurrentTrailLength < (this.TrailPositions.Length - 1) ) {
				this.CurrentTrailLength++;
			}
		}


		////////////////

		public void RenderTrail( SpriteBatch sb, Projectile proj, Color lightColor ) {
			float intensity = Math.Min( 255f, 6f * proj.velocity.Length() );
			if( intensity <= 8f ) {
				return;
			}

			//

			//Color mainColor = PowerfulMagicProjectile.GetRenderColors( proj, lightColor, intensity );
			Color mainColor;
			if( proj.GetAlpha(lightColor) == lightColor ) { // non-illuminant projectile
				mainColor = lightColor * (intensity / 255f);
			} else {
				mainColor = new Color( intensity, intensity, intensity, intensity );
			}

			//

//DebugLibraries.Print( "trail", "intensity: " + intensity+", c1: "+proj.GetAlpha(lightColor)+", c2: "+lightColor );
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];

				float perc = (float)i / (float)this.CurrentTrailLength;
				float farPerc = perc + ((1f - perc) * perc);
				float farFarPerc = farPerc + ((1f - farPerc) * perc);

				Color color = Color.Lerp( mainColor, Color.Transparent, farFarPerc );

//DebugLibraries.Print( "trail_"+i, "color: "+new Color(color) );
				if( color.A <= 8f ) {
					break;
				}

				float scale = MathHelper.Lerp( proj.scale, proj.scale * 3f, farPerc );

				//

				ProjectileLibraries.DrawSimple( sb, proj, pos, rot, color, scale );
			}
		}
	}
}
