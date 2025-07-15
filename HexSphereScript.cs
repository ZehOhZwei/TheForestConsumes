using Godot;
using System;

public class HexSphereScript : Spatial
{
    public override void _Ready()
    {
        var packedScene = GD.Load<PackedScene>("res://Tile.tscn");
        var children = GD.Load<PackedScene>("res://HexSphere.tscn").Instance().GetChildren();

        foreach (MeshInstance meshInstance in children)
        {
            var tile = packedScene.Instance();

            tile.GetChild<MeshInstance>(0).Mesh = meshInstance.Mesh;

            AddChild(tile);

        }
    }

}
