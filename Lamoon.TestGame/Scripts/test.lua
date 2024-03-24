import("NekoLib", "NekoLib.Core")
import("Lamoon.Engine", "Lamoon.Engine")
import("Serilog", "Serilog")
import("System.Numerics")

TestScript = {
    position = Vector3.Zero;
}

function TestScript:Awake()
    Log.Information("Hello World!")
    self.position = self.this.Transform.LocalPosition;
end

function TestScript:Update()
    self.this.Transform.LocalPosition = Vector3(self.position.X, self.position.Y+math.sin(Time.CurrentTime*32)*16, self.position.Z)
end

return TestScript