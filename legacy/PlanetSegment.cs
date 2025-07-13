using Godot;
using System;

public class PlanetSegment : Spatial
{
    public override void _Ready()
    {
        var scene = GD.Load<PackedScene>("res://PlanetSegment.tscn");
        for (int i = 0; i < 19; i++)
        {
            var segmentAmount = 72 - i * 3;
            for (int j = 0; j < segmentAmount; j++)
            {
                Spatial instance = (Spatial)scene.Instance();
                AddChild(instance);
                instance.Rotate(Vector3.Right, ((float)Math.PI) / 180 * 5 * i);
                instance.Rotate(Vector3.Up, ((float)Math.PI) / 180 * (5 + i) * j + (i + 5));
            }
        }
    }
}
