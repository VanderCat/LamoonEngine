using System.Numerics;
using JoltPhysicsSharp;
using Lamoon.Physics.Shapes;
using NekoLib.Core;

namespace Lamoon.Physics; 

public class Rigidbody : Behaviour {
    private bool _created = false;
    private Body body;
    private BodyInterface BodyInterface;

    private JoltPhysicsSharp.MotionType _motionType = JoltPhysicsSharp.MotionType.Dynamic;

    public MotionType MotionType {
        get => (MotionType) _motionType;
        set => _motionType = (JoltPhysicsSharp.MotionType)value;
    }

    public Rigidbody() {
        BodyInterface = PhysicsWorld.Instance.BodyInterface;
    }

    private BodyCreationSettings _settings;

    private Vector3 _velocity;
    public Vector3 Velocity {
        get {
            if (!_created)
                return _velocity;
            return _velocity = body.GetLinearVelocity();
        }
        set {
            _velocity = value;
            if (_created)
                body.SetLinearVelocity(value);
        }
    }
    
    private Vector3 _angularVelocity;
    public Vector3 AngularVelocity {
        get {
            if (!_created)
                return _angularVelocity;
            return _angularVelocity = body.GetAngularVelocity();
        }
        set {
            _angularVelocity = value;
            if (_created)
                body.SetAngularVelocity(value);
        }
    }

    public bool ActiveOnStart = true;

    void Awake() {
        
    }

    void Start() {
        UpdateShape();
        CreateBody();
        BodyInterface.AddBody(body, ActiveOnStart?Activation.Activate:Activation.DontActivate);
    }

    public void UpdateShape() {
        _settings = new BodyCreationSettings(GetShape(), Transform.Position, Transform.Rotation, _motionType, PhysicsWorldSettings.Layers[GameObject.Layer]);
    }

    private Shape GetShape() {
        var shapes = GameObject.GetComponents().Where(o => o.GetType() == typeof(IShapeComponent)).Cast<IShapeComponent>();
        return shapes.First().Shape;
    }

    private void CreateBody() {
        if (_created) BodyInterface.DestroyBody(body.ID);
        body = BodyInterface.CreateBody(_settings);
    }
    
    public void AddForce(Vector3 force) {
        BodyInterface.AddForce(body.ID, force);
    }

    public void AddTorque(Vector3 direction) {
        BodyInterface.AddTorque(body.ID, direction);
    }

    void FixedUpdate() {
        Transform.LocalPosition = BodyInterface.GetPosition(body.ID); //TODO: use global things
        Transform.LocalRotation = BodyInterface.GetRotation(body.ID);
    }

    public override void Dispose() {
        base.Dispose();
        _settings.Dispose();
        BodyInterface.DestroyBody(body.ID);
    }
}