using UnityEngine;
using System.Collections;

public class ShotTiming : MonoBehaviour {

	public HitShot _FlickObj = null;
	public MinMax _ForceForThisTiming = new MinMax(1,30);
	public float _AngleForThisTIming = 45.0f;
	protected MinMax _temp = null;
	// Use this for initialization
	void Start () 
	{
		if ( _FlickObj != null)
			_temp = _FlickObj._Force;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter( Collider other)
	{
		if (other.gameObject.tag == "ball") 
		{

			if (this.gameObject.name.StartsWith  ("TriggerFor6"))
			{
				other.gameObject.transform.FindChild("glow6").gameObject.SetActive(true);
				if ( this.gameObject.name == "TriggerFor6_low")
					Debug.Log(" Low SIX HIt");
			}

			_FlickObj._Force = _ForceForThisTiming;
			GameManager.mInstance._AngleOfHit = _AngleForThisTIming;
		
		}
	}

	void OnTriggerStay( Collider other)
	{
		if (other.gameObject.tag == "ball") 
		{

		}
	}

	void OnTriggerExit( Collider other)
	{
		if (other.gameObject.tag == "ball")
		{
			if (this.gameObject.name =="TriggerFor6_low")
			{
				other.gameObject.transform.FindChild("glow6").gameObject.SetActive(false);

                
            }

		}
	}
}
