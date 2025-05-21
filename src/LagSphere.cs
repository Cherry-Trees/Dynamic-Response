using Godot;
using System;

public partial class LagSphere : CharacterBody3D
{
	private Vector2 _inputPosition = Vector2.Zero;
	private Vector2 _inputVelocity = Vector2.Zero;
	private Vector2 _inputPositionPrev = Vector2.Zero;
	private Vector2 _responsePosition = Vector2.Zero;
	private Vector2 _responseVelocity = Vector2.Zero;
	private Vector2 _responseAcceleration = Vector2.Zero;
	private Vector2 _responseAccelerationPrev = Vector2.Zero;
	
	private double delta_crit;
	private int frames = 1;
	
	public double f = 2.9;
	public double zeta = 0.25;
	public double rho = 2;
	
	public double k1;
	public double k2;
	public double k3;
	
	public LagSphere() {
		k1 = zeta / (Math.PI * f);
		k2 = 1 / ((2 * Math.PI * f) * (2 * Math.PI * f));//FIX THIS//zeta / (Math.PI * f);
		k3 = (rho * zeta) / (2 * Math.PI * f);
	}
	
	public override void _Input(InputEvent @event) {
		
		if (@event is InputEventMouseMotion eventMouseMove) {
			_inputPosition = eventMouseMove.Position;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		delta_crit = Math.Sqrt(4 * k2 + k1 * k1) - k1;
		frames = (int)Mathf.Ceil(delta / delta_crit);
		delta /= frames;
		
		// Differentiate position to get velocity
		_inputVelocity.X = Convert.ToSingle((_inputPosition.X - _inputPositionPrev.X) / delta);
		_inputVelocity.Y = Convert.ToSingle((_inputPosition.Y - _inputPositionPrev.Y) / delta);
		_inputPositionPrev = _inputPosition;
		
		// Response variables describe the movement of the red sphere
		// Described by the equation: Y'' + Y' + Y = X' + X
		// Solved using Verlet Integration
		for (int i=0; i<frames; i++) {
			_responsePosition.X += Convert.ToSingle(delta*_responseVelocity.X + (_responseAcceleration.X*delta*delta*0.5));
			_responsePosition.Y += Convert.ToSingle(delta*_responseVelocity.Y + (_responseAcceleration.Y*delta*delta*0.5));
			_responseAcceleration.X = Convert.ToSingle((_inputPosition.X + k3*_inputVelocity.X - _responsePosition.X - k1*_responseVelocity.X) / k2);
			_responseAcceleration.Y = Convert.ToSingle((_inputPosition.Y + k3*_inputVelocity.Y - _responsePosition.Y - k1*_responseVelocity.Y) / k2);
			_responseVelocity.X += Convert.ToSingle((_responseAccelerationPrev.X + _responseAcceleration.X)*0.5*delta);
			_responseVelocity.Y += Convert.ToSingle((_responseAccelerationPrev.Y + _responseAcceleration.Y)*0.5*delta);
			_responseAccelerationPrev = _responseAcceleration;
		}
		
		Vector3 _position = new Vector3((_responsePosition.X/100) - 5.5f, 0, (_responsePosition.Y/100) - 4.0f);
		Position = _position;
		MoveAndSlide();
	}
	
	private void _update_k_values() {
		k1 = zeta / (Math.PI * f);
		k2 = 1 / ((2 * Math.PI * f) * (2 * Math.PI * f));
		k3 = (rho * zeta) / (2 * Math.PI * f);
	}
	
	private void _on_fslider_value_changed(double value)
	{
		f = value;
		_update_k_values();
	}
	
	private void _on_zslider_value_changed(double value)
	{
		zeta = value;
		_update_k_values();
	}
	
	private void _on_rslider_value_changed(double value)
	{
		rho = value;
		_update_k_values();
	}
}
