using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HitShot : Flick
{
	public override void OnFlick()
	{
		base.OnFlick();
		if(GameManager.mInstance._ballOnPitch != null &&  pFlickVector.y > 0) 
			GameManager.mInstance.LaunchBall();
	}
	
	
}