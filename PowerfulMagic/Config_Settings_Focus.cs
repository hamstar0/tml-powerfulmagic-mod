using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public partial class PowerfulMagicConfig : ModConfig {
		[Label( "Mana recharge per second rate increase amount while focusing" )]
		[Range( 0f, 1f )]
		[DefaultValue( 1f / 3.5f )]	// 3.5 seconds until max
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FocusManaChargeRatePerSecondIncrease { get; set; } = 1f / 3.5f;

		[Label( "Max focus mana charge amount per second" )]
		[Range( 0f, 200f )]
		[DefaultValue( 5f )] // 5 mana per second
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float FocusManaChargeMaxRatePerSecond { get; set; } = 5f;

		[Label( "Movement speed percent while in focus mode" )]
		[Range( 0f, 10f )]
		[DefaultValue( 0.25f )] // 25% speed
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float FocusMoveSpeedScale { get; set; } = 0.25f;

		[Label( "Movement jump height percent while in focus mode" )]
		[Range( 0f, 10f )]
		[DefaultValue( 0.25f )] // 25% height
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float FocusJumpScale { get; set; } = 0.25f;
		
		[Label("Set focus mode to interrupt on movement")]
		[DefaultValue( true )]
		public bool FocusInterruptsOnMove { get; set; } = true;

		[Label( "Set focus mode to interrupt when hurt" )]
		[DefaultValue( true )]
		public bool FocusInterruptsOnHurt { get; set; } = true;
	}
}
