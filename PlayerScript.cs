using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PlayerScript : Spatial
{
    float speed = 0.00125f;

    int health = 100;

    public override void _PhysicsProcess(float delta)
    {

        var RotationX = 0f;
        var RotationY = 0f;

        if (Input.IsActionPressed("moveRight"))
        {
            RotationX += speed;
        }
        if (Input.IsActionPressed("moveLeft"))
        {
            RotationX += -speed;
        }

        RotationY += -speed;

        RotateObjectLocal(Vector3.Forward, RotationX * 20);
        RotateObjectLocal(Vector3.Right, RotationY);

    }
}
