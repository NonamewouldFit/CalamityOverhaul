﻿using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Magic.HeldProjs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Magic
{
    /// <summary>
    /// 命运神启
    /// </summary>
    internal class FatesReveal : EctypeItem //或许让其变成主动型的鬼火生成器是个不错的想法
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";

        public override string Texture => CWRConstant.Cay_Wap_Magic + "FatesReveal";

        public override void SetStaticDefaults() {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults() {
            Item.damage = 56;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 80;
            Item.height = 86;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FatesRevealHeldProj>();
            Item.shootSpeed = 1f;
            Item.value = CalamityGlobalItem.Rarity13BuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Magic/FatesRevealGlow").Value);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.ownedProjectileCounts[Item.shoot] <= 0)
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
