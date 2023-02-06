using System.Text;

namespace SlipeServer.Resources.DGS.Style;

public class DGSStyleShaderImage
{
    public string FilePath { get; set; }
    public string TextureType { get; set; } = "image";
    public object? ShaderSettings { get; set; } = null;

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.WriteIndent(IndentScope.CurrentIndent);
        {
            using var _ = new IndentScope();
            sb.WriteIndent(IndentScope.CurrentIndent);
            sb.AppendLine($"\"{FilePath}\", \"{TextureType}\", {{}},");
        }
        return sb.ToString();
    }
}
