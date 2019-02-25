using UnityEngine;
using System.Collections;

/// <summary>
/// Ball.
/// This a base class for all ball physics related stuff. The ball physics is affected by wind. 
/// This class should handle only generic stuff. Derive from this class to have game specific functionalities.
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBase 
{
	public ForceMode _ForceModeLaunch = ForceMode.Acceleration;
	public ForceMode _ForceModeWind = ForceMode.Acceleration;
	
	public AudioClip _FlyingSFX = null;
	
	protected TrailRenderer mTrailRenderer = null;

	public bool _BallLaunched = false;
	
	public TrailRenderer pTrailRenderer 
	{
		get { return mTrailRenderer; }
	}
	
	protected virtual void Awake()
	{
		mTrailRenderer = transform.GetComponentInChildren<TrailRenderer>();

	}
	
	protected virtual void Start () 
	{
		
	}
	
	protected virtual void Update () 
	{
	}
	
	protected virtual void FixedUpdate()
	{
		if(rigidbody.useGravity && !rigidbody.IsSleeping())
		{
			//Vector3 force = ObWind.pForce; 
			//rigidbody.AddForce(force, _ForceModeWind);
		}
	}
	
	//Launch the ball in the given direction with the given force.
	public virtual void Launch(Vector3 inDirection, float force)
	{
		if(mTrailRenderer != null)
			mTrailRenderer.enabled = true;
		
		//EnableRotation(true);
		enabled = true;
		rigidbody.useGravity = true;
		_BallLaunched = true;
		rigidbody.AddForce(inDirection * force, _ForceModeLaunch);
	}
	
	//Reset all values & forces rigidbody to sleep. 
	public virtual void ResetValues()
	{
		if(mTrailRenderer != null)
			mTrailRenderer.enabled = false;
		//EnableRotation(false);
		enabled = false;
		_BallLaunched = false;
		rigidbody.useGravity = false;
		rigidbody.Sleep();
	
	}
	
//	//Use this when you want to rotate the ball on kicking/flick it
//	public virtual void EnableRotation(bool isEnabled)
//	{
//		if(mObRotate != null)
//			mObRotate.enabled = isEnabled;
//	}

	void OnDestroy()
	{

		Debug.Log (" The ball is destroyed");
	}
	

}

