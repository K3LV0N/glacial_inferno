using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using glacial_inferno.Projectiles.Weapons.Summon;
using glacial_inferno.Buffs.Summon;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace glacial_inferno.Items.Weapons.Summon
{
    public class IceSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.knockBack = 3f;
            Item.mana = 5;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.notAmmo = true;
            Item.maxStack = 1;

            Item.width = 24;
            Item.height = 49;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<SnowballSummonBuff>();
            Item.shoot = ModContent.ProjectileType<SnowballSummon>();

            Item.value = Item.buyPrice(gold: 30);
            Item.value = Item.sellPrice(gold: 25);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            // Add crafting recipes for the item if desired
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BorealWood, 5);
            recipe.AddIngredient(ItemID.Gel, 5);
            recipe.AddIngredient(ItemID.IceBlock, 5);
            recipe.AddIngredient(ItemID.SnowBlock, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Center;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }
    }
}
