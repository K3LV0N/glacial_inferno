using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace glacial_inferno.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class FireBreastplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 60;
        }

        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
            //Might do this later
            //player.buffImmunep[BuffID.Frozen] = true;
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
