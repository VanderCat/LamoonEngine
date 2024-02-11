using Silk.NET.OpenGL;

namespace Lamoon.Graphics; 

public class Mesh {
    public uint VboHandle;
    public uint EboHandle;
    public uint VaoHandle;

    public float[] Vertices;
    public uint[] Indices;
    
    public PrimitiveType PrimitiveType { get; private set; }

    public Mesh(
        float[] vertices, 
        uint[] indices,
        PrimitiveType type = PrimitiveType.Triangles
        ) {
        Vertices = vertices;
        Indices = indices;
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
            const uint stride = (3 * sizeof(float)) + (2 * sizeof(float));

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

            // Unbind everything as we don't need it.
            gl.BindVertexArray(0);
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        }
    }
}