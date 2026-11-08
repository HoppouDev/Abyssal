using Aether.Modding;
using Aether.Api;

namespace Abyssal;

public sealed class Startup : IModInitializer
{
    public const string ModId = "abyssal";

    public void Initialize()
    {
        Keybinds.Register("abyssal:god-mode", "OemTilde", () =>
        {
            if (!Players.TryGetLocal(out var player))
            {
                return;
            }

            GodState.Enabled = !GodState.Enabled;
            player.creativeGodMode = GodState.Enabled;
            Terraria.Main.NewText($"[abyssal] God mode {(GodState.Enabled ? "enabled" : "disabled")}", new Microsoft.Xna.Framework.Color(255, 215, 75));
        });

        // HudOverlays.Register("abyssal:god-mode", canvas =>
        // {
        //     if (GodState.Enabled)
        //         canvas.DrawText("GOD MODE", 16, 80, HudColor.White);
        // });
    }
}
