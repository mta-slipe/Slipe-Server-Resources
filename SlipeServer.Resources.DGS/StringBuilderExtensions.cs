using SlipeServer.Resources.DGS.Style;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace SlipeServer.Resources.DGS;

internal static class StringBuilderExtensions
{
    public static void WriteIndent(this StringBuilder sb, int times)
    {
        if(times > 0)
            sb.Append(string.Join("", Enumerable.Range(0, times).Select(x => '\t')));
    }

    public static void Write(this StringBuilder sb, string property, bool value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {value.ToString().ToLower()},");
    }

    public static void Write(this StringBuilder sb, string property, float value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {value},");
    }

    public static void Write(this StringBuilder sb, string property, string? value, bool wrapInBrackets = false)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        if (wrapInBrackets)
        {
            if(value == null)
                sb.AppendLine($"{property.FirstCharToLowerCase()} = {{}},");
            else
                sb.AppendLine($"{property.FirstCharToLowerCase()} = {{\"{value}\"}},");
        }
        else
            sb.AppendLine($"{property.FirstCharToLowerCase()} = \"{value}\",");
    }

    public static void Write(this StringBuilder sb, string property, Vector2 value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{{value.X}, {value.Y}}},");
    }
    
    public static void Write(this StringBuilder sb, string property, Vector3 value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{{value.X}, {value.Y}, {value.Z}}},");
    }
    
    public static void Write(this StringBuilder sb, string property, Color value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = tocolor({value.R}, {value.G}, {value.B}, {value.A}),");
    }
    
    public static void Write(this StringBuilder sb, string property, DGSStyleColor value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{");

        {
            using var _ = new IndentScope();
            WriteIndent(sb, IndentScope.CurrentIndent);
            sb.AppendLine($"tocolor({value.Normal.R}, {value.Normal.G}, {value.Normal.B}, {value.Normal.A}),");
            WriteIndent(sb, IndentScope.CurrentIndent);
            sb.AppendLine($"tocolor({value.Hover.R}, {value.Hover.G}, {value.Hover.B}, {value.Hover.A}),");
            WriteIndent(sb, IndentScope.CurrentIndent);
            sb.AppendLine($"tocolor({value.Click.R}, {value.Click.G}, {value.Click.B}, {value.Click.A})");
        }
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine("},");
    }
    
    public static void Write(this StringBuilder sb, string property, DGSStylePadding value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{{value.Space}, {value.Relative.ToString().ToLower()}}},");
    }
    
    public static void Write(this StringBuilder sb, string property, DGSStyleSize value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{{value.Size}, {value.Relative.ToString().ToLower()}}},");
    }
    
    public static void Write(this StringBuilder sb, string property, DGSStyleUnit value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{{value.Value}, {value.Relative.ToString().ToLower()}}},");
    }
    
    public static void Write(this StringBuilder sb, string property, DGSStyleImage value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine($"{property.FirstCharToLowerCase()} = {{");

        {
            using var _ = new IndentScope();
            WriteIndent(sb, IndentScope.CurrentIndent);
            if(value.Normal != null)
                sb.AppendLine($"{{\"{value.Normal}\"}},");
            else
                sb.AppendLine("{},");
            WriteIndent(sb, IndentScope.CurrentIndent);
            if (value.Hover != null)
                sb.AppendLine($"{{\"{value.Hover}\"}},");
            else
                sb.AppendLine("{},");
            WriteIndent(sb, IndentScope.CurrentIndent);
            if (value.Click != null)
                sb.AppendLine($"{{\"{value.Click}\"}},");
            else
                sb.AppendLine("{},");
        }
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine("},");
    }

    public static void Write(this StringBuilder sb, string property, object value)
    {
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.Append($"{property.FirstCharToLowerCase()} = {{\n{value}");
        WriteIndent(sb, IndentScope.CurrentIndent);
        sb.AppendLine("},");
    }
}
