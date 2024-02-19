﻿using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.Items.Ranged;
using log4net.Core;
using Mono.Cecil;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class SeadragonHeldProj : BaseHeldGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Seadragon";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Seadragon>();
        public override int targetCWRItem => ModContent.ItemType<Seadragon>();
        public override float ControlForce => 0.06f;
        public override float GunPressure => 0.2f;
        public override float Recoil => 1.2f;
        public override void InOwner() {
            float armRotSengsFront = 60 * CWRUtils.atoR;
            float armRotSengsBack = 110 * CWRUtils.atoR;

            Projectile.Center = Owner.Center + new Vector2(DirSign * 15, 0);
            Projectile.rotation = DirSign > 0 ? MathHelper.ToRadians(20) : MathHelper.ToRadians(160);
            Projectile.timeLeft = 2;
            SetHeld();

            if (!Owner.mouseInterface) {
                if (Owner.PressKey()) {
                    Owner.direction = ToMouse.X > 0 ? 1 : -1;
                    Projectile.rotation = GunOnFireRot;
                    Projectile.Center = Owner.Center + Projectile.rotation.ToRotationVector2() * 20 + new Vector2(0, -3) + offsetPos;
                    armRotSengsBack = armRotSengsFront = (MathHelper.PiOver2 - Projectile.rotation) * DirSign;
                    if (HaveAmmo) {
                        onFire = true;
                        Projectile.ai[1]++;
                    }
                }
                else {
                    onFire = false;
                }
            }

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotSengsFront * -DirSign);
            Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armRotSengsBack * -DirSign);
        }

        public override void SpanProj() {
            if (onFire && Projectile.ai[1] > heldItem.useTime) {
                SoundEngine.PlaySound(heldItem.UseSound, Projectile.Center);
                Vector2 gundir = Projectile.rotation.ToRotationVector2();

                Projectile.NewProjectile(Owner.parent(), Projectile.Center + gundir * 3, ShootVelocity
                    , AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);

                int blast = Projectile.NewProjectile(Owner.parent(), Projectile.Center + gundir * 3, ShootVelocity
                    , ModContent.ProjectileType<ArcherfishShot>(), WeaponDamage * 5, WeaponKnockback, Owner.whoAmI, 0);

                Projectile.NewProjectile(Owner.parent(), Projectile.Center + gundir * 3
                    , ShootVelocity.RotatedByRandom(MathHelper.ToRadians(5f)) * Main.rand.NextFloat(1.45f, 1.65f)
                    , ModContent.ProjectileType<ArcherfishRing>()
                    , WeaponDamage, WeaponKnockback, Owner.whoAmI);

                _ = UpdateConsumeAmmo();
                _ = CreateRecoil();

                Projectile.ai[1] = 0;
                onFire = false;
            }
        }
    }
}