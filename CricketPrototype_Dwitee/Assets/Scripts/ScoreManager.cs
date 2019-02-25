using UnityEngine;
using System.Collections;

/// <summary>
/// </summary>
public class ScoreManager
{
	private static ScoreManager mInstance = null;

	private int mScore = 0;
	private int mBestScore = 0;

	//making it singleton as to make only one instance of score in the game.
	public static ScoreManager GetInstance()
	{
		if (mInstance == null) 
		{
			mInstance = new ScoreManager ();
		}
		return mInstance;
	}

	public int GetCurrentScore()
	{
		return mScore;
	}
	public void SetBestScore(int score)
	{
		mBestScore = score;
	}
	public int GetBestScore()
	{
		return mBestScore;
	}
	public void AddToScore( int score)
	{
		mScore += score;
	}
	public void ResetScore()
	{
		mScore = 0;
	}
	public int CalculateScore( float AngleOfHit, float ForceMagnitude)
	{
		if ( AngleOfHit >= 30 && ForceMagnitude >= 25)
			return 6;
		else if (  AngleOfHit >= 20 && ForceMagnitude >= 25)
			return 4;
		else
			return 2;

	}

}
