using System.Reflection;

namespace Abyssal.Modules;

/// <summary>
/// A toggleable feature listed in the mod menu. Subclasses describe themselves with <see cref="ModuleInfoAttribute"/>.
/// </summary>
public abstract class Module
{
    private readonly ModuleInfoAttribute _info;

    protected Module()
    {
        _info = GetType().GetCustomAttribute<ModuleInfoAttribute>()
            ?? throw new InvalidOperationException($"{GetType().FullName} is missing [{nameof(ModuleInfoAttribute)}].");
    }

    public string Id => _info.Id;

    public string Name => _info.Name;

    public string Description => _info.Description;

    public ModuleCategory Category => _info.Category;

    public bool Enabled { get; private set; }

    public void Toggle()
    {
        Enabled = !Enabled;
        if (Enabled)
            OnEnable();
        else
            OnDisable();
    }

    protected virtual void OnEnable() { }

    protected virtual void OnDisable() { }
}
