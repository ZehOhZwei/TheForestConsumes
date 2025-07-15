using Godot;
using System;

public class TileScript : Spatial
{
    MeshInstance meshInstance;

    public override void _Ready()
    {
        meshInstance = GetChild<MeshInstance>(0);
        var shape = meshInstance.Mesh.CreateConvexShape();
        GetChild<Area>(1).GetChild<CollisionShape>(0).Shape = shape;
    }

    private void PlayerEntered(Node body)
    {
        var material = new SpatialMaterial();
        material.AlbedoColor = new Color(0, 1, 0);
        meshInstance.SetSurfaceMaterial(0, material);
    }
}
