using UnityEngine;
using System.Collections;


public class BattingRange : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerEnter( Collider other)
	{
		if (other.gameObject.tag == "ball") 
		{
			GameManager.mInstance._ballOnPitch = other.gameObject;
		}
	}

	
	void OnTriggerExit( Collider other)
	{
		if (other.gameObject.tag == "ball")
		{
			GameManager.mInstance._ballOnPitch = null;
			if (other.gameObject.GetComponent<CricketBall>()._BallLaunched == false)
			{
				HUDManager.GetInstance().DisplayScoreOnScreen(0,"Score");
				Debug.Log("MISS");
			}
		}
	}
}