using Godot;
using System;
using System.Diagnostics;

public partial class PlanetController : CsgSphere3D
{

    Vector3 direction = Vector3.Zero;
    float rotationAmount = 0.0025f;
    int spawnInterval = 30;
    int tickCounter = 0;

    public override void _PhysicsProcess(double delta)
    {
        // We create a local variable to store the input direction.
        Transform3D transform = Transform;

        if (tickCounter >= spawnInterval)
        {
            tickCounter = 0;
            var scene = GD.Load<PackedScene>("res://ForestPlaceholder.tscn");
            Node3D instance = (Node3D)scene.Instantiate();
            AddChild(instance);
            instance.GlobalPosition = new Vector3(0, 0, 1);
            instance.GlobalRotation = Vector3.Zero;
        }

        // We check for each move input and update the direction accordingly.
        if (Input.IsActionPressed("moveRight"))
        {
            transform.Basis = transform.Basis.Rotated(Vector3.Down, rotationAmount);
            Transform = transform;
            tickCounter++;
        }
        if (Input.IsActionPressed("moveLeft"))
        {
            transform.Basis = transform.Basis.Rotated(Vector3.Up, rotationAmount);
            Transform = transform;
            tickCounter++;
        }
        if (Input.IsActionPressed("moveDown"))
        {
            transform.Basis = transform.Basis.Rotated(Vector3.Left, rotationAmount);
            Transform = transform;
            tickCounter++;
        }
        if (Input.IsActionPressed("moveUp"))
        {
            transform.Basis = transform.Basis.Rotated(Vector3.Right, rotationAmount);
            Transform = transform;
            tickCounter++;
        }
    }
}
