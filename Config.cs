using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public class PowerfulMagicConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		[Range( 0f, 50f )]
		[DefaultValue(6f)]
		public float DamageScale = 6f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 6f )]
		public float ManaScale = 1f / 6f;	//was 5/6?
	}
}
