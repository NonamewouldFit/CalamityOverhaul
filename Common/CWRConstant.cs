﻿using Terraria.ModLoader;

namespace CalamityOverhaul.Common
{
    public static class CWRConstant
    {
        public const string Asset = "CalamityOverhaul/Assets/";
        public const string Effects = Asset + "Effects/";
        public const string Placeholder = Asset + "placeholder";
        public const string Placeholder2 = Asset + "placeholder2";
        public const string Masking = "CalamityOverhaul/Assets/Masking/";
        public const string Item = "CalamityOverhaul/Assets/Items/";
        public const string Item_Magic = "CalamityOverhaul/Assets/Items/Magic/";
        public const string Item_Melee = "CalamityOverhaul/Assets/Items/Melee/";
        public const string Item_Ranged = "CalamityOverhaul/Assets/Items/Ranged/";
        public const string Item_Summon = "CalamityOverhaul/Assets/Items/Summon/";
        public const string Projectile = "CalamityOverhaul/Assets/Projectiles/";
        public const string Projectile_Magic = "CalamityOverhaul/Assets/Projectiles/Magic/";
        public const string Projectile_Melee = "CalamityOverhaul/Assets/Projectiles/Melee/";
        public const string Projectile_Ranged = "CalamityOverhaul/Assets/Projectiles/Ranged/";
        public const string Projectile_Summon = "CalamityOverhaul/Assets/Projectiles/Summon/";
        public const string UI = Asset + "UIs/";
        public const string Buff = Asset + "Buffs/";
        public const string Dust = Asset + "Dusts/";
        public const string NPC = Asset + "NPCs/";
        public const string Sound = Asset + "Sounds/";

        public const string Cay_Item = "CalamityMod/Items/";
        public const string Cay_Wap = Cay_Item + "Weapons/";
        public const string Cay_Wap_Melee = Cay_Wap + "Melee/";
        public const string Cay_Wap_Ranged = Cay_Wap + "Ranged/";
        public const string Cay_Wap_Magic = Cay_Wap + "Magic/";
        public const string Cay_Wap_Summon = Cay_Wap + "Summon/";
        public const string Cay_Wap_Rogue = Cay_Wap + "Rogue/";
        public const string Cay_Proj = "CalamityMod/Projectiles/";
        public const string Cay_Proj_Melee = Cay_Proj + "Melee/";
        public const string Cay_Proj_Ranged = Cay_Proj + "Ranged/";
        public const string Cay_Proj_Magic = Cay_Proj + "Magic/";
        public const string Cay_Proj_Summon = Cay_Proj + "Summon/";
        public const string Cay_Proj_Rogue = Cay_Proj + "Rogue/";

        public const string noEffects = "Assets/Effects/";
        public const string noItem = "Assets/Items/";
        public const string noProjectile = "Assets/Projectiles/";

        public static bool ForceReplaceResetContent => ModContent.GetInstance<ContentConfig>().ForceReplaceResetContent;
        public static bool WeaponEnhancementSystem => ModContent.GetInstance<ContentConfig>().WeaponEnhancementSystem;
    }
}
