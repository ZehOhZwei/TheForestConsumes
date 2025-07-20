using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static GameScript;

public class PlayerScript : Spatial
{
    float Speed = 0.00125f;
    int Health = 100;
    int TileStatus;
    bool stopped = false;
    ProgressBar HealthBar;

    public override void _Ready()
    {
        HealthBar = GetChild<ProgressBar>(2);
    }


    public override void _PhysicsProcess(float delta)
    {
        if (!stopped)
        {
            var RotationX = 0f;
            var RotationY = 0f;

            if (Input.IsActionPressed("moveRight"))
            {
                RotationX += Speed;
            }
            if (Input.IsActionPressed("moveLeft"))
            {
                RotationX += -Speed;
            }

            RotationY += -Speed;

            RotateObjectLocal(Vector3.Forward, RotationX * 20);
            RotateObjectLocal(Vector3.Right, RotationY);

            Health += TileStatus;
            if (Health > 100)
            {
                Health = 100;
            }
            else if (Health < 0)
            {
                EmitSignal(nameof(game_over), false);
                Health = 0;
            }
            HealthBar.Value = Health;
        }
    }

    public void Stop()
    {
        stopped = true;
    }

    private void TileEntered(Area area)
    {
        TileStatus = (int)area.GetParent().Get("TileStatus");
    }

    [Signal]
    public delegate void game_over(bool victory);
}
