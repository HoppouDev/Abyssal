using Abyssal.Menu;
using Abyssal.Modules;
using Aether.Modding;
using Aether.Api;

namespace Abyssal;

public sealed class Startup : IModInitializer
{
    public const string ModId = "abyssal";

    public void Initialize()
    {
        RetainedUiLayer menu = RetainedUi.Register("abyssal:menu", () => new ModMenu(), initiallyVisible: false);
        Keybinds.Register("abyssal:menu", "OemTilde", () => menu.IsVisible = !menu.IsVisible);

        WorldEvents.Tick.Register(_ =>
        {
            foreach (var module in ModuleRegistry.All)
            {
                if (module.Enabled)
                    module.OnTick();
            }
        });
    }
}
