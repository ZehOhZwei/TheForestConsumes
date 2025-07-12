using Godot;
using System;
using System.Diagnostics;

public partial class PlanetScript : CSGSphere
{
	Vector3 direction = Vector3.Zero;
	float rotationAmount = 0.0025f;
	int spawnInterval = 30;
	int tickCounter = 0;

	public override void _PhysicsProcess(float delta)
	{
		// We create a local variable to store the input direction.

		if (tickCounter >= spawnInterval)
		{
			tickCounter = 0;
			var scene = GD.Load<PackedScene>("res://Forest.tscn");
			Spatial instance = (Spatial)scene.Instance();
			AddChild(instance);
			instance.GlobalPosition = new Vector3(0, 0, 1);
			instance.GlobalRotation = Vector3.Zero;
		}

		// We check for each move input and update the direction accordingly.
		if (Input.IsActionPressed("moveRight"))
		{
			Rotate(Vector3.Down, rotationAmount);
			tickCounter++;
		}
		if (Input.IsActionPressed("moveLeft"))
		{
			Rotate(Vector3.Up, rotationAmount);
			tickCounter++;
		}
		if (Input.IsActionPressed("moveDown"))
		{
			Rotate(Vector3.Left, rotationAmount);
			tickCounter++;
		}
		if (Input.IsActionPressed("moveUp"))
		{
			Rotate(Vector3.Right, rotationAmount);
			tickCounter++;
		}
	}
}


