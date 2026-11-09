namespace Abyssal.Modules;

/// <summary>
/// A toggleable feature listed in the mod menu.
/// </summary>
public abstract class Module
{
    public abstract string Name { get; }

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
