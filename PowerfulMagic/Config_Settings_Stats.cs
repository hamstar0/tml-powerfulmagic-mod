using System;
using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace PowerfulMagic {
	public partial class PowerfulMagicConfig : ModConfig {
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

		[Range( 1, 601 )]
		[DefaultValue( (int)(60f * 5.5f) + 1 )]
		public int ManaSicknessMaximumTicksAllowedToEnableAttacks { get; set; } = (int)(60f * 5.5f) + 1;

		//

		[Range( 0f, 1f )]
		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float ManaStarDropChancePercentOfVanilla { get; set; } = 0.15f;

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
	}
}
