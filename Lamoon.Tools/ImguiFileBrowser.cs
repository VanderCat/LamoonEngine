using Lamoon.Engine;
using Lamoon.Engine.Components;
using Lamoon.Filesystem;
using Lamoon.Graphics;
using NekoLib.Core;
using Silk.NET.OpenGL;
using Texture = Lamoon.Graphics.Texture;

namespace Lamoon.Tools; 

public class ImguiFileBrowser : Behaviour {
    public List<FileBrowserFileDefinition>? Files;
    public string CurrentPath { get; private set; }

    void DrawGui() {
        if (ImGui.Begin("Game View")) {
            
        }
        ImGui.End();
    }

    void RefreshCurrentFolder() {
        Files = new();
        var files = Filesystem.Files.ListDirectory(CurrentPath);
        foreach (var filepath in files) {
            var file = new FileBrowserFileDefinition(Filesystem.Files.GetFile(filepath));
            Files.Add(file);
        }
    }

    void OpenFolder(string path) {
        CurrentPath = path;
        RefreshCurrentFolder();
    }
}

public class FileBrowserFileDefinition {
    public IFile File { get; }
    public string Name { get; }
    public string Extension { get; }
    public string FullName { get; }

    internal FileBrowserFileDefinition(IFile file) {
        File = file;
        var path = file.Path;
        Name = Path.GetFileNameWithoutExtension(path);
        Extension = Path.GetExtension(path);
        FullName = Path.GetFileName(path);
    }
}