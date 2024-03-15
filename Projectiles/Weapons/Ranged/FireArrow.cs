using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//An arrow shot by the glacial bow, not obtainable any other way
namespace glacial_inferno.Projectiles.Weapons.Ranged
{
    public class FireArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = ProjectileID.WoodenArrowFriendly;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }


        //Adds a dust trail and make the projectile split
        public override void AI()
        {
            //timer for projectile split
            Projectile.ai[1] += 1f;

            if (Main.rand.NextBool(1)) //Adds dust particles
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.InfernoFork, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }

            //Split the projectile into 3 and delete the original projectile
            if (Projectile.ai[1] == 45f && Projectile.owner == Main.myPlayer && Projectile.ai[2] == 0f)
            {
                //3 projectiles one keeps the original direction of the projectile and the other 2 go up and down

                //Original Direction
                float speedX = Projectile.velocity.X;
                float speedY = Projectile.velocity.Y;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, speedX, speedY, ModContent.ProjectileType<FireArrow>(), (int)(Projectile.damage * 0.75), 0f, Projectile.owner, 0f, 0f, 1f);

                //up
                Vector2 rotateUpSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(15));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, rotateUpSpeed.X, rotateUpSpeed.Y, ModContent.ProjectileType<FireArrow>(), (int)(Projectile.damage * 0.75), 0f, Projectile.owner, 0f, 0f, 1f);

                //down
                Vector2 rotateDownSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-15));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, rotateDownSpeed.X, rotateDownSpeed.Y, ModContent.ProjectileType<FireArrow>(), (int)(Projectile.damage * 0.75), 0f, Projectile.owner, 0f, 0f, 1f);

                Projectile.Kill();
            }
        }
    }
}
