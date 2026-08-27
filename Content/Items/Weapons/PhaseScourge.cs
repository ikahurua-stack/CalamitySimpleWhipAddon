using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Items;


namespace CalamitySimpleWhipAddon.Content.Items.Weapons
{
    public class GreenPhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<GreenPhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15 with { Volume = 20f };
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GreenPhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class BluePhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<BluePhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<BluePhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class OrangePhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<OrangePhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<OrangePhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class PurplePhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<PurplePhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<PurplePhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class RedPhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<RedPhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<RedPhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class WhitePhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<WhitePhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<WhitePhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

    public class YellowPhaseScourge : ModItem
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 45;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;

            Item.shoot = ModContent.ProjectileType<YellowPhaseScourgeProj>();
            Item.shootSpeed = 4f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<YellowPhaseWhip>()
                .AddIngredient(ItemID.CrystalShard, 40)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }

}