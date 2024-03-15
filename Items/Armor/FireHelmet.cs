using glacial_inferno.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace glacial_inferno.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class FireHelmet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 90, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 30;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FireBreastplate>() && legs.type == ModContent.ItemType<FireLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.AddBuff(ModContent.BuffType<FireWeaponBuff>(), 2);
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
