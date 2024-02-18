namespace Lamoon.Physics; 

/// <summary>
/// Motion type of a physics body
/// </summary>
/// <remarks>Alias for JoltPhysicsSharp.MotionType</remarks>
public enum MotionType
{
    /// <summary>
    /// Non movable
    /// </summary>
    Static = JoltPhysicsSharp.MotionType.Static,
    /// <summary>
    /// Movable using velocities only, does not respond to forces
    /// </summary>
    Kinematic = JoltPhysicsSharp.MotionType.Kinematic,
    /// <summary>
    /// Responds to forces as a normal physics object
    /// </summary>
    Dynamic =  JoltPhysicsSharp.MotionType.Dynamic
}