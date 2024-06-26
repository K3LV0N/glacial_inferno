using glacial_inferno.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//WIP
//Back Attack Bonus
//Make exploding attack at full charge

namespace glacial_inferno.Items.Weapons.Melee
{
    public class MagicalClub : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 100;
            Item.useAnimation = 15;
            Item.knockBack = 6;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IceBlock, 10);
            recipe.AddIngredient(ItemID.Star, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        int counter = 0;
        int counter_max = 600;
        //does slow debuff on hit
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(counter >= counter_max)
            {
                target.AddBuff(BuffID.Slow, 200);
            }
            counter /= 2;
            base.OnHitNPC(player, target, hit, damageDone);
        }

        /*
        public override bool AltFunctionUse(Player player)
        {
            player.AddBuff(BuffID.OnFire, 200);
            return base.AltFunctionUse(player);
        }
        */

        //checks to see if you are on fire and are holding the weapon
        //if this happens, weapon disintegrates and you get a small buff
        public override void HoldItem(Player player)
        {
            if (player.onFire == true || player.onFire2 == true || player.onFire3 == true)
            {
                player.onFire = false;
                player.onFire2 = false;
                player.onFire3 = false;

                player.AddBuff(BuffID.ObsidianSkin, 600);
                player.AddBuff(ModContent.BuffType<MeltedIce>(), 600);
                counter = 0;
            }

            base.HoldItem(player);
        }


        //damage increases over time to a set amount
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HeldItem.Name == Item.Name && player.HasBuff(ModContent.BuffType<MeltedIce>()) == false)
            {
                if (counter <= counter_max)
                {
                    damage.Base += (counter / 60);
                    counter += 1;
                }
                else
                {
                    damage.Base = counter / 60;
                }


            }
            else
            {
                counter -= 1;
            }
            base.ModifyWeaponDamage(player, ref damage);
        }

        //checks to see if you have the MeltedIce debuff, if you do, you cannot use the weapon.
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<MeltedIce>()) == true)
            {
                return false;
            }
            return base.CanUseItem(player);
        }

    }
}