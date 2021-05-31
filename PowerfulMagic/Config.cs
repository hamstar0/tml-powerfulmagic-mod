using System;
using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using ModLibsCore.Classes.UI.ModConfig;


namespace PowerfulMagic {
	class MyFloatInputElement : FloatInputElement { }




	public class ItemMagicScale {
		[Range( 0f, 100f )]
		[DefaultValue( 1f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float Scale { get; set; } = 1f;
	}




	public partial class PowerfulMagicConfig : ModConfig {
		public static PowerfulMagicConfig Instance => ModContent.GetInstance<PowerfulMagicConfig>();



		////////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;



		////////////////

		public override ModConfig Clone() {
			var clone = (PowerfulMagicConfig)base.Clone();
			clone.PerItemDamageScale = new Dictionary<ItemDefinition, ItemMagicScale>( this.PerItemDamageScale );
			return clone;
		}
	}
}
