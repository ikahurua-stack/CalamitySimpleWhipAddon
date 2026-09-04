using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CalamitySimpleWhipAddon
{
    /// <summary>
    /// Server-synchronized gameplay settings for Calamity Simple Whip Addon.
    /// </summary>
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        [Label("Enable Hummus balancing")]
        [Tooltip("Uses Infernal Eclipse compatibility balance even when Infernal Eclipse is not installed.")]
        public bool HummusBalancing;
    }
}
