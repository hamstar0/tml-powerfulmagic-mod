using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public class PowerfulMagicConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		[Range( 0f, 50f )]
		[DefaultValue(3f)]
		public float DamageScale { get; set; } = 3f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 5f )]
		public float ManaHealScale { get; set; } = 1f / 5f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 3f )]
		public float MaxManaSicknessDamageScale { get; set; } = 1f / 3f;

		[Range( 1, 301 )]
		[DefaultValue( 180 )]
		public int ManaSicknessMaximumTicksAllowedToEnableAttacks { get; set; } = 180;


		[Range( 0f, 20f )]
		[DefaultValue( 2f )]
		public float WeaponManaConsumeMulitplier { get; set; } = 2f;


		[DefaultValue( true )]
		public bool RemoveMerchantLesserPotions { get; set; } = true;
	}
}
