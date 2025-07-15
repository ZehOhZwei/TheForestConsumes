using Godot;
using System;

public class HexSphereScript : Spatial
{
    public override void _Ready()
    {
        var packedScene = GD.Load<PackedScene>("res://Tile.tscn");
        var childFaces = GD.Load<PackedScene>("res://HexSphere5.tscn").Instance().GetChildren();

        foreach (MeshInstance meshInstance in childFaces)
        {
            var tile = packedScene.Instance();

            tile.GetChild<MeshInstance>(0).Mesh = meshInstance.Mesh;

            AddChild(tile);

        }

        foreach (Spatial child in GetChildren())
        {
            child.CallDeferred("RegisterAdjacentTiles");
        }
    }

}
