using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Abyssal.Modules;

/// <summary>
/// Replaces the local player's tile collision with free flight driven by the movement keys. Shift doubles the speed.
/// </summary>
[ModuleInfo("noclip", "Noclip", "Fly through blocks with the movement keys. Hold Shift to go faster.", ModuleCategory.Movement)]
public sealed class NoClip : Module
{
    private const float Speed = 8f;

    public static NoClip Instance { get; } = new();

    public static bool IsActiveFor(Player player) => Instance.Enabled && player.whoAmI == Main.myPlayer;

    /// <summary>
    /// Moves the player without collision. Returns false when noclip doesn't apply, so the game's collision runs.
    /// </summary>
    public static bool TryMove(Player player)
    {
        if (!IsActiveFor(player))
            return false;

        var direction = Vector2.Zero;
        if (player.controlLeft)
            direction.X -= 1;
        if (player.controlRight)
            direction.X += 1;
        if (player.controlUp || player.controlJump)
            direction.Y -= 1;
        if (player.controlDown)
            direction.Y += 1;

        bool fast = Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);
        float speed = fast ? Speed * 2 : Speed;

        player.velocity = direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(direction) * speed;
        player.position += player.velocity;

        // Measure falls from here so turning noclip off mid-air doesn't count the flight.
        player.fallStart = player.fallStart2 = (int)(player.position.Y / 16f);
        return true;
    }
}
