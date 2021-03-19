using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;


namespace PowerfulMagic {
	public partial class PowerfulMagicConfig : ModConfig {
		public bool DebugModeInfo { get; set; } = false;


		////////////////

		[DefaultValue( true )]
		public bool RemoveMerchantLesserPotions { get; set; } = true;

		[DefaultValue( true )]
		public bool ReplaceWizardGreaterPotions { get; set; } = true;


		//
		
		[Label("Remove any Arcane prefix of spawned items; permanent")]
		[DefaultValue( true )]
		public bool RemoveItemArcanePrefix { get; set; } = true;
	}
}
