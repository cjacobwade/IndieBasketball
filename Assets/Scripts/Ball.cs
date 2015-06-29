using UnityEngine;
using System.Collections;
using InControl;

public class Ball : WadeBehaviour 
{
	Player _lastCarrier = null;
	Collider2D _playerTrigger = null;

	SpriteRenderer _spriteRenderer = null;
	MinMaxI _frontAndBackLayers = new MinMaxI( 1, 3 );

	Collider2D _hoopCollider = null;

	void Awake()
	{
		_playerTrigger = transform.GetChild(0).GetComponent<Collider2D>();
		_hoopCollider = transform.GetChild(1).GetComponent<Collider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		Debug.Log( Input.GetJoystickNames().Length + InputManager.Devices.Count );
	}

	void FixedUpdate()
	{
		if( rigidbody2D.velocity.y > 0f )
		{
			_spriteRenderer.sortingOrder = _frontAndBackLayers.max;
			_hoopCollider.enabled = true;
		}
		else
		{
			_spriteRenderer.sortingOrder = _frontAndBackLayers.min;
			_hoopCollider.enabled = false;
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if( other.transform.parent )
		{
			Hoop hoop = other.transform.parent.GetComponent<Hoop>();
			if( hoop )
			{
				Vector3 toHoop = hoop.scoreTransform.position - transform.position;
				if( Vector3.Dot( toHoop, Vector3.down ) > 0f )
				{
					hoop.Score();
				}
			}
		}
	}

	public void OnCatch( Player player )
	{
		_lastCarrier = player;
		rigidbody2D.isKinematic = true;
		collider2D.enabled = false;
		_playerTrigger.enabled = false;
	}

	public void OnRelease()
	{
		rigidbody2D.isKinematic = false;
		collider2D.enabled = true;
		_playerTrigger.enabled = true;
	}
}
