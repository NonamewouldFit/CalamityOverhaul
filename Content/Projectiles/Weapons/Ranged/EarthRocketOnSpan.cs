﻿using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged
{
    internal class EarthRocketOnSpan : ModProjectile
    {
        public override string Texture => CWRConstant.Placeholder;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults() {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 40;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 1.2f;
        }

        public override void AI() {
            BaseOnSpanProj.FlowerAI(Projectile);
            if (Projectile.timeLeft % 5 == 0 && Owner.PressKey()) {
                Vector2 vr = Projectile.rotation.ToRotationVector2() * 7;
                DragonsBreathRifleHeldProj.SpawnGunDust(Projectile, Projectile.Center, vr);
                Projectile.NewProjectile(Owner.parent(), Projectile.Center, vr, ModContent.ProjectileType<ScorchedEarthRocket>()
                        , Owner.GetShootState().WeaponDamage, Owner.GetShootState().WeaponKnockback, Owner.whoAmI, 0);
                Vector2 pos = Projectile.Center - vr * 3 + vr.GetNormalVector() * 10 * Owner.direction;
                for (int i = 0; i < 100; i++) {
                    Vector2 dustVel = (Projectile.rotation + Main.rand.NextFloat(-0.1f, 0.1f)).ToRotationVector2() * -Main.rand.Next(26, 117);
                    float scale = Main.rand.NextFloat(0.5f, 1.5f);
                    Dust.NewDust(pos, 5, 5, DustID.CopperCoin, dustVel.X, dustVel.Y, 0, default, scale);
                }
            }
        }
    }
}
