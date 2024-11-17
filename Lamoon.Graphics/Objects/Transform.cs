using System.Numerics;

namespace Lamoon;

public struct Transform {
    public Transform(Matrix4x4 mat) {
        Matrix = mat;
    }

    public Transform(Vector3 pos, Quaternion rotation, Vector3 scale, Vector3 origin) {
        Matrix = Matrix4x4.CreateTranslation(origin);
        Matrix = Matrix4x4.Transform(Matrix, rotation);
        Matrix *= Matrix4x4.CreateScale(scale);
        Matrix *= Matrix4x4.CreateTranslation(pos);
    }
    
    public Transform(Vector2 pos, float rotation, Vector2 scale, Vector2 origin) : 
        this(
            new Vector3(origin, 0), 
            Quaternion.CreateFromAxisAngle(Vector3.UnitZ, rotation),
            new Vector3(scale, 1),
            new Vector3(origin, 0)
            ) { }
    
    public Matrix4x4 Matrix { get; private set; }

    public Transform Apply(Transform transform) => new(Matrix * transform.Matrix);

    public static Transform operator *(Transform transform1, Transform transform2) => transform1.Apply(transform2);

    public Transform Inverse() {
        if (Matrix4x4.Invert(Matrix, out var inverted)) {
            return new Transform(inverted);
        }
        throw new InvalidDataException("Could not Invert this Transform");
    }

    public Vector3 TransformPoint(Vector3 position) => Vector3.Transform(position, Matrix);
    public Vector2 TransformPoint(Vector2 position) => Vector2.Transform(position, Matrix);

    public Vector3 InverseTransformPoint(Vector3 position) => Inverse().TransformPoint(position);
    public Vector2 InverseTransformPoint(Vector2 position) => Inverse().TransformPoint(position);

    public Transform Rotate(Quaternion quaternion) => new(Matrix4x4.Transform(Matrix, quaternion));

    public Transform Rotate(float angle) => Rotate(Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle));

    public Transform Scale(Vector3 scale) => new(Matrix * Matrix4x4.CreateScale(scale));

    public Transform Shear(Vector2 shear) {
        throw new NotImplementedException();
    }

    public Transform Translate(Vector3 position) => new(Matrix * Matrix4x4.CreateTranslation(position));
}