using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PlayerScript : Spatial
{
    float rotationAmount = 0.00125f;

    public override void _PhysicsProcess(float delta)
    {

        var RotationX = 0f;
        var RotationY = 0f;

        if (Input.IsActionPressed("moveRight"))
        {
            RotationX += rotationAmount;
        }
        if (Input.IsActionPressed("moveLeft"))
        {
            RotationX += -rotationAmount;
        }

        RotationY += -rotationAmount;

        RotateObjectLocal(Vector3.Forward, RotationX * 20);
        RotateObjectLocal(Vector3.Right, RotationY);

    }
}
