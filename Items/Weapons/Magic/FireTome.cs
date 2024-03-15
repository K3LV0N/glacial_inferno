using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using glacial_inferno.Projectiles.Weapons.Magic;
using System.Numerics;
using glacial_inferno.Projectiles.Weapons.Ranged;
using glacial_inferno.Buffs;

namespace glacial_inferno.Items.Weapons.Magic
{
    public class FireTome : ModItem
    {
        /*public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
        }*/
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.shootSpeed = 10f;
            Item.autoReuse = false;
            Item.maxStack = 1;
            Item.noMelee = true;

            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 18;

            Item.width = 27;
            Item.height = 30;

            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.shoot = ModContent.ProjectileType<FireBall>();
            Item.mana = 10;

            Item.staff[Item.type] = true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff<FireWeaponBuff>())
                damage.Base += 5f;
        }

        //TODO: Figure Recipes Out
        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            //TODO: add Recipe items
            //r.AddIngredient()
            //TODO: add workbench requirement (might be a modded workbench I got no clue)
            //r.AddTile()
            //TODO: register recipe
            //r.Register();
        }

        
    }
}
