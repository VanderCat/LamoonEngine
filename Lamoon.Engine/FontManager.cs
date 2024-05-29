using System.Runtime.InteropServices;
using System.Text;
using ImGuiNET;
using Lamoon.Filesystem;

namespace Lamoon.Engine; 

public static class FontManager {

    private class FontPtrInfo {
        public IntPtr Pointer;
        public IFile File;
        public Byte[] Contents;
        public int Length => Contents.Length;

        public FontPtrInfo(string fontpath) {
            File = Files.GetFile(fontpath);
            Contents = File.ReadBinary();
            Pointer = Marshal.AllocHGlobal(Length);
            Marshal.Copy(Contents, 0, Pointer, Length);
        }
    }
    public static unsafe void SetupFonts() {
        var io = ImGui.GetIO();
        
        var robotoMono = new FontPtrInfo("Fonts/RobotoMono.ttf");
        var mdi = new FontPtrInfo("Fonts/MDI2.ttf");

        var fontcfg = ImGuiNative.ImFontConfig_ImFontConfig();
        fontcfg->OversampleH = 2;
        fontcfg->OversampleV = 2;
        fontcfg->SizePixels = 24f;
        string name = "RobotoMono +MDI, 16px";
        for (int i = 0; i < name.Length && i < 40; ++i)
        {
            fontcfg->Name[i] = Convert.ToByte(name[i]);
        }
            
        var range = new ushort[] {MaterialIcons.IconMin, MaterialIcons.IconMax16, 0 };
        var rangeHandle = GCHandle.Alloc(range, GCHandleType.Pinned);

        io.Fonts.AddFontFromMemoryTTF(robotoMono.Pointer, robotoMono.Length, 16f, fontcfg, io.Fonts.GetGlyphRangesDefault());
        fontcfg->MergeMode = 1;
        fontcfg->GlyphMinAdvanceX = 24f;
        fontcfg->PixelSnapH = 1;
        io.Fonts.AddFontFromMemoryTTF(mdi.Pointer, mdi.Length, 16f, fontcfg,  rangeHandle.AddrOfPinnedObject());

        io.Fonts.Build();
    }
}