using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace PowerfulMagic {
	class MyFloatInputElement : FloatInputElement { }




	public class ItemMagicScale {
		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		public float Scale;


		////////////////

		public ItemMagicScale() { }

		public ItemMagicScale( float scale ) {
			this.Scale = scale;
		}
	}




	public class PowerfulMagicConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////////

		public bool DebugModeInfo { get; set; } = false;


		////

		[Range( 0f, 50f )]
		[DefaultValue(3f)]
		public float DamageScale { get; set; } = 3f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 5f )]
		public float ManaHealScale { get; set; } = 1f / 5f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 10f )]
		public float ManaRegenScale { get; set; } = 1f / 10f;

		[Range( 0f, 5f )]
		[DefaultValue( 1f / 3f )]
		public float MaxManaSicknessDamageScale { get; set; } = 1f / 3f;

		[Range( 1, 301 )]
		[DefaultValue( 180 )]
		public int ManaSicknessMaximumTicksAllowedToEnableAttacks { get; set; } = 180;

		//

		public Dictionary<ItemDefinition, ItemMagicScale> PerItemDamageScale { get; set; } = new Dictionary<ItemDefinition, ItemMagicScale> {
			{ new ItemDefinition(ItemID.Vilethorn), new ItemMagicScale(2f) },
			{ new ItemDefinition(ItemID.CrimsonRod), new ItemMagicScale(1.5f) },
			{ new ItemDefinition(ItemID.ClingerStaff), new ItemMagicScale(1.5f) },
			{ new ItemDefinition(ItemID.NimbusRod), new ItemMagicScale(1.5f) },
			{ new ItemDefinition(ItemID.MagnetSphere), new ItemMagicScale(2f) },
			{ new ItemDefinition(ItemID.MagicalHarp), new ItemMagicScale(2f) }
		};

		//

		[Range( 0f, 20f )]
		[DefaultValue( 2.5f )]
		public float WeaponManaConsumeMulitplier { get; set; } = 3f;


		//

		[DefaultValue( true )]
		public bool RemoveMerchantLesserPotions { get; set; } = true;


		//

		[Label("Remove any Arcane prefix of spawned items; permanent")]
		[DefaultValue( true )]
		public bool RemoveItemArcanePrefix { get; set; } = true;
	}
}
