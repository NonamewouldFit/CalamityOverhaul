﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityOverhaul.Content.Particles.Core;
using CalamityOverhaul.Content.Particles;
using Microsoft.Xna.Framework.Graphics;
using System;
using CalamityOverhaul.Content.Items.Ranged;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee
{
    internal class EssenceStar : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/StarProj";

        public override void SetDefaults() {
            Projectile.height = 24;
            Projectile.width = 24;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.MaxUpdates = 3;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15 * Projectile.MaxUpdates;
        }

        public override void AI() {
            NPC target = Projectile.position.InPosClosestNPC(300);
            if (target != null) {
                Projectile.ChasingBehavior2(target.Center, 1, 0.05f);
            }
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < 6; i++) {
                    Vector2 vector = Projectile.velocity * 1.01f;
                    float slp = Main.rand.NextFloat(0.5f, 1.7f);
                    CWRParticleHandler.AddParticle(new HeavenStarParticle(Projectile.Center, vector, Color.White
                        , new Color(150, 100, 255, 255) * Projectile.Opacity, 0f, new Vector2(0.6f, 1f) * slp
                        , new Vector2(1, 1.7f) * slp, 10 + Main.rand.Next(6), 0f, 3f, 0f, Main.rand.Next(7) * 2, Main.rand.NextFloat(-0.3f, 0.3f)));
                }
            }
        }

        public void SpanEssStar(int maxNum, int minSp, int maxSp, float minSlp, float maxSlp) {
            if (Main.netMode != NetmodeID.Server) {
                for (int i = 0; i < maxNum; i++) {
                    Vector2 vector = Main.rand.NextVector2Unit() * Main.rand.Next(minSp, maxSp);
                    float slp = Main.rand.NextFloat(minSlp, maxSlp);
                    CWRParticleHandler.AddParticle(new HeavenStarParticle(Projectile.Center, vector, Color.White
                        , new Color(150, 100, 255, 255) * Projectile.Opacity, 0f, new Vector2(0.6f, 1f) * slp
                        , new Vector2(1.5f, 2.7f) * slp, 20 + Main.rand.Next(6), 0f, 3f, 0f, Main.rand.Next(7) * 2, Main.rand.NextFloat(-0.3f, 0.3f)));
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            Projectile.Explode();
            SpanEssStar(16, 3, 29, 0.2f, 0.7f);
            Projectile.velocity = -oldVelocity;
            Projectile.timeLeft -= 15;
            return false;
        }

        public override void OnKill(int timeLeft) {
            Projectile.damage *= 3;
            Projectile.Explode(150);
            SpanEssStar(22, 0, 79, 0.1f, 2.7f);
        }

        public override bool PreDraw(ref Color lightColor) {
            return false;
        }
    }
}
