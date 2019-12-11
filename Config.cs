using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public class PowerfulMagicConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		[Range( 0f, 50f )]
		[DefaultValue(4f)]
		public float DamageScale = 4f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 6f )]
		public float ManaHealScale = 1f / 6f;   //was 5/6?


		[Range( 0f, 20f )]
		[DefaultValue( 2f )]
		public float WeaponManaConsumeMulitplier { get; set; } = 2f;


		[DefaultValue( true )]
		public bool RemoveMerchantLesserPotions { get; set; } = true;
	}
}
