using System.Numerics;
using System.Reflection;
using Lamoon.Filesystem;
using Serilog;
using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Mesh {
    public uint VboHandle;
    public uint EboHandle;
    public uint VaoHandle;

    public float[] Vertices;
    public uint[] Indices;
    
    public static readonly Mesh Quad = new (new[] {
            1f, 1f, 0.0f, 1.0f, 1.0f, 0f, 0f, -1f,
            1f, -1f, 0.0f, 1.0f, 0.0f, 0f, 0f, -1f,
            -1f, -1f, 0.0f, 0.0f, 0.0f, 0f, 0f, -1f,
            -1f, 1f, 0.0f, 0.0f, 1.0f, 0f, 0f, -1f
        },
        new[] {
            0u, 1u, 3u,
            1u, 2u, 3u
        });
    
    public PrimitiveType PrimitiveType { get; private set; }

    public Mesh(
        float[] vertices, 
        uint[] indices,
        PrimitiveType type = PrimitiveType.Triangles
        ) {
        Vertices = vertices;
        Indices = indices;
        PrimitiveType = type;
        var gl = GraphicsReferences.OpenGl;
        
        VaoHandle = gl.GenVertexArray();
        gl.BindVertexArray(VaoHandle);

        VboHandle = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, VboHandle);
        unsafe {
            fixed (float* buf = vertices)
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), buf,
                    BufferUsageARB.StaticDraw);
        }

        EboHandle = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, EboHandle);
        unsafe {
            fixed (uint* buf = indices)
                gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), buf,
                    BufferUsageARB.StaticDraw);
        }

        unsafe {
            const uint stride = 8 * sizeof(float);

            // Enable the "aPosition" attribute in our vertex array, providing its size and stride too.
            const uint positionLoc = 0;
            gl.EnableVertexAttribArray(positionLoc);
            gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, stride, (void*)0);

            // Now we need to enable our texture coordinates! We've defined that as location 1 so that's what we'll use
            // here. The code is very similar to above, but you must make sure you set its offset to the **size in bytes**
            // of the attribute before.
            const uint textureLoc = 1;
            gl.EnableVertexAttribArray(textureLoc);
            gl.VertexAttribPointer(textureLoc, 2, VertexAttribPointerType.Float, false, stride,
                (void*)(3 * sizeof(float)));
            
            const uint normalLoc = 2;
            gl.EnableVertexAttribArray(normalLoc);
            gl.VertexAttribPointer(normalLoc, 3, VertexAttribPointerType.Float, false, stride,
                (void*)(5 * sizeof(float)));

            // Unbind everything as we don't need it.
            gl.BindVertexArray(0);
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        }
    }

    public static Mesh FromObjStream(Stream stream) {
        using var streamReader = new StreamReader(stream);
        var result = streamReader.ReadToEnd();
        var lines = result.Split("\n");
        var verticies = new List<Vertex>();
        var indicies = new List<uint>();
        var vectors = new List<Vector3>();
        foreach (var line in lines) {
            if (line.StartsWith("#")) continue;
            if (line.StartsWith("v ")) {
                var vert = new Vertex();
                var verts = line.Substring(2).Split(" ");
                var vector = new Vector3();
                for (var i = 0; i < verts.Length; i++) {
                    vector[i] = float.Parse(verts[i]);
                }

                vert.Position = vector;
                verticies.Add(vert);
            }

            if (line.StartsWith("vn ")) {
                var normal = line.Substring(3).Split(" ");
                var vector = new Vector3();
                for (var i = 0; i < normal.Length; i++) {
                    vector[i] = float.Parse(normal[i]);
                }
                vectors.Add(vector);
            }

            if (line.StartsWith("f")) {
                var faces = line.Substring(2).Split(" ");
                foreach (var face in faces) {
                    var vidx = uint.Parse(face.Split("/")[0])-1;
                    var vnidx = uint.Parse(face.Split("/")[2])-1;
                    verticies[(int)vidx] = verticies[(int)vidx] with {Normal = vectors[(int)vnidx]};
                    indicies.Add(vidx);
                }
            }
        }
        
        return new Mesh(verticies.BuildVerticies(), indicies.ToArray());
    }

    public static readonly Mesh Default = FromObjStream(Assembly.GetAssembly(typeof(Mesh)).GetManifestResourceStream("Lamoon.Graphics.Models.missing_model.obj"));
}