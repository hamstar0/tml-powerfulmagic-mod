using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public override Color? GetAlpha( Item item, Color lightColor ) {
			if( this.Temperature > 0f ) {
				return PowerfulMagicItem.GetTemperatureColor( lightColor, this.Temperature );
			}

			return base.GetAlpha( item, lightColor );
		}


		////////////////

		public override void PostDrawInInventory(
					Item item,
					SpriteBatch sb,
					Vector2 position,
					Rectangle frame,
					Color drawColor,
					Color itemColor,
					Vector2 origin,
					float scale ) {
			if( this.Temperature <= 0f ) {
				return;
			}

			var mymod = PowerfulMagicMod.Instance;
			Texture2D thermTex = mymod.ThermoTex;
			Texture2D thermBarTex = mymod.ThermoBarTex;

			var offset = new Vector2( -4f, 0f ) * scale;
			var barOffset = new Vector2( 2f, 2f ) * scale;

			float tempPerc = Math.Min( this.Temperature / 100f, 1f );
			float invTempPerc = 1f - tempPerc;
			float tempOffset = (float)thermBarTex.Height * tempPerc;
			float invTempOffset = (float)thermBarTex.Height * invTempPerc;

			scale *= 2f;

			sb.Draw(
				texture: thermTex,
				position: position + offset,
				sourceRectangle: null,	//frame
				color: Color.White,
				rotation: 0f,
				origin: default,
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);

			var barFrameOffset = new Vector2( 0f, (int)invTempOffset * scale );
			var barFrame = new Rectangle(
				0,
				(int)invTempOffset,
				thermBarTex.Width,
				(int)tempOffset
			);

			sb.Draw(
				texture: thermBarTex,
				position: position + offset + barOffset + barFrameOffset,
				sourceRectangle: barFrame,	//frame
				color: Color.White,
				rotation: 0f,
				origin: default,
				scale: scale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}
