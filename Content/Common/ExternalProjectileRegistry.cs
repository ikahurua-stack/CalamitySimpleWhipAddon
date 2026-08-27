using System.Collections.Generic;
using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.Common
{
    public class ExternalProjectileRegistry : ModSystem
    {
        private static readonly HashSet<int> RegisteredProjectiles = new();

        public static IEnumerable<int> GetRegisteredTypes() => RegisteredProjectiles;

        public override void PostSetupContent()
        {
            RegisterThoriumProjectiles();
            RegisterCalamityOverhaulProjectiles();
            RegisterAAModProjectiles();
            RegisterConsolariaProjectiles();
            RegisterFargowiltasSoulsProjectiles();
            RegisterSOTSProjectiles();
            RegisterSpiritProjectiles();
            RegisterSpiritReforgedProjectiles();
            RegisterCatalystProjectiles();
            RegisterFablesProjectiles();
            RegisterGensokyoProjectiles();
            RegisterStarsAboveProjectiles();
            RegisterEntropyProjectiles();
            RegisterJourneyProjectiles();
            RegisterSplitProjectiles();
            RegisterClamityProjectiles();
        }

        public static bool Contains(int projectileType)
        {
            return RegisteredProjectiles.Contains(projectileType);
        }

        private static void RegisterThoriumProjectiles()
        {
            if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                return;

            TryAdd(thorium, "LivingWoodAcornPro");
            TryAdd(thorium, "LivingWoodAcornShotPro");
            TryAdd(thorium, "LivingWoodAcornShotPro2");
            TryAdd(thorium, "LivingWoodAcornShotPro3");
            TryAdd(thorium, "LivingWoodAcornShotPro4");
            TryAdd(thorium, "SeahorseWandPro2");
            TryAdd(thorium, "Spark");
            TryAdd(thorium, "MantisPro2");
            TryAdd(thorium, "YarnBallProShoot");
            TryAdd(thorium, "IceFairyStaffProShoot");
            TryAdd(thorium, "DraconicMagmaStaffProShot");
            TryAdd(thorium, "LadyLightProShoot");
            TryAdd(thorium, "MastersLibramPro1Shoot");
            TryAdd(thorium, "MastersLibramPro2Shoot");
            TryAdd(thorium, "MastersLibramPro3Shoot");
            TryAdd(thorium, "MastersLibramPro4Shoot");
            TryAdd(thorium, "MastersLibramPro5Shoot");
            TryAdd(thorium, "MastersLibramPro6Shoot");
            TryAdd(thorium, "SnowmanBombBoom");
            TryAdd(thorium, "BeholderStaffProShoot");
            TryAdd(thorium, "SteamgunnerControllerProShoot");
            TryAdd(thorium, "EyeofOdinPro2");
            TryAdd(thorium, "TheBlackCaneBowProShoot");
            TryAdd(thorium, "BloodyPaganStaffPro2");
            TryAdd(thorium, "BloodyPaganStaffPro3");
            TryAdd(thorium, "EnigmaBeam");
            TryAdd(thorium, "NebulaReflectionProShoot1");
            TryAdd(thorium, "NebulaReflectionProShoot2");
            TryAdd(thorium, "EmberFlare");
            TryAdd(thorium, "EmberStaffProShoot");
        }

        private static void RegisterCalamityOverhaulProjectiles()
        {
            if (!ModLoader.TryGetMod("CalamityOverhaul", out Mod overhaul))
                return;

            TryAdd(overhaul, "DestroyerHead");
            TryAdd(overhaul, "DestroyerBody");
            TryAdd(overhaul, "DestroyerTail");
        }

        private static void RegisterAAModProjectiles()
        {
            if (!ModLoader.TryGetMod("AAMod", out Mod aamod))
                return;

            TryAdd(aamod, "Criminal_Proj");
            TryAdd(aamod, "MireHelper_Proj");
            TryAdd(aamod, "InfernoHelper_Proj");
            TryAdd(aamod, "TwinkleStar_Proj");
            TryAdd(aamod, "SporadicSporovid_Proj");
        }

        private static void RegisterConsolariaProjectiles()
        {
            if (!ModLoader.TryGetMod("Consolaria", out Mod consolaria))
                return;

            TryAdd(consolaria, "EternalLaser");
            TryAdd(consolaria, "EternalScythe");
        }

        private static void RegisterFargowiltasSoulsProjectiles()
        {
            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargo))
                return;

            TryAdd(fargo, "BrainMinion");
            TryAdd(fargo, "CreeperMinion");
            TryAdd(fargo, "JungleMimicSummonCoin");
            TryAdd(fargo, "BigBrainIllusion");
            TryAdd(fargo, "OpticFlamethrower");
            TryAdd(fargo, "OpticElectricOrb");
            TryAdd(fargo, "CirnoIceChunk");
            TryAdd(fargo, "CirnoIceCube");
            TryAdd(fargo, "MystiaNote2");
            TryAdd(fargo, "DaiyoLeaf");
            TryAdd(fargo, "RazorbladeTyphoonFriendly2");
            TryAdd(fargo, "PrimeMinionProj");

        }

        private static void RegisterSOTSProjectiles()
        {
            if (!ModLoader.TryGetMod("SOTS", out Mod sots))
                return;

            TryAdd(sots, "FreshGreenyCounter");
            TryAdd(sots, "FreshGreeny");
            TryAdd(sots, "CrystalSerpentBody");
            TryAdd(sots, "CrystalSerpentHead");
            TryAdd(sots, "StarBolt");
            TryAdd(sots, "RippleWaveSummon");
            TryAdd(sots, "PurpleHomingBolt");
            TryAdd(sots, "SpectralWispLaser");
            TryAdd(sots, "PenguinMissile");
            TryAdd(sots, "SharangaBlastSummon");
            TryAdd(sots, "TerminatorAcorn");
            TryAdd(sots, "CursedStab");
            TryAdd(sots, "FrostSpear");
            TryAdd(sots, "OtherworldLightning");
            TryAdd(sots, "ThunderRing");
            TryAdd(sots, "NatureBeam");
            TryAdd(sots, "InfernoLaser");
            TryAdd(sots, "EvilSpear");
            TryAdd(sots, "EvilExplosion");
            TryAdd(sots, "ChaosBeam");

        }

        private static void RegisterSpiritProjectiles()
        {
            if (!ModLoader.TryGetMod("SpiritMod", out Mod spirit))
                return;

            TryAdd(spirit, "LunazoaOrbiter");
            TryAdd(spirit, "JellyfishOrbiter_Friendly");
            TryAdd(spirit, "LocustSmall");
            TryAdd(spirit, "JellyfishBolt");
            TryAdd(spirit, "AquaBall");
            TryAdd(spirit, "OrangeBeam");
            TryAdd(spirit, "Blaze");
            TryAdd(spirit, "FairyProj");
            TryAdd(spirit, "PigronBubble");
            TryAdd(spirit, "ToucanFeather");
            TryAdd(spirit, "PoisonCloud");

        }

        private static void RegisterSpiritReforgedProjectiles()
        {
            if (!ModLoader.TryGetMod("SpiritReforged", out Mod spiritreforged))
                return;

            TryAdd(spiritreforged, "FairyProj");
            TryAdd(spiritreforged, "ToucanFeather");
            TryAdd(spiritreforged, "JellyfishBolt");

        }

        private static void RegisterFablesProjectiles()
        {
            if (!ModLoader.TryGetMod("CalamityFables", out Mod fables))
                return;

            TryAdd(fables, "WulfrumEnergyBurst");
            TryAdd(fables, "InkyCapSporeBomb");

        }

        private static void RegisterCatalystProjectiles()
        {
            if (!ModLoader.TryGetMod("CatalystMod", out Mod catalyst))
                return;

            TryAdd(catalyst, "AstralpodBullet");
            TryAdd(catalyst, "AstralpodLaser");

        }

        private static void RegisterGensokyoProjectiles()
        {
            if (!ModLoader.TryGetMod("Gensokyo", out Mod gensokyo))
                return;

            TryAdd(gensokyo, "DollStaff_CasterDollBullet");
            TryAdd(gensokyo, "UfoStaff_Fireball");
            TryAdd(gensokyo, "UfoStaff_GreenBeam");
            TryAdd(gensokyo, "UfoStaff_BlueLaser");
            TryAdd(gensokyo, "WillowStaff_SekibankiEyeLaser");
            TryAdd(gensokyo, "NamelessDoll_GasCloud");
            TryAdd(gensokyo, "VerminStaff_FoulGlob");
            TryAdd(gensokyo, "MaskStaff_JoyLaser");

        }

        private static void RegisterStarsAboveProjectiles()
        {
            if (!ModLoader.TryGetMod("StarsAbove", out Mod starsabove))
                return;

            TryAdd(starsabove, "SugartleBubble");
            TryAdd(starsabove, "BulbasugarSeeds");
            TryAdd(starsabove, "TimePulse");
            TryAdd(starsabove, "TimePulseExplosion");
            TryAdd(starsabove, "TakodachiRound");
            TryAdd(starsabove, "ChaosMagic");
            TryAdd(starsabove, "ChaosObject");
            TryAdd(starsabove, "FleetingSparkBullet");
            TryAdd(starsabove, "SparkExplosion");
            TryAdd(starsabove, "FleetingSparkMinion");
            TryAdd(starsabove, "WavedancerSummon");
            TryAdd(starsabove, "SatanaelRound");
            TryAdd(starsabove, "PodShot");

        }

        private static void RegisterEntropyProjectiles()
        {
            if (!ModLoader.TryGetMod("CalamityEntropy", out Mod entropy))
                return;

            TryAdd(entropy, "NxCrack");
            TryAdd(entropy, "LuminarisMinionAstralShoot");
            TryAdd(entropy, "Brimstone");
            TryAdd(entropy, "WohLaser");
            TryAdd(entropy, "SpiritLightSoul");
            TryAdd(entropy, "AzafureDroneBullet");
            TryAdd(entropy, "CommonExplotionFriendly");
            TryAdd(entropy, "CursingFlame");
            TryAdd(entropy, "CombatDroneBullet");
            TryAdd(entropy, "MinionCinderFireball");

        }

        private static void RegisterJourneyProjectiles()
        {
            if (!ModLoader.TryGetMod("ContinentOfJourney", out Mod journey))
                return;

            TryAdd(journey, "CraniumBolt");
            TryAdd(journey, "ClottedLaser");
            TryAdd(journey, "ClottedHunger");
            TryAdd(journey, "MinionshotSentry_1");
            TryAdd(journey, "WhiteDwarfBullet");
            TryAdd(journey, "DeathButterflyProj");
            TryAdd(journey, "SpiritMinionshot");
            TryAdd(journey, "SilverPortalShot");
            TryAdd(journey, "GoldenPortalShot");
            TryAdd(journey, "OrbitalLaser");
            TryAdd(journey, "OrganicPetal_1");
            TryAdd(journey, "Phantom_1_Shoot_1");
            TryAdd(journey, "Phantom_1_Shoot_2");
            TryAdd(journey, "Phantom_3_Shoot_1");
            TryAdd(journey, "Phantom_3");

        }

        private static void RegisterSplitProjectiles()
        {
            if (!ModLoader.TryGetMod("Split", out Mod split))
                return;

            TryAdd(split, "CirnoBolt");
            TryAdd(split, "UtsuhoSunbolt");
            TryAdd(split, "UtsuhoSunboltExplosion");
            TryAdd(split, "AeyhBolt");
            TryAdd(split, "JellyBubble");
            TryAdd(split, "FlandreWave");

        }

        private static void RegisterClamityProjectiles()
        {
            if (!ModLoader.TryGetMod("Clamity", out Mod clamity))
                return;

            TryAdd(clamity, "HellsBellsRing");
        }

        private static void TryAdd(Mod mod, string projectileName)
        {
            if (mod.TryFind(projectileName, out ModProjectile projectile))
                RegisteredProjectiles.Add(projectile.Type);
        }

    }
}
