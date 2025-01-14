﻿using CalamityMod.Items;
using CalamityMod;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityOverhaul.Content.Items.Melee;
using Microsoft.Xna.Framework;

namespace CalamityOverhaul.Content.RemakeItems.Melee
{
    internal class RBrimstoneSword : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Melee.BrimstoneSword>();
        public override int ProtogenesisID => ModContent.ItemType<BrimstoneSword>();
        public override void SetDefaults(Item item) {
            item.width = 32;
            item.height = 32;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.Rapier;
            item.damage = 90;
            item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            item.useAnimation = item.useTime = 10;
            item.shoot = ModContent.ProjectileType<BrimstoneSwordHeldProj>();
            item.shootSpeed = 2f;
            item.knockBack = 7.5f;
            item.UseSound = SoundID.Item1;
            item.value = CalamityGlobalItem.Rarity5BuyPrice;
            item.rare = ItemRarityID.Pink;
            item.EasySetLocalTextNameOverride("BrimstoneSword");
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            CWRUtils.OnModifyTooltips(CWRMod.Instance, tooltips, "BrimstoneSword");
        }

        public override bool? AltFunctionUse(Item item, Player player) {
            return false;
        }

        public override bool? CanUseItem(Item item, Player player) {
            return player.altFunctionUse != 2;
        }

        public override bool? UseItem(Item item, Player player) {
            item.DamageType = DamageClass.Melee;
            item.noMelee = false;
            return base.UseItem(item, player);
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            type = ModContent.ProjectileType<BrimstoneSwordHeldProj>();
        }

        public override void UseAnimation(Item item, Player player) {
            item.noUseGraphic = false;
            base.UseAnimation(item, player);
        }
    }
}
