using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System.IO;
using Microsoft.Xna.Framework;
using CalamitySimpleWhipAddon.Content.Systems;
using CalamitySimpleWhipAddon.Content.Buffs;

namespace CalamitySimpleWhipAddon.Content.Projectiles
{
    public class WulfrumLightning_Damage : ModProjectile
    {
        public int buffType;

        public override void SetDefaults()
        {
            Projectile.netImportant = true;

            Projectile.width = 4;
            Projectile.height = 4;

            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 1;

            Projectile.penetrate = 1;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.hide = true;


            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.ArmorPenetration = 9999;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)Projectile.ai[0]];

            if (!target.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = target.Center;

            if (target.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx19>()))
            {
                buffType = ModContent.BuffType<SimpleWhipDebuffEx19>();
            }
            else if (target.HasBuff(ModContent.BuffType<SimpleWhipDebuffEx20>()))
            {
                buffType = ModContent.BuffType<SimpleWhipDebuffEx20>();
            }


            int index = target.FindBuffIndex(buffType);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket packet = Mod.GetPacket();

                packet.Write((byte)SimpleWhipPacketID.RemoveDebuff); // 独自の識別子
                packet.Write(target.whoAmI);
                packet.Write(buffType);
                packet.Send();

            }

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                if (index != -1)
                {
                    target.DelBuff(index);
                }
            }
        }
    }
}
