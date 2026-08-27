using Terraria;

namespace CalamitySimpleWhipAddon.Content.Common
{
    /// <summary>
    /// セグメント構成を持つ NPC に関する判定ユーティリティ
    /// </summary>
    public static class WormLikeNPCUtils
    {
        /// <summary>
        /// 同一 realLife を持つ NPC の数を数える
        /// </summary>
        public static int CountSegments(int realLife)
        {
            if (realLife < 0)
                return 1;

            int count = 0;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC other = Main.npc[i];

                if (!other.active)
                    continue;

                if (other.realLife == realLife)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// 「ワーム級（多数セグメント）」かどうか
        /// </summary>
        public static bool IsLargeWormLike(NPC npc, int minSegments = 6)
        {
            if (npc == null || npc.realLife < 0)
                return false;

            return CountSegments(npc.realLife) >= minSegments;
        }
    }
}
