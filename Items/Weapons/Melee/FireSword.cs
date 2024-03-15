using glacial_inferno.Buffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace glacial_inferno.Items.Weapons.Melee
{
    //Fire sword that has 3 charges (subject to change) that recharge over time
    //When hitting an enemy with a charged attack set them on fire, if they are already on fire deal bonus damage
    public class FireSword : ModItem
    {
        internal static int maxCharge = 3;
        internal int charge = maxCharge;
        internal int timer = 0;
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.knockBack = 6;
            Item.DamageType = DamageClass.Melee;

            Item.width = 27;
            Item.height = 32;

            Item.useTime = 30;
            Item.useAnimation = Item.useTime;
            Item.UseSound = SoundID.Item1;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 0, 80, 10);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
        }

        

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //Sets the npc on fire if sword has a charge and the npc isn't on fire yet
            if(charge > 0 && !target.HasBuff(BuffID.OnFire))
            {
                target.AddBuff(BuffID.OnFire, 180);
                charge--;
            }
        }


        public override void HoldItem(Player player) 
        {
            timer++;
            //gain a charge every 5 seconds (subject to change)
            if(timer == 300 && charge < maxCharge) {
                charge++;
                timer = 0; //reset timer to stop it from going to infinity
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff<FireWeaponBuff>())
                damage.Base += 5f;
        }

        //Increase the damage by 2 if the enemy is already on fire and the weapon has a charge
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (charge > 0 && target.HasBuff(BuffID.OnFire))
            {
                modifiers.FlatBonusDamage += 2;
                charge--;
            }
        }

        //TODO: Figure Recipes Out
        public override void AddRecipes()
        {
            //Recipe r = CreateRecipe();
            //TODO: add Recipe items
            //r.AddIngredient()
            //TODO: add workbench requirement (might be a modded workbench I got no clue)
            //r.AddTile()
            //TODO: register recipe
            //r.Register();
        }

    }
}