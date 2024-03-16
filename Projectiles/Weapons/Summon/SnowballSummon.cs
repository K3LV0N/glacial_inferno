using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using glacial_inferno.Buffs.Summon;
using System;

namespace glacial_inferno.Projectiles.Weapons.Summon
{
    public class SnowballSummon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 23;
            Projectile.height = 16;
            Projectile.ignoreWater = true;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0.25f;
            Projectile.penetrate = -1;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Skip applying the buff effects for bosses or ice biome mobs
            if (target.boss || target.buffImmune[BuffID.Frostburn])
            {
                return;
            }
            target.AddBuff(ModContent.BuffType<FrozenBuff>(), 2 * 60);
            Projectile.Kill();
        }

        // The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!CheckActive(owner)) return;

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<SnowballSummonBuff>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<SnowballSummonBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 48f; //Three tiles above the player.

            //Have the minion follow behind the player
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
            idlePosition.X += minionPositionOffsetX;

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.04f;

            // Fix overlap with other minions
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];

                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            // Starting search distance
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
                if (between < 1200f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                //Find the target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            Projectile.friendly = foundTarget;
        }

        private const int JumpCooldown = 60;
        private int jumpCooldownTimer = 0;
        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            float jumpVelocity = 8f;
            float horizontalSpeed = 4f;

            bool onGround = Projectile.velocity.Y == 0 && Projectile.oldVelocity.Y == 0;

            if (onGround)
                Projectile.frame = 0;
            else
                Projectile.frame = 1;

            if (jumpCooldownTimer > 0)
            {
                jumpCooldownTimer--;
                if (onGround) Projectile.velocity.X = 0f;
            }

            //Jump towards the target
            if (foundTarget)
            {
                if (onGround && jumpCooldownTimer <= 0)
                {
                    Vector2 direction = targetCenter - Projectile.Center;
                    float distanceX = 0;
                    float distanceY = -Math.Abs(jumpVelocity);

                    if (Math.Abs(direction.X) > 40)
                    {
                        distanceX = direction.X > 0 ? horizontalSpeed : -horizontalSpeed;
                    }

                    Projectile.velocity.Y = distanceY;
                    Projectile.velocity.X = distanceX;

                    jumpCooldownTimer = JumpCooldown;
                }
            }
            else
            {
                // Idle behavior, jumping towards player if on the ground
                if (onGround && distanceToIdlePosition > 40f && jumpCooldownTimer <= 0)
                {
                    vectorToIdlePosition.Normalize();
                    Projectile.velocity.Y = -jumpVelocity;
                    Projectile.velocity.X = vectorToIdlePosition.X * horizontalSpeed;
                    jumpCooldownTimer = JumpCooldown;
                }
            }

            // Apply gravity to simulate the jump falling down
            if (!onGround || Projectile.velocity.Y < 0)
            {
                Projectile.velocity.Y += 0.4f;
            }
        }
    }
}
