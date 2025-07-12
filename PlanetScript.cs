using Godot;
using System.Linq;
using System;
using System.Diagnostics;

public partial class PlanetScript : CSGSphere
{
	Vector3 direction = Vector3.Zero;
	float rotationAmount = 0.0025f;
	int spawnInterval = 30;
	int moveCounter = 0;
	bool first = true;

	public override void _PhysicsProcess(float delta)
	{
		var isInForest = false;
		var forestHead = GetParent().GetChild<Spatial>(3);
		var areas = forestHead.GetChild<Area>(1).GetOverlappingAreas();
		foreach (Area area in areas)
		{
			if (area.GetParent().IsInGroup("forest"))
			{
				isInForest = true;
			}
		}

		if (moveCounter >= spawnInterval && !isInForest)
		{
			moveCounter = 0;
			var scene = GD.Load<PackedScene>("res://Forest.tscn");
			Spatial instance = (Spatial)scene.Instance();
			AddChild(instance);
			instance.GlobalPosition = new Vector3(0, 0, 1);
			instance.GlobalRotation = Vector3.Zero;
			if (first)
			{
				instance.Call("Grow");
				first = false;
			}
			else
			{
				instance.AddToGroup("growing");
			}
		}

		var RotationX = 0f;
		var RotationY = 0f;


		// We check for each move input and update the direction accordingly.
		if (Input.IsActionPressed("moveRight"))
		{
			RotationX += -rotationAmount;
		}
		if (Input.IsActionPressed("moveLeft"))
		{
			RotationX += rotationAmount;
		}
		if (Input.IsActionPressed("moveDown"))
		{
			RotationY += -rotationAmount;
		}
		if (Input.IsActionPressed("moveUp"))
		{
			RotationY += rotationAmount;
		}

		if (RotationX != 0f && RotationY != 0f)
		{
			Rotate(Vector3.Up, RotationX / 1.5f);
			Rotate(Vector3.Right, RotationY / 1.5f);

			moveCounter++;
		}

		else if (RotationX != 0 || RotationY != 0)
		{
			Rotate(Vector3.Up, RotationX);
			Rotate(Vector3.Right, RotationY);

			moveCounter++;
		}

	}
}


