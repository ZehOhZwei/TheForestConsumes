using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public class ForestScript : Area
{
    int i = 0;

    int objects = 0;

    int maxObjects = 10;

    Vector3 newPoint = new Vector3(1, 0, 0);

    Random rand = new Random();

    public override void _Process(float delta)
    {
        ConvexPolygonShape shape = (ConvexPolygonShape)GetChild<CollisionShape>(0).Shape;
        maxObjects = shape.Points.Count();

        newPoint = newPoint.Rotated(Vector3.Right, ((float)Math.PI) / 180 * rand.Next(360));
        newPoint = newPoint.Rotated(Vector3.Up, ((float)Math.PI) / 180 * rand.Next(360));
        newPoint = newPoint.Rotated(Vector3.Back, ((float)Math.PI) / 180 * rand.Next(360));
        newPoint = newPoint * 1;
        var spaceState = GetWorld().DirectSpaceState;
        var result = spaceState.IntersectPoint(newPoint, collideWithBodies: false, collideWithAreas: true);
        if (result.Count > 0)
        {

            if (objects < maxObjects)
            {
                var scene = GD.Load<PackedScene>("res://Forest.tscn");
                Spatial instance = (Spatial)scene.Instance();
                GetParent().AddChild(instance);
                instance.GlobalPosition = newPoint;
                instance.LookAt(Vector3.Zero, Vector3.Forward);
                objects++;
            }
        }
        i = 0;

        i++;
    }
}
