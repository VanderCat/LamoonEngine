namespace Lamoon;

public static partial class Graphics {
    public static void Arc() => throw new NotImplementedException();
    public static void Circle() => throw new NotImplementedException();
    public static void Ellipse() => throw new NotImplementedException();
    public static void Line() => throw new NotImplementedException();
    public static void Points() => throw new NotImplementedException();
    public static void Polygon() => throw new NotImplementedException();
    public static void Rectangle() => throw new NotImplementedException();
    
    public static void Print() => throw new NotImplementedException();
    public static void Printf() => throw new NotImplementedException();
    
    public static void DebugPrint(string text, ushort x = 0, ushort y = 0, byte ansiColor = 0) => dbg_text_vprintf(x, y , ansiColor, text, 0);
    public unsafe static void DebugImage(IntPtr data, ushort w, ushort h, ushort x = 0, ushort y = 0, ushort pitch = 0) => dbg_text_image(x, y, w, h, (void*)data, pitch);
    public static void DebugPrintClear(bool small = false) => dbg_text_clear(0, small);
    
    public static void Stencil() => throw new NotImplementedException();
    
    public static void FlushBatch() => throw new NotImplementedException();
    
    public static void Draw() => throw new NotImplementedException();
    public static void DrawInstanced() => throw new NotImplementedException();
    public static void DrawLayer() => throw new NotImplementedException();

    public static void Clear() => throw new NotImplementedException();
    
    public static void Present(bool capture) => frame(capture);
    public static void Discard() => discard((int)DiscardFlags.All);
}