using UnityEngine;
using System.Collections;
using InControl;

public class PlayerBallControl : PlayerComponent 
{
	Ball _currentBall = null;

	[SerializeField] float _catchCooldownTime = 1f; // Time until we can pick up the ball again
	float _catchCooldownTimer = 0f;

	[SerializeField] float _shotYMod = 1.4f;

	[SerializeField] float _ballVelocityInheritScale = 0.5f;

	[SerializeField] float _dribbleSpeed = 5f;
	float _dribbleTimer = 0f;

	float _currentBallHeightOffset = 0.1f;
	[SerializeField] MinMaxF _ballHeightOffsetRange = new MinMaxF( 0.1f, 0.43f );

	[SerializeField] float _ballFollowSpeed = 10f;

	// Shooting
	bool _isShooting = false;
	public bool isShooting
	{
		get { return _isShooting; }
	}

	[SerializeField] MinMaxF _shotForceRange = new MinMaxF( 3f, 7f );

	[SerializeField] float _maxShootTime = 1.5f;
	float _shootTimer = 0f;

	InputDevice _inputDevice = null;

	void Awake()
	{
		_catchCooldownTimer = _catchCooldownTime;
		_inputDevice = InputManager.ActiveDevice;
	}

	void Update()
	{
		if( _currentBall )
		{
			if( _inputDevice.Action3.WasPressed )
			{
				_isShooting = true;
				_shootTimer = 0f;
			}

			if( _isShooting && _inputDevice.Action3.WasReleased )
			{
				// TODO: Clamp these to character specific angles
				ShootBall( _inputDevice.LeftStick.Vector, _shotForceRange.Lerp( _shootTimer/_maxShootTime ) );

				_isShooting = false;
			}
		}
	}

	void FixedUpdate()
	{
		if( _currentBall )
		{
			if( _isShooting && _inputDevice.Action3.IsPressed )
			{
				_shootTimer += Time.deltaTime;
			}

			if( player.physics.isGrounded )
			{
				_currentBallHeightOffset = Mathf.Sin( _dribbleTimer ) * 0.5f + 0.5f;
				_currentBallHeightOffset = _ballHeightOffsetRange.Lerp( _currentBallHeightOffset );

				_dribbleTimer += Time.deltaTime * _dribbleSpeed;
			}

			_currentBall.transform.position = Vector3.Lerp( _currentBall.transform.position, transform.position + Vector3.up * _currentBallHeightOffset, Time.deltaTime * _ballFollowSpeed );
		}

		_catchCooldownTimer += Time.deltaTime;
	}

	void OnTriggerStay2D( Collider2D other )
	{
		if( !_currentBall && _catchCooldownTimer > _catchCooldownTime )
		{
			Ball ball = other.transform.GetComponent<Ball>();
			if( ball )
			{
				CatchBall( ball );
			}
		}
	}

	void CatchBall( Ball ball )
	{
		_dribbleTimer = 0f;

		_currentBall = ball;
		_currentBall.OnCatch( player );
	}

	void ShootBall( Vector2 shootDirection, float shootForce )
	{
		_currentBall.OnRelease();

		Vector2 ballVelocity = shootDirection * shootForce + rigidbody2D.velocity * _ballVelocityInheritScale;
		ballVelocity.y += _shotYMod;
		_currentBall.rigidbody2D.velocity = ballVelocity;

		_currentBall = null;

		_shootTimer = 0f;
		_catchCooldownTimer = 0f;
	}

	void DropBall()
	{
		_currentBall.OnRelease();
		_currentBall = null;

		_catchCooldownTimer = 0f;
	}
}
