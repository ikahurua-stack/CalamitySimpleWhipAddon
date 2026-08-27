using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon.Content.DamageClasses
{
    /// <summary>
    /// 召喚タグ・鞭効果などの「効果」だけを継承し、
    /// 召喚ダメージ倍率・装備補正は一切受けない疑似召喚 DamageClass。
    /// </summary>
    public class PseudoSummonDamageClass : DamageClass
    {
        public override bool UseStandardCritCalcs => false;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            // ダメージ・クリ率・ノックバック等の数値補正は一切継承しない
            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            // 鞭タグ・Minion 関連の「効果」だけは Summon から継承
            return damageClass == DamageClass.Summon;
        }
    }
}