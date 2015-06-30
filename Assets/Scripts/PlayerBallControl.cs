using UnityEngine;
using System.Collections;
using InControl;

public class PlayerBallControl : PlayerComponent 
{
	Ball _currentBall = null;
	public Ball currentBall
	{
		get { return _currentBall; }
	}

	[SerializeField] float _catchCooldownTime = 1f; // Time until we can pick up the ball again
	float _catchCooldownTimer = 0f;

	[SerializeField] float _shotYMod = 1.4f;

	[SerializeField] float _ballVelocityInheritScale = 0.5f;

	[SerializeField] float _dribbleSpeed = 5f;
	float _dribbleTimer = 0f;

	float _currentBallHeightOffset = 0.1f;
	[SerializeField] MinMaxF _ballHeightOffsetRange = new MinMaxF( 0.1f, 0.43f );

	[SerializeField] float _ballFollowSpeed = 10f;

	[SerializeField] bool _debugCanShoot = true;

	// Shooting
	bool _isShooting = false;
	public bool isShooting
	{
		get { return _isShooting; }
	}

	[SerializeField] MinMaxF _shotForceRange = new MinMaxF( 3f, 7f );

	[SerializeField] float _maxShootTime = 1.5f;
	float _shootTimer = 0f;

	// Stealing
	[SerializeField] float _stealCooldownTime = 0.7f;
	float _stealCooldownTimer = 0f;

	[SerializeField] float _stealCheckRadius = 1.5f;
	[SerializeField] LayerMask _playerLayer = 0;

	// Input
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
			if( _debugCanShoot )
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
		else if( _inputDevice.Action3.WasPressed && _stealCooldownTimer > _stealCooldownTime )
		{
			AttemptSteal();
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

		_stealCooldownTimer += Time.deltaTime;
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

	void AttemptSteal()
	{
		_stealCooldownTimer = 0f;

		Collider2D[] nearbyPlayerColliders = Physics2D.OverlapCircleAll( transform.position, _stealCheckRadius, _playerLayer );
		if( nearbyPlayerColliders.Length > 0 )
		{
			PlayerBallControl closestBallHolder = null;
			float closestBallHolderDistance = Mathf.Infinity;
			foreach( Collider2D collider2D in nearbyPlayerColliders )
			{
				PlayerBallControl ballHolder = collider2D.GetComponent<PlayerBallControl>();
				if( ballHolder && ballHolder != this )
				{
					if( closestBallHolder )
					{
						float ballHolderDistance = Vector3.Distance( closestBallHolder.transform.position, transform.position );
						if( ballHolderDistance < closestBallHolderDistance )
						{
							closestBallHolder = ballHolder;
							closestBallHolderDistance = ballHolderDistance;
						}
					}
					else
					{
						closestBallHolder = ballHolder;
						closestBallHolderDistance = Vector3.Distance( closestBallHolder.transform.position, transform.position );
					}
				}
			}

			if( closestBallHolder && closestBallHolder.currentBall )
			{
				Ball ball = closestBallHolder.currentBall;
				closestBallHolder.DropBall();
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
		_stealCooldownTimer = 0f;
		_catchCooldownTimer = 0f;
	}

	void DropBall()
	{
		_isShooting = false;

		_currentBall.OnRelease();
		_currentBall = null;

		_stealCooldownTimer = 0f;
		_catchCooldownTimer = 0f;
	}
}
