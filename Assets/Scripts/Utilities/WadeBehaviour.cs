using UnityEngine;
using System.Collections;

public class WadeBehaviour : MonoBehaviour
{
	[HideInInspector] Transform _transform = null;
	public new Transform transform
	{
		get
		{
			if ( !_transform )
			{
				_transform = GetComponent<Transform>();
			}

			return _transform;
		}
	}

	[HideInInspector] Renderer _renderer = null;
	public new Renderer renderer
	{
		get
		{
			if ( !_renderer )
			{
				_renderer = GetComponent<Renderer>();
			}

			return _renderer;
		}
	}

	[HideInInspector] Camera _camera = null;
	public new Camera camera
	{
		get
		{
			if ( !_camera )
			{
				_camera = GetComponent<Camera>();
			}

			return _camera;
		}
	}

	[HideInInspector] Collider _collider = null;
	public new Collider collider
	{
		get
		{
			if ( !_collider )
			{
				_collider = GetComponent<Collider>();
			}

			return _collider;
		}
	}

	[HideInInspector] Collider2D _collider2D = null;
	public new Collider2D collider2D
	{
		get
		{
			if ( !_collider2D )
			{
				_collider2D = GetComponent<Collider2D>();
			}

			return _collider2D;
		}
	}

	[HideInInspector] Rigidbody _rigidbody = null;
	public new Rigidbody rigidbody
	{
		get
		{
			if ( !_rigidbody )
			{
				_rigidbody = GetComponent<Rigidbody>();
			}

			return _rigidbody;
		}
	}

	[HideInInspector] Rigidbody2D _rigidbody2D = null;
	public new Rigidbody2D rigidbody2D
	{
		get
		{
			if ( !_rigidbody2D )
			{
				_rigidbody2D = GetComponent<Rigidbody2D>();
			}

			return _rigidbody2D;
		}
	}

	[HideInInspector] Animator _animator = null;
	public Animator animator
	{
		get
		{
			if( !_animator )
			{
				_animator = GetComponent<Animator>();
			}

			return _animator;
		}
	}
}
