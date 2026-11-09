namespace Abyssal.Modules;

/// <summary>
/// Identifies a module and places it in the menu.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ModuleInfoAttribute(string id, string name, string description, ModuleCategory category) : Attribute
{
    public string Id { get; } = id;

    public string Name { get; } = name;

    public string Description { get; } = description;

    public ModuleCategory Category { get; } = category;
}
