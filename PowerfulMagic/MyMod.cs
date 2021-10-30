using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicMod : Mod {
		public static PowerfulMagicMod Instance => ModContent.GetInstance<PowerfulMagicMod>();



		////////////////

		private bool OscillateDir = false;


		////////////////

		public float Oscillate { get; private set; } = 0f;

		public Texture2D ThermoTex { get; private set; }
		public Texture2D ThermoBarTex { get; private set; }



		////////////////

		public override void Load() {
			if( Main.netMode != NetmodeID.Server ) {
				this.ThermoTex = ModContent.GetTexture( "PowerfulMagic/Thermo" );
				this.ThermoBarTex = ModContent.GetTexture( "PowerfulMagic/ThermoBar" );
			}
		}


		////////////////


		public override void AddRecipes() {
			var manaPotRecipe = new ModRecipe( this );
			manaPotRecipe.AddIngredient( ItemID.Bottle, 2 );
			manaPotRecipe.AddIngredient( ItemID.FallenStar, 1 );
			//manaPotRecipe.AddIngredient( ItemID.GlowingMushroom, 1 );
			manaPotRecipe.AddIngredient( ItemID.Gel, 2 );
			manaPotRecipe.AddTile( TileID.Bottles );
			manaPotRecipe.SetResult( ItemID.LesserManaPotion, 2 );
			manaPotRecipe.AddRecipe();
		}


		////////////////

		public override void PostUpdateEverything() {
			this.RunOscillation();
		}


		////////////////

		private void RunOscillation() {
			if( this.OscillateDir ) {
				if( this.Oscillate < 1f ) {
					this.Oscillate += 1f / 60f;
				} else {
					this.Oscillate = 1f;
					this.OscillateDir = !this.OscillateDir;
				}
			} else {
				if( this.Oscillate > 0f ) {
					this.Oscillate -= 1f / 60f;
				} else {
					this.Oscillate = 0f;
					this.OscillateDir = !this.OscillateDir;
				}
			}
		}
	}
}
