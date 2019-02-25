using UnityEngine;
using System.Collections;

public class GameState 
{
	public enum States 
	{
		IDLE,
		COUNTDOWN,
		PLAYING,
		RESULTSCREEN,
		PAUSED,
		STOPPED,

	}
	
	public static States state = States.IDLE;
	
	public static void ChangeState(States stateTo)
	{
		if(state == stateTo) 
			return;  
		state = stateTo;  
	}
	
	public static bool IsState(States stateTo) 
	{        
		if(state == stateTo)
			return true;
		return false;
	}
}
