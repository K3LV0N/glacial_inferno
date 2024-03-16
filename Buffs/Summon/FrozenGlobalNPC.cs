using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace glacial_inferno.Buffs.Summon
{
    public class FrozenGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int originalAIStyle = -1;
        private int originalDamage = -1;
        public bool isFrozen = false;

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<FrozenBuff>()))
            {
                isFrozen = false;
                if (originalAIStyle >= 0)
                {
                    npc.aiStyle = originalAIStyle;
                    npc.damage = originalDamage;
                    originalAIStyle = -1;
                    originalDamage = -1;
                }
            }
        }

        public override void AI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<FrozenBuff>()))
            {
                if (!isFrozen)
                {
                    originalAIStyle = npc.aiStyle;
                    originalDamage = npc.damage;
                    isFrozen = true;
                }
                npc.aiStyle = 0;
                npc.damage = 0;
            }
        }
    }

}
