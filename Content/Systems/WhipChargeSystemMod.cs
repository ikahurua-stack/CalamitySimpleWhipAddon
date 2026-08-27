using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CalamitySimpleWhipAddon.Content.Systems;

public class WhipChargeSystemMod : ModSystem
{
    public override void PostUpdatePlayers()
    {
        WhipChargeSystem.Update();
    }
}