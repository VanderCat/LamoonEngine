using Lamoon.Engine;
using NekoLib.Core;
using Serilog;

namespace Lamoon.Tools.ModelViewer; 

internal class SelectedModelController : Behaviour {
    public GameObject? ModelToView;
    public delegate void ChangedModelCallback(GameObject model);

    public event ChangedModelCallback? OnModelChange;
    
    public void ChangeModel(string filePath) {
        if (ModelToView is not null) {
            Destroy(ModelToView);
        }

        try {
            ModelToView = Model.Spawn(filePath);
            ModelToView.Transform.Parent = Transform;
        }
        catch (Exception e) {
            Log.Error("Failed to load model", e);
        }

        OnModelChange?.Invoke(ModelToView);
    }
}