using Abyssal.Modules;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Abyssal.Menu;

/// <summary>
/// Panel with one toggle button per registered module.
/// </summary>
public sealed class ModMenu : UIState
{
    private const float PanelWidth = 220;
    private const float Padding = 12;
    private const float TitleHeight = 30;
    private const float ButtonHeight = 36;
    private const float ButtonSpacing = 6;

    private static readonly Color EnabledColor = new(80, 220, 120);
    private static readonly Color DisabledColor = new(220, 90, 90);

    private readonly List<(Module Module, UITextPanel<string> Button)> _buttons = [];
    private UIPanel _panel = null!;

    public override void OnInitialize()
    {
        var modules = ModuleRegistry.All;

        _panel = new UIPanel();
        _panel.Left.Set(24, 0);
        _panel.Top.Set(260, 0);
        _panel.Width.Set(PanelWidth, 0);
        _panel.Height.Set(Padding * 2 + TitleHeight + modules.Count * (ButtonHeight + ButtonSpacing), 0);
        _panel.SetPadding(Padding);
        Append(_panel);

        var title = new UIText("Abyssal", 0.6f, large: true) { HAlign = 0.5f };
        _panel.Append(title);

        for (int i = 0; i < modules.Count; i++)
        {
            var module = modules[i];
            var button = new UITextPanel<string>(Label(module));
            button.Top.Set(TitleHeight + i * (ButtonHeight + ButtonSpacing), 0);
            button.Width.Set(0, 1);
            button.Height.Set(ButtonHeight, 0);
            button.OnLeftClick += (_, _) =>
            {
                module.Toggle();
                Refresh(module, button);
            };
            button.OnMouseOver += (_, element) => ((UIPanel)element).BorderColor = Color.Gold;
            button.OnMouseOut += (_, element) => ((UIPanel)element).BorderColor = Color.Black;
            Refresh(module, button);
            _panel.Append(button);
            _buttons.Add((module, button));
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Stop clicks on the menu from also using the held item.
        if (_panel.IsMouseHovering)
            Terraria.Main.LocalPlayer.mouseInterface = true;

        foreach (var (module, button) in _buttons)
            Refresh(module, button);
    }

    private static string Label(Module module) => $"{module.Name}: {(module.Enabled ? "ON" : "OFF")}";

    private static void Refresh(Module module, UITextPanel<string> button)
    {
        string label = Label(module);
        if (button.Text != label)
            button.SetText(label);
        button.TextColor = module.Enabled ? EnabledColor : DisabledColor;
    }
}
