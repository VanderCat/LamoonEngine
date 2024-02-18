using System.Diagnostics;
using System.Numerics;
using JoltPhysicsSharp;
using Lamoon.Engine;
using Serilog;

namespace Lamoon.Physics; 

public class PhysicsWorld : IDisposable {
    private static PhysicsWorld? _instance;

    public static PhysicsWorld Instance {
        get {
            _instance ??= new PhysicsWorld();
            return _instance;
        }
    }

    public static bool UseTable = true;

    private PhysicsSystemSettings settings;
    private PhysicsSystem physicsSystem;

    public BodyInterface BodyInterface => physicsSystem.BodyInterface;

    private PhysicsWorld() {
        if (!Foundation.Init())
            throw new Exception("Could not initialize Jolt Physics!");
        ObjectLayerPairFilter objectLayerPairFilter;
        BroadPhaseLayerInterface broadPhaseLayerInterface;
        ObjectVsBroadPhaseLayerFilter objectVsBroadPhaseLayerFilter;
        if (UseTable) {
            // We use only 2 layers: one for non-moving objects and one for moving objects
            ObjectLayerPairFilterTable objectLayerPairFilterTable = new(2);
            objectLayerPairFilterTable.EnableCollision(PhysicsWorldSettings.Layers[(int)CollisionTypes.NonMoving], PhysicsWorldSettings.Layers[(int)CollisionTypes.Moving]); //TODO: Collision Matrix
            objectLayerPairFilterTable.EnableCollision(PhysicsWorldSettings.Layers[(int)CollisionTypes.Moving], PhysicsWorldSettings.Layers[(int)CollisionTypes.Moving]);

            // We use a 1-to-1 mapping between object layers and broadphase layers
            BroadPhaseLayerInterfaceTable broadPhaseLayerInterfaceTable = new(2, 2);
            broadPhaseLayerInterfaceTable.MapObjectToBroadPhaseLayer(PhysicsWorldSettings.Layers[(int)CollisionTypes.NonMoving], PhysicsWorldSettings.BroadPhaseLayers[(int)CollisionTypes.NonMoving]);
            broadPhaseLayerInterfaceTable.MapObjectToBroadPhaseLayer(PhysicsWorldSettings.Layers[(int)CollisionTypes.Moving], PhysicsWorldSettings.BroadPhaseLayers[(int)CollisionTypes.Moving]);

            objectLayerPairFilter = objectLayerPairFilterTable;
            broadPhaseLayerInterface = broadPhaseLayerInterfaceTable;
            objectVsBroadPhaseLayerFilter = new ObjectVsBroadPhaseLayerFilterTable(broadPhaseLayerInterfaceTable, 2, objectLayerPairFilterTable, 2);
        }
        else {
            objectLayerPairFilter = new ObjectLayerPairFilterMask();

            // Layer that objects can be in, determines which other objects it can collide with
            // Typically you at least want to have 1 layer for moving bodies and 1 layer for static bodies, but you can have more
            // layers if you want. E.g. you could have a layer for high detail collision (which is not used by the physics simulation
            // but only if you do collision testing).
            const uint NUM_BROAD_PHASE_LAYERS = 2;

            BroadPhaseLayerInterfaceMask bpInterface = new(NUM_BROAD_PHASE_LAYERS);
            //bpInterface.ConfigureLayer(BroadPhaseLayers.NonMoving, GROUP_STATIC, 0); // Anything that has the static bit set goes into the static broadphase layer
            //bpInterface.ConfigureLayer(BroadPhaseLayers.Moving, GROUP_FLOOR1 | GROUP_FLOOR2 | GROUP_FLOOR3, 0); // Anything that has one of the floor bits set goes into the dynamic broadphase layer

            broadPhaseLayerInterface = bpInterface;
            objectVsBroadPhaseLayerFilter = new ObjectVsBroadPhaseLayerFilterMask(bpInterface);
        }
        settings = new()
        {
            ObjectLayerPairFilter = objectLayerPairFilter,
            BroadPhaseLayerInterface = broadPhaseLayerInterface,
            ObjectVsBroadPhaseLayerFilter = objectVsBroadPhaseLayerFilter
        };
        physicsSystem = new(settings);
        physicsSystem.Gravity = PhysicsWorldSettings.Gravity;
    }
    
    public void Update() {
        var error = physicsSystem.Step(Time.FixedDelta, (int)(1f / 60f / Time.FixedDelta));
        if (error is not PhysicsUpdateError.None)
            Log.Warning("Physics error! {Error}", error);
    }

    public void Dispose() {
        physicsSystem.Dispose();
        Foundation.Shutdown();
    }
}