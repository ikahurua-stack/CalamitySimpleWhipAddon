using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using CalamitySimpleWhipAddon.Content.Buffs;
using CalamitySimpleWhipAddon.Content.Common;
using CalamitySimpleWhipAddon.Content.Systems;

namespace CalamitySimpleWhipAddon
{
    public class CalamitySimpleWhipAddon : Mod
    {

        public static Effect GateEffect;
        public static Effect DiagonalCutEffect;
        public static Effect ShotTrailEffect;


        public override void Load()
        {
            if (!Main.dedServ)
            {
                GateEffect = ModContent.Request<Effect>(
                    "CalamitySimpleWhipAddon/Content/Effects/GateShader",
                    ReLogic.Content.AssetRequestMode.ImmediateLoad
                ).Value;

                DiagonalCutEffect = ModContent.Request<Effect>(
                    "CalamitySimpleWhipAddon/Content/Effects/DiagonalCut",
                    ReLogic.Content.AssetRequestMode.ImmediateLoad
                ).Value;

                ShotTrailEffect = ModContent.Request<Effect>(
                    "CalamitySimpleWhipAddon/Content/Effects/TrailShader",
                    ReLogic.Content.AssetRequestMode.ImmediateLoad
                ).Value;


            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                GateEffect = null;
                DiagonalCutEffect = null;
                ShotTrailEffect = null;
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            SimpleWhipPacketID type = (SimpleWhipPacketID)reader.ReadByte();


            switch (type)
            {
                case SimpleWhipPacketID.RemoveDebuff:
                    {
                        int npcIndex = reader.ReadInt32();
                        int buffToRemove = reader.ReadInt32();

                        if (npcIndex >= 0 && npcIndex < Main.maxNPCs)
                        {
                            NPC target = Main.npc[npcIndex];
                            if (target.active)
                            {
                                for (int i = 0; i < NPC.maxBuffs; i++)
                                {
                                    if (target.buffType[i] == buffToRemove)
                                    {
                                        target.DelBuff(i);
                                        break;
                                    }
                                }
                            }
                        }

                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket forward = GetPacket();
                            forward.Write((byte)SimpleWhipPacketID.RemoveDebuff);
                            forward.Write(npcIndex);
                            forward.Write(buffToRemove);
                            forward.Send(-1, whoAmI);
                        }
                        break;
                    }

                case SimpleWhipPacketID.LightningSpawn:
                    {
                        int projID = reader.ReadInt32();

                        // ReadVector2() で一括読み込み
                        Vector2 start = reader.ReadVector2();
                        Vector2 end = reader.ReadVector2();

                        int index = reader.ReadInt32();

                        RubellusGemSystem.AddLightning(start, end, index);

                        // サーバーの場合は他の全クライアントに転送
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket forward = GetPacket();
                            forward.Write((byte)SimpleWhipPacketID.LightningSpawn);
                            forward.Write(projID);

                            // 転送時も型を統一
                            forward.WriteVector2(start);
                            forward.WriteVector2(end);

                            forward.Write(index);

                            // 送信元(whoAmI / fromWho)以外の全員に送る
                            // ※HandlePacket(BinaryReader reader, int whoAmI) の whoAmI を指定してください
                            forward.Send(-1, -1);
                        }
                        break;
                    }

                case SimpleWhipPacketID.LightningSystem1:
                    {
                        int projID = reader.ReadInt32();

                        // ReadVector2() で一括読み込み
                        Vector2 start = reader.ReadVector2();
                        Vector2 end = reader.ReadVector2();

                        int index = reader.ReadInt32();

                        LightningSystem.AddLightning(start, end);

                        // サーバーの場合は他の全クライアントに転送
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket forward = GetPacket();
                            forward.Write((byte)SimpleWhipPacketID.LightningSystem1);
                            forward.Write(projID);

                            // 転送時も型を統一
                            forward.WriteVector2(start);
                            forward.WriteVector2(end);

                            // 送信元(whoAmI / fromWho)以外の全員に送る
                            // ※HandlePacket(BinaryReader reader, int whoAmI) の whoAmI を指定してください
                            forward.Send(-1, -1);
                        }
                        break;
                    }

                case SimpleWhipPacketID.WulfrumLightningSystem1:
                    {
                        Vector2 start = reader.ReadVector2();
                        Vector2 end = reader.ReadVector2();

                        WulfrumLightningSystem.AddLightning(start, end);

                        // サーバーの場合は他の全クライアントに転送
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket forward = GetPacket();
                            forward.Write((byte)SimpleWhipPacketID.WulfrumLightningSystem1);

                            // 転送時も型を統一
                            forward.WriteVector2(start);
                            forward.WriteVector2(end);

                            // 送信元(whoAmI / fromWho)以外の全員に送る
                            // ※HandlePacket(BinaryReader reader, int whoAmI) の whoAmI を指定してください
                            forward.Send(-1, -1);
                        }
                        break;
                    }

                case SimpleWhipPacketID.MechanicalLightningSystem1:
                    {
                        Vector2 start = reader.ReadVector2();
                        Vector2 end = reader.ReadVector2();

                        MechanicalLightningSystem.AddLightning(start, end);

                        // サーバーの場合は他の全クライアントに転送
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket forward = GetPacket();
                            forward.Write((byte)SimpleWhipPacketID.MechanicalLightningSystem1);

                            // 転送時も型を統一
                            forward.WriteVector2(start);
                            forward.WriteVector2(end);

                            // 送信元(whoAmI / fromWho)以外の全員に送る
                            // ※HandlePacket(BinaryReader reader, int whoAmI) の whoAmI を指定してください
                            forward.Send(-1, -1);
                        }
                        break;
                    }

            }
        }

    }

    public enum SimpleWhipPacketID : byte
    {
        RemoveDebuff,
        LightningSpawn,
        LightningSystem1,
        WulfrumLightningSystem1,
        MechanicalLightningSystem1
    }
}
