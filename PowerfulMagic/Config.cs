using System;
using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace PowerfulMagic {
	class MyFloatInputElement : FloatInputElement { }




	public class ItemMagicScale {
		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float Scale { get; set; } = 1f;
	}




	public class PowerfulMagicConfig : ModConfig {
		public static PowerfulMagicConfig Instance => ModContent.GetInstance<PowerfulMagicConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////
		
		public bool DebugModeInfo { get; set; } = false;


		////

		[Range( 0f, 100f )]
		[DefaultValue(3f)]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BaseDamageScale { get; set; } = 3f;

		//

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ManaHealScale { get; set; } = 1f / 5f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 10f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ManaRegenScale { get; set; } = 1f / 10f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 3f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float MaxManaSicknessDamageScale { get; set; } = 1f / 3f;

		[Range( 1, 301 )]
		[DefaultValue( 180 )]
		public int ManaSicknessMaximumTicksAllowedToEnableAttacks { get; set; } = 180;

		//

		public Dictionary<ItemDefinition, ItemMagicScale> PerItemDamageScale { get; set; } = new Dictionary<ItemDefinition, ItemMagicScale> {
			{ new ItemDefinition(ItemID.Vilethorn), new ItemMagicScale{ Scale = 2f } },
			{ new ItemDefinition(ItemID.CrimsonRod), new ItemMagicScale{ Scale = 1.5f } },
			{ new ItemDefinition(ItemID.ClingerStaff), new ItemMagicScale{ Scale = 1.5f } },
			{ new ItemDefinition(ItemID.WaterBolt), new ItemMagicScale{ Scale = 1.5f } },
			{ new ItemDefinition(ItemID.NimbusRod), new ItemMagicScale{ Scale = 1.5f } },
			{ new ItemDefinition(ItemID.MagnetSphere), new ItemMagicScale{ Scale = 2f } },
			{ new ItemDefinition(ItemID.MagicalHarp), new ItemMagicScale{ Scale = 2f } }
		};

		//

		[Range( 0f, 20f )]
		[DefaultValue( 2.5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float WeaponManaConsumeMulitplier { get; set; } = 2.5f;


		//

		[DefaultValue( true )]
		public bool RemoveMerchantLesserPotions { get; set; } = true;


		//
		
		[Label("Remove any Arcane prefix of spawned items; permanent")]
		[DefaultValue( true )]
		public bool RemoveItemArcanePrefix { get; set; } = true;


		//

		[Label( "Focus percent increase per tick while focusing" )]
		[Range( 0f, 1f )]
		[DefaultValue( 1f / (60f * 5f) )]	// 5 seconds until max
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FocusPercentChargeRatePerTick { get; set; } = 1f / (60f * 5f);

		[Label( "Max focus mana charge amount per quarter second" )]
		[Range( 0f, 200f )]
		[DefaultValue( 2.5f )] // 10 mana per second
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float FocusManaChargeMaxRatePerSecond { get; set; } = 2.5f;

		[Range( 0f, 10f )]
		[DefaultValue( 0.25f )] // 25% speed
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float FocusMoveSpeedScale { get; set; } = 0.25f;

		[Range( 0f, 10f )]
		[DefaultValue( 0.25f )] // 25% height
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float FocusJumpScale { get; set; } = 0.25f;



		////////////////

		public override ModConfig Clone() {
			var clone = (PowerfulMagicConfig)base.Clone();
			clone.PerItemDamageScale = new Dictionary<ItemDefinition, ItemMagicScale>( this.PerItemDamageScale );
			return clone;
		}
	}
}
