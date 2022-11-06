using SlipeServer.Packets.Definitions.Lua;
using System.Numerics;

namespace SlipeServer.Resources.GuiProxy.Gui;

public class GuiButton : GuiElement
{
    private string text;
    public string Text
    {
        get => this.text;
        set
        {
            this.text = value;
            TriggerUpdate(nameof(this.Text), value);
        }
    }

    public GuiButton(Gui gui, string text, Vector2 position, Vector2 size, GuiElement? parent = null)
        : base(gui, position, size, parent)
    {
        this.text = text;
    }

    internal override void BuildCreationTable(Dictionary<LuaValue, LuaValue> values)
    {
        base.BuildCreationTable(values);
        values["Type"] = "Button";
        values["Text"] = this.text;
    }
}
