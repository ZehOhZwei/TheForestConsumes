using Godot;
using System;

public partial class PlanetController : CsgSphere3D
{
	private float _rotationX = 0f;
	private float _rotationY = 0f;
	private float LookAroundSpeed = 0.01f;

	/*public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// modify accumulated mouse rotation
			_rotationX += mouseMotion.Relative.X * LookAroundSpeed;
			_rotationY += mouseMotion.Relative.X * LookAroundSpeed;

			// reset rotation
			Transform3D transform = Transform;
			transform.Basis = Basis.Identity;
			Transform = transform;

			RotateObjectLocal(Vector3.Up, _rotationX); // first rotate about Y
			RotateObjectLocal(Vector3.Back, _rotationY); // then rotate about X
		}
	}*/

	public override void _PhysicsProcess(double delta)
	{
		// We create a local variable to store the input direction.
		var direction = Vector3.Zero;
		Transform3D transform = Transform;
		float rotationAmount = 0.01f;

		// We check for each move input and update the direction accordingly.
		if (Input.IsActionPressed("moveRight"))
		{
			transform.Basis = transform.Basis.Rotated(Vector3.Down, rotationAmount);
			Transform = transform;
		}
		if (Input.IsActionPressed("moveLeft"))
		{
			transform.Basis = transform.Basis.Rotated(Vector3.Up, rotationAmount);
			Transform = transform;
		}
		if (Input.IsActionPressed("moveDown"))
		{
			transform.Basis = transform.Basis.Rotated(Vector3.Left, rotationAmount);
			Transform = transform;
		}
		if (Input.IsActionPressed("moveUp"))
		{
			transform.Basis = transform.Basis.Rotated(Vector3.Right, rotationAmount);
			Transform = transform;
		}
	}
}
