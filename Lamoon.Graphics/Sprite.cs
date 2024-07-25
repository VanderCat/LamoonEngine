using System.Drawing;

namespace Lamoon.Graphics; 

public class Sprite {
    public Material Material;
    public Rectangle Bounds;
    
    public float Width => Bounds.Width;
    public float Height => Bounds.Height;
    
    private Mesh SpriteMesh => Mesh.Quad;

    public static readonly Sprite Default = new(Material.Default);

    public Sprite(Material material, Rectangle bounds) {
        Material = material;
        Bounds = bounds;
    }

    public Sprite(Material material) : this(material, new Rectangle(new Point(0, 0), material.Textures[0].Size)) { }

    public static Sprite FromFilesystem(string location, Rectangle bounds) =>
        new Sprite(Graphics.Material.FromFilesystem(location), bounds);

    public static Sprite FromFilesystem(string location) =>
        new Sprite(Graphics.Material.FromFilesystem(location));
}