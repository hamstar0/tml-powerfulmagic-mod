using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCamera.Classes.CameraAnimation;
using ModLibsGeneral.Libraries.Players;


namespace PowerfulMagic {
	public partial class PowerfulMagicItem : GlobalItem {
		public static Color GetTemperatureColor( Color baseColor, float temperature ) {
			float temp = Math.Min( temperature / 100f, 1f );

			return Color.Lerp( baseColor, Color.Red, temp );
		}



		////////////////

		private void ApplyPlayerSpellFx( int damage ) {
			if( !PlayerMovementLibraries.IsOnFloor( Main.LocalPlayer ) ) {
				return;
			}

			float shakePower = (float)damage / 30f;
			float magnitude = MathHelper.Clamp( shakePower, 1f, 15f ) * 0.5f;

			var curr = CameraShaker.Current;
			if( curr != null ) {
				float percent = (float)curr.TicksElapsed / (float)curr.TotalTickDuration;
				percent = 1f - percent;

				if( (percent * curr.ShakePeakMagnitude) > magnitude ) {
					return;
				}
			}

			CameraShaker.Current = new CameraShaker(
				name: "PowerfulMagicFx",
				peakMagnitude: magnitude,
				toDuration: 0,
				lingerDuration: 1,
				froDuration: 10 + (int)( (magnitude - 0.5f) * 10f ),
				isSmoothed: false
			);
		}
	}
}
