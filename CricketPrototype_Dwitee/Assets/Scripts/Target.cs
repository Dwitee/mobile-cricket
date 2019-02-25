using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	public int _RunAwarded = 6;
	public float _ExtraTime = 5.0f;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnCollisionEnter( Collision col)
	{
		if (col.gameObject.tag == "ball")
		{
			OnTargetHit ();
			col.gameObject.GetComponent<SphereCollider>().enabled = false;
				 
		}
	}

	private void OnTargetHit()
	{

		ScoreManager.GetInstance ().AddToScore (_RunAwarded);
		HUDManager.GetInstance().DisplayBonusScoreOnScreen(this.gameObject.name);
		gameObject.GetComponentInChildren<ParticleSystem> ().Play ();

		if ( this.gameObject.name == "SmallSign3Collider")
		{
			GameManager.mInstance.mGameTimer += _ExtraTime;
            
            StartCoroutine(HideForSeconds(10.0f));

		}
		else
		{
			this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
			this.gameObject.GetComponent<SphereCollider>().enabled = false;		
		}
	}

	IEnumerator HideForSeconds( float time)
	{
		this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
		this.gameObject.GetComponent<SphereCollider>().enabled = false;
        
        yield return new WaitForSeconds(time);
		this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        this.gameObject.GetComponent<SphereCollider>().enabled = true;   
    }
}
