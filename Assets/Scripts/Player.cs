using UnityEngine;
using System.Collections;

public class Player : WadeBehaviour 
{
	PlayerBallControl _ballControl = null;
	public PlayerBallControl ballControl
	{
		get
		{
			if( !_ballControl )
			{
				_ballControl = GetComponent<PlayerBallControl>();
			}

			return _ballControl;
		}
	}

	PlayerPhysics _physics = null;
	public PlayerPhysics physics
	{
		get
		{
			if( !_physics )
			{
				_physics = GetComponent<PlayerPhysics>();
			}

			return _physics;
		}
	}

	void Awake()
	{
		foreach( PlayerComponent playerComponent in GetComponentsInChildren<PlayerComponent>() )
		{
			playerComponent.player = this;
		}
	}
}
