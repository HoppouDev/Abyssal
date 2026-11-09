using Aether.Api;

namespace Abyssal.Modules;

/// <summary>
/// Keeps the local player's god mode field set. Only the field is set, so the Journey power state is untouched.
/// </summary>
public sealed class GodMode : Module
{
    public static GodMode Instance { get; } = new();

    public override string Name => "God Mode";

    protected override void OnEnable() => Apply(true);

    protected override void OnDisable() => Apply(false);

    private static void Apply(bool enabled)
    {
        if (Players.TryGetLocal(out var player))
            player.creativeGodMode = enabled;
    }
}
