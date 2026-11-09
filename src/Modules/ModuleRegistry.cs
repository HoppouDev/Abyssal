namespace Abyssal.Modules;

/// <summary>
/// Modules in the order they appear in the menu.
/// </summary>
public static class ModuleRegistry
{
    public static IReadOnlyList<Module> All { get; } =
    [
        GodMode.Instance,
    ];
}
