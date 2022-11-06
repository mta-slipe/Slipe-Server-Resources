using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Elements;
using System.Numerics;

namespace SlipeServer.Resources.GuiProxy.Gui;

public class GuiElement
{
    private readonly Gui gui;

    public Guid Id { get; set; } = Guid.NewGuid();
    public List<GuiElement> Children { get; }

    private Vector2 position;
    public Vector2 Position
    {
        get => this.position;
        set
        {
            this.position = value;
            TriggerUpdate(nameof(this.Position), value.ToLuaValue());
        }
    }

    private Vector2 size;
    public Vector2 Size
    {
        get => this.size;
        set
        {
            this.size = value;
            TriggerUpdate(nameof(this.Size), value.ToLuaValue());
        }
    }

    private bool isRelative;
    public bool IsRelative
    {
        get => this.isRelative;
        set
        {
            this.isRelative = value;
            TriggerUpdate(nameof(this.IsRelative), value);
        }
    }

    private float alpha;
    public float Alpha
    {
        get => this.alpha;
        set
        {
            this.alpha = value;
            TriggerUpdate(nameof(this.Alpha), value);
        }
    }

    private bool isVisible;
    public bool IsVisible
    {
        get => this.isVisible;
        set
        {
            this.isVisible = value;
            TriggerUpdate(nameof(this.IsVisible), value);
        }
    }

    private GuiElement? parent;

    public GuiElement? Parent
    {
        get => this.parent;
        set
        {
            this.parent = value;
            TriggerUpdate(nameof(this.Parent), value == null ? new LuaValue() : value.Id.ToString());
        }
    }

    protected GuiElement(Gui gui, Vector2 position, Vector2 size, GuiElement? parent = null)
    {
        this.gui = gui;
        this.position = position;
        this.size = size;
        this.parent = parent;
        this.alpha = 255;

        this.Children = new();
    }

    protected void TriggerUpdate(string field, LuaValue value)
    {
        this.gui.TriggerUpdate(this, field, value);
    }

    internal virtual void TriggerEvent(Player player, string eventName, IEnumerable<LuaValue> parameters)
    {
        var parameterArray = parameters.ToArray();

        switch (eventName)
        {
            case "Click":
                this.Clicked?.Invoke(this, player);
                break;
        }
    }

    internal virtual void BuildCreationTable(Dictionary<LuaValue, LuaValue> values)
    {
        values["Type"] = "UNKNOWN";
        values["Id"] = this.Id.ToString();
        values["Position"] = this.position.ToLuaValue();
        values["Size"] = this.size.ToLuaValue();
        values["IsRelative"] = this.isRelative;
        values["IsVisible"] = this.isVisible;
        values["Parent"] = this.parent == null ? new LuaValue() : this.parent.Id.ToString();
    }

    public event Action<GuiElement, Player>? Clicked;
}
