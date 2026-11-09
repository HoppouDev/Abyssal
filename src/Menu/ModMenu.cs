using Abyssal.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Abyssal.Menu;

/// <summary>
/// Panel with a section per module category and one toggle button per module.
/// </summary>
public sealed class ModMenu : UIState
{
    private const float PanelWidth = 220;
    private const float Padding = 12;
    private const float TitleHeight = 30;
    private const float HeaderHeight = 24;
    private const float ButtonHeight = 36;
    private const float ButtonSpacing = 6;

    private static readonly Color HeaderColor = new(170, 180, 220);
    private static readonly Color EnabledColor = new(80, 220, 120);
    private static readonly Color DisabledColor = new(220, 90, 90);

    private readonly List<(Module Module, UITextPanel<string> Button)> _buttons = [];
    private UIPanel _panel = null!;

    public override void OnInitialize()
    {
        _panel = new UIPanel();
        _panel.Left.Set(24, 0);
        _panel.Top.Set(260, 0);
        _panel.Width.Set(PanelWidth, 0);
        _panel.SetPadding(Padding);
        Append(_panel);

        var title = new UIText("Abyssal", 0.6f, large: true) { HAlign = 0.5f };
        _panel.Append(title);

        float y = TitleHeight;
        foreach (var category in ModuleRegistry.All.GroupBy(module => module.Category).OrderBy(group => group.Key))
        {
            var header = new UIText(category.Key.ToString(), 0.9f) { TextColor = HeaderColor };
            header.Top.Set(y, 0);
            _panel.Append(header);
            y += HeaderHeight;

            foreach (var module in category)
            {
                _panel.Append(CreateButton(module, y));
                y += ButtonHeight + ButtonSpacing;
            }
        }

        _panel.Height.Set(y + Padding * 2, 0);
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

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Vanilla mouse text draws after mod layers and picks this up.
        foreach (var (module, button) in _buttons)
        {
            if (button.IsMouseHovering)
                Terraria.Main.hoverItemName = module.Description;
        }
    }

    private UITextPanel<string> CreateButton(Module module, float top)
    {
        var button = new UITextPanel<string>(Label(module));
        button.Top.Set(top, 0);
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
        _buttons.Add((module, button));
        return button;
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
