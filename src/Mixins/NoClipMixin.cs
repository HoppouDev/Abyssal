using Abyssal.Modules;
using Aether.Modding;
using Microsoft.Xna.Framework;

namespace Abyssal.Mixins;

[Mixin(typeof(Terraria.Player))]
public class NoClipMixin
{
    [Inject("DryCollision(System.Boolean,System.Boolean)", At.HEAD, Cancellable = true)]
    private static void BeforeDryCollision(Terraria.Player player, bool fallThrough, bool ignorePlats, CallbackInfo callback)
    {
        if (NoClip.TryMove(player))
            callback.Cancel();
    }

    [Inject("WetCollision(System.Boolean,System.Boolean,System.Single)", At.HEAD, Cancellable = true)]
    private static void BeforeWetCollision(Terraria.Player player, bool fallThrough, bool ignorePlats, float movementSpeed, CallbackInfo callback)
    {
        if (NoClip.TryMove(player))
            callback.Cancel();
    }

    // Slope collision would push the player out of the blocks they're flying through.
    [Inject("SlopingCollision(System.Boolean,System.Boolean)", At.HEAD, Cancellable = true)]
    private static void BeforeSlopingCollision(Terraria.Player player, bool fallThrough, bool ignorePlats, CallbackInfo callback)
    {
        if (NoClip.IsActiveFor(player))
            callback.Cancel();
    }

    // Update also steps the player onto ledges and along slopes before collision runs, which drags flight off course.
    [Inject("SlopeDownMovement()", At.HEAD, Cancellable = true)]
    private static void BeforeSlopeDownMovement(Terraria.Player player, CallbackInfo callback)
    {
        if (NoClip.IsActiveFor(player))
            callback.Cancel();
    }

    [Redirect("Update(System.Int32)", At.INVOKE,
        Target = "Terraria.Collision::StepDown(Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Vector2&,System.Int32,System.Int32,System.Single&,System.Single&,System.Int32,System.Boolean)")]
    private void StepDownUnlessNoClip(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir, bool waterWalk)
    {
        if (!NoClip.IsActiveFor((Terraria.Player)(object)this))
            Terraria.Collision.StepDown(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, gravDir, waterWalk);
    }

    [Redirect("Update(System.Int32)", At.INVOKE,
        Target = "Terraria.Collision::StepUp(Microsoft.Xna.Framework.Vector2&,Microsoft.Xna.Framework.Vector2&,System.Int32,System.Int32,System.Single&,System.Single&,System.Int32,System.Boolean,System.Int32)")]
    private void StepUpUnlessNoClip(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir, bool holdsMatching, int specialChecksMode)
    {
        if (!NoClip.IsActiveFor((Terraria.Player)(object)this))
            Terraria.Collision.StepUp(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, gravDir, holdsMatching, specialChecksMode);
    }
}
