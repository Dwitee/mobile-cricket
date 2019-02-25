using UnityEngine;
using System.Collections;

public class CricketBall : Ball 
{
	
	private Vector3 mInitialforce;
	
	private bool mIsDestroyCalled = false;

	protected override void Awake()
	{
		base.Awake();	
		_ForceModeLaunch = ForceMode.Impulse;
	}
	
	protected override void FixedUpdate()
	{
		//base.FixedUpdate();
		if(rigidbody.useGravity && !rigidbody.IsSleeping())
		{
			//add force to rigid body if you want to wind
		}
		
	}

	
	public void AddTorque(Vector3 inDirection, float force)
	{
		rigidbody.useGravity = true;
		rigidbody.AddTorque(inDirection * force, ForceMode.Impulse);
	}
	
	public override void ResetValues()
	{
		base.ResetValues();
	}
	
	private void OnCollisionEnter(Collision col)
	{
		if(rigidbody.useGravity)
		{

			if(!mIsDestroyCalled && _BallLaunched)
			{
				mIsDestroyCalled = true;
			}
		}
	}
	
	private void OnTriggerEnter(Collider inCollider)
	{
		if(!mIsDestroyCalled && _BallLaunched)
		{
			mIsDestroyCalled = true;
		}
	}

}

