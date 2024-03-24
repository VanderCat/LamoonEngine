using NekoLib.Core;

namespace Lamoon.Scripting; 

public class ScriptableBehaviour : Behaviour {
    public string? Name = "ScriptableBehaviour";
    public string? ScriptPath;
    public object? ScriptBlob;

    public virtual void Init() {
        if (ScriptPath is null) throw new Exception("No Script Path provided to the Scriptable Behaviour!");
        ScriptBlob =  ScriptingManager.ExecuteFromFileSystem(ScriptPath);
        //if (result is null) throw new NullReferenceException("The provided script has no return string");
    }
    public override void Invoke(string methodName, object? o = null) {
        if (methodName == "StartIfNeeded") {
            base.Invoke(methodName, o);
            return;
        }
        ScriptingManager.ScriptingProvider.InvokeMethod(Name, methodName, o);
    }
}