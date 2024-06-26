﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using glacial_inferno.Projectiles.Weapons.Ranged;
using glacial_inferno.Buffs;
using glacial_inferno.Items.Ores;


namespace glacial_inferno.Items.Weapons.Ranged
{
    public class FireBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1.5f;
            Item.shootSpeed = 10f;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item5;

            Item.width = 10;
            Item.height = 30;

            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;

            //Change to custom projectile
            Item.shoot = AmmoID.Arrow;
            Item.useAmmo = AmmoID.Arrow;

        }

        //turns the arrows into fire arrows
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<FireArrow>();
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff<FireWeaponBuff>())
                damage.Base += 5f;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<FireIngot>(), 3);
            r.AddIngredient(ItemID.Rope, 3);
            r.AddTile(TileID.Anvils);
            r.Register();
        }
    }
}