using Abyssal.Modules;
using Aether.Modding;

namespace Abyssal.Mixins;

[Mixin(typeof(Terraria.Player))]
public class GodMixin
{
    [Shadow]
    public bool creativeGodMode;

    [Inject("ResetEffects()", At.RETURN)]
    private void OnResetEffects(CallbackInfo callback)
    {
        if (GodMode.Instance.Enabled)
            creativeGodMode = true;
    }
}
