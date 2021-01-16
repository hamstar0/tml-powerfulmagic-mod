using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace PowerfulMagic.Items {
	class ManaPotionConcentrateItem : ModItem {
        public override void SetStaticDefaults() {
            this.Tooltip.SetDefault( "Key active ingredient for Greater Mana Potions." );
        }

        public override void SetDefaults() {
            this.item.width = 20;
            this.item.height = 20;
            this.item.maxStack = 99;
            this.item.material = true;
            this.item.rare = ItemRarityID.Orange;
            this.item.value = Item.buyPrice( gold: 1 );
        }


        ////////////////

        public override void AddRecipes() {
            var recipe = new ModRecipe( this.mod );
            recipe.AddTile( TileID.Bottles );
            recipe.AddIngredient( this, 2 );
            recipe.AddIngredient( ItemID.FallenStar, 1 );
            recipe.AddIngredient( ItemID.ManaPotion, 2 );
            recipe.SetResult( ItemID.GreaterManaPotion, 2 );
            recipe.AddRecipe();
        }
    }
}
