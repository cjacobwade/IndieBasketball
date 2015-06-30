using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : SingletonBehaviour<ScoreManager> 
{
	int _team1Score = 0;
	int _team2Score = 0;

	Text _team1ScoreText = null;
	Text _team2ScoreText = null;

	void Awake()
	{
		_team1ScoreText = transform.GetChild( 0 ).GetComponent<Text>();
		_team2ScoreText = transform.GetChild( 1 ).GetComponent<Text>();

		_team1ScoreText.text = _team1Score.ToString();
		_team2ScoreText.text = _team2Score.ToString();
	}

	public void Score( int teamNum, int scoreAmount )
	{
		if( teamNum == 0 )
		{
			_team1Score += scoreAmount;
			_team1ScoreText.text = _team1Score.ToString();
		}
		else if( teamNum == 1 )
		{
			_team2Score += scoreAmount;
			_team2ScoreText.text = _team2Score.ToString();
		}
	}
}
