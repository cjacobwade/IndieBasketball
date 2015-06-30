using UnityEngine;
using System.Collections;
using InControl;

public class PlayerPhysics : PlayerComponent 
{
	// Movement
	bool _inControl = true;
	[SerializeField] float _moveSpeed = 5f;
	[SerializeField] float _stopSpeed = 7f;
	[SerializeField] MinMaxF _defaultXSpeedRange = new MinMaxF( 0.1f, 5f );
	MinMaxF _xSpeedRange = new MinMaxF( 0f, 1f );

	// Jumping
	[SerializeField] float _jumpForce = 50f;
	[SerializeField] float _jumpCheckSidesDistance = 0.25f;
	[SerializeField] float _jumpCheckDownDistance = 0.5f;
	[SerializeField] LayerMask _jumpLayer = 0;
	[SerializeField] float _lateJumpTime = 0.1f;
	float _lateJumpTimer = 0f;
	
	bool _grounded = false;
	public bool isGrounded
	{
		get { return _grounded; }
	}

	// Rendering
	SpriteRenderer _spriteRenderer = null;
	Transform _spriteTransform = null;

	InputDevice _inputDevice = null;

	void Awake()
	{
		_spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		_spriteTransform = _spriteRenderer.GetComponent<Transform>();

		_xSpeedRange = _defaultXSpeedRange;
		_lateJumpTimer = _lateJumpTime;

		_inputDevice = InputManager.ActiveDevice;
	}

	void Update()
	{
		if( _inControl )
		{
			_grounded = Physics2D.Raycast( transform.position + Vector3.right * _jumpCheckSidesDistance, -Vector2.up, _jumpCheckDownDistance, _jumpLayer ) ||
						Physics2D.Raycast( transform.position - Vector3.right * _jumpCheckSidesDistance, -Vector2.up, _jumpCheckDownDistance, _jumpLayer );

			animator.SetBool( "isGrounded", _grounded );

			// TODO: Probably should move this out to a function that can be overriden in derived class (like Piloteer)

			if( _grounded || _lateJumpTimer < _lateJumpTime )
			{
				if( _inputDevice.Action1.WasPressed )
				{
					Vector2 currentVelocity = rigidbody2D.velocity;
					currentVelocity.y = _jumpForce;
					rigidbody2D.velocity = currentVelocity;
				}

				if( _grounded )
				{
					_lateJumpTimer = 0f;
				}
			}
		}
	
		_lateJumpTimer += Time.deltaTime;
	}

	void FixedUpdate()
	{
		float horInput = _inputDevice.LeftStick.Vector.x;

		if( _inControl && !player.ballControl.isShooting )
		{
			if( Mathf.Abs( horInput ) * rigidbody2D.velocity.x < _xSpeedRange.max )
			{
				if( !WadeUtils.IsZero( horInput ) )
				{
					if( Mathf.Abs( rigidbody2D.velocity.x ) < _xSpeedRange.min )
					{
						Vector2 currentVelocity = rigidbody2D.velocity;
						currentVelocity.x = _xSpeedRange.min * Mathf.Sign( horInput );
						rigidbody2D.velocity = currentVelocity;
					}

					Vector3 spriteScale = _spriteTransform.localScale;
					spriteScale.x = -Mathf.Sign( horInput );
					_spriteTransform.localScale = spriteScale;

					rigidbody2D.AddForce( Vector2.right * horInput * _moveSpeed );
				}
				else
				{
					ComeToStop();
				}
			}
		}

		if( WadeUtils.IsZero( horInput ) || player.ballControl.isShooting )
		{
			ComeToStop();
		}

		if( Mathf.Abs( rigidbody2D.velocity.x ) > _xSpeedRange.max )
		{
			rigidbody2D.velocity = new Vector2( Mathf.Sign( rigidbody2D.velocity.x ) * _xSpeedRange.max, rigidbody2D.velocity.y );
		}

		animator.SetFloat( "moveSpeed", Mathf.Abs( rigidbody2D.velocity.x ) );
	}

	void ComeToStop()
	{
		Vector2 currentVelocity = rigidbody2D.velocity;
		currentVelocity.x = Mathf.Lerp ( currentVelocity.x, 0f, Time.deltaTime * _stopSpeed );
		rigidbody2D.velocity = currentVelocity;
	}

	public float GetLookDirection()
	{
		return -_spriteTransform.localScale.x;
	}
}
