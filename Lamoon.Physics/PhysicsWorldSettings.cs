using System.Numerics;
using JoltPhysicsSharp;

namespace Lamoon.Physics; 

public static class PhysicsWorldSettings {
    public static uint MaxBodies = 1024;

    public static uint NumBodyMutexes = 0;
    public static uint MaxBodyPairs = 1024;
    public static uint MaxContactConstraints = 1024;

    public static Dictionary<int, ObjectLayer> Layers = new(){ //Maybe use lists instead?
        {(int)CollisionTypes.NonMoving, 0},
        {(int)CollisionTypes.Moving, 1}
    };

    public static Dictionary<int, BroadPhaseLayer> BroadPhaseLayers = new(){
        {(int)CollisionTypes.NonMoving, 0},
        {(int)CollisionTypes.Moving, 1}
    };
    
    public static int NumLayers => Layers.Count;
    public static float WorldScale = 1.0f;
    
    public static Vector3 Gravity = -Vector3.UnitY*9.31f;
}