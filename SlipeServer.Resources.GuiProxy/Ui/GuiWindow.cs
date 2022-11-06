using SlipeServer.Packets.Definitions.Lua;
using System.Numerics;

namespace SlipeServer.Resources.GuiProxy.Gui;
public class GuiWindow : GuiElement
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

    public GuiWindow(Gui gui, string text, Vector2 position, Vector2 size, GuiElement? parent = null)
        : base(gui, position, size, parent)
    {
        this.text = text;
    }

    internal override void BuildCreationTable(Dictionary<LuaValue, LuaValue> values)
    {
        base.BuildCreationTable(values);
        values["Type"] = "Window";
        values["Text"] = this.text;
    }
}
