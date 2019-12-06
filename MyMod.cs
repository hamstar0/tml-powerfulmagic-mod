using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicMod : Mod {
		public static PowerfulMagicMod Instance { get; private set; }



		////////////////

		public PowerfulMagicConfig Config => ModContent.GetInstance<PowerfulMagicConfig>();

		public float Oscillate { get; private set; } = 0f;


		////////////////

		private bool OscillateDir = false;



		////////////////

		public PowerfulMagicMod() {
			PowerfulMagicMod.Instance = this;
		}

		public override void Load() {
			PowerfulMagicMod.Instance = this;
		}

		public override void Unload() {
			PowerfulMagicMod.Instance = null;
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

		internal void RunOscillation() {
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
