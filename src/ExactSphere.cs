using Godot;
using System;

public partial class ExactSphere : CharacterBody3D
{
	private Vector2 _inputPosition = new Vector2(0, 0);
	
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion eventMouseMove) {
			_inputPosition = eventMouseMove.Position;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 _position = new Vector3((_inputPosition.X/100) - 5.5f, 0, (_inputPosition.Y/100) - 4.0f);
		Position = _position;
		MoveAndSlide();
	}
}
