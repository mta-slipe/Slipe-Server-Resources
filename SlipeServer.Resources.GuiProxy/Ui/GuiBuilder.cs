using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Resources.GuiProxy.Gui;
public class GuiBuilder
{
    private readonly LuaEventService luaEventService;
    private readonly RootElement rootElement;
    private readonly Gui gui;

    internal GuiBuilder(LuaEventService luaEventService, RootElement rootElement)
    {
        this.luaEventService = luaEventService;
        this.rootElement = rootElement;

        this.gui = new Gui(this.luaEventService, this.rootElement);
    }

    public GuiWindow AddWindow(string title, Vector2 position, Vector2 size, bool isRelative = false, float alpha = 255, bool isVisible = false)
    {
        var element = new GuiWindow(this.gui, title, position, size)
        {
            IsRelative = isRelative,
            Alpha = alpha,
            IsVisible = isVisible
        };
        this.gui.AddElement(element);
        return element;
    }

    public GuiButton AddButton(string text, Vector2 position, Vector2 size, GuiElement? parent = null, bool isRelative = false, float alpha = 255, bool isVisible = true)
    {
        var element = new GuiButton(this.gui, text, position, size, parent)
        {
            IsRelative = isRelative,
            Alpha = alpha,
            IsVisible = isVisible
        };
        this.gui.AddElement(element);
        return element;
    }

    public GuiLabel AddLabel(string text, Vector2 position, Vector2 size, GuiElement? parent = null, bool isRelative = false, float alpha = 255, bool isVisible = true)
    {
        var element = new GuiLabel(this.gui, text, position, size, parent)
        {
            IsRelative = isRelative,
            Alpha = alpha,
            IsVisible = isVisible
        };
        this.gui.AddElement(element);
        return element;
    }

    public Gui Build()
    {
        return this.gui;
    }
}
