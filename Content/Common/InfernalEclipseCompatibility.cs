using Terraria.ModLoader;

namespace CalamitySimpleWhipAddon
{
    /// <summary>
    /// Identifies whether the Infernal Eclipse compatibility balance should be used.
    /// </summary>
    internal static class InfernalEclipseCompatibility
    {
        internal const string ModName = "InfernalEclipseAPI";

        internal static bool IsEnabled => ModContent.GetInstance<Config>().HummusBalancing || ModLoader.HasMod(ModName);
    }
}
