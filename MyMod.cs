using System;
using Terraria;
using Terraria.ModLoader;


namespace PowerfulMagic {
	partial class PowerfulMagicMod : Mod {
		public static PowerfulMagicMod Instance { get; private set; }



		////////////////

		public PowerfulMagicConfig Config => this.GetConfig<PowerfulMagicConfig>();

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
