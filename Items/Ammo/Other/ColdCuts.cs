﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using glacial_inferno.Projectiles.Ammo.Other;

namespace glacial_inferno.Items.Ammo.Other
{
    public class ColdCuts : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.consumable = true;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = Item.type;
            Item.maxStack = Item.CommonMaxStack;
            Item.shoot = ModContent.ProjectileType<ColdCutProjectile>();
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}