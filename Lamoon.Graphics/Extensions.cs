namespace Lamoon.Graphics; 

public static class Extensions {
    public static float[] BuildVerticies(this IEnumerable<Vertex> vertices) {
        var ogl = new List<float>();

        foreach (var vertex in vertices)
        {
            ogl.Add(vertex.Position.X);
            ogl.Add(vertex.Position.Y);
            ogl.Add(vertex.Position.Z);
            ogl.Add(vertex.TexCoords.X);
            ogl.Add(vertex.TexCoords.Y);
        }

        return ogl.ToArray();
    }
}