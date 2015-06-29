using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hoop : MonoBehaviour 
{
	public Transform scoreTransform = null;
	List<Collider2D> _collidables = new List<Collider2D>();

	[SerializeField] float _scoreCooldownTime = 0.4f;
	Coroutine _noScoreRoutine = null;

	[SerializeField] float _minResetDistance = 2f;
	[SerializeField] Ball _ball;

	void Awake()
	{
		foreach( Collider2D collider2D in GetComponentsInChildren<Collider2D>() )
		{
			if( collider2D.isTrigger )
			{
				scoreTransform = collider2D.transform;
			}
			else
			{
				_collidables.Add( collider2D );
			}
		}

		_noScoreRoutine = null;
	}

	void FixedUpdate()
	{
		if( _noScoreRoutine == null )
		{
			foreach( Collider2D collider2D in _collidables )
			{
				collider2D.enabled = _ball.transform.position.y < transform.position.y;
			}
		}
	}

	IEnumerator TemporaryNoScoreRoutine()
	{
		foreach( Collider2D collider2D in _collidables )
		{
			collider2D.enabled = false;
		}

		while( Vector3.Distance( _ball.transform.position, transform.position ) < _minResetDistance )
		{
			yield return 0;
		}

		foreach( Collider2D collider2D in _collidables )
		{
			collider2D.enabled = true;
		}
		_noScoreRoutine = null;
	}

	public void Score()
	{
		if( _noScoreRoutine == null )
		{
			Debug.Log( "Legit score!" );
			_noScoreRoutine = StartCoroutine( TemporaryNoScoreRoutine() );
		}
	}
}
