using UnityEngine;
using System.Collections;

public class MonoBase : MonoBehaviour 
{
	private Transform mTransform = null;
	private GameObject mGameObject = null;
	private Rigidbody mRigidBody = null;
	private Collider mCollider = null;
	private Renderer mRenderer = null;
	
	public new Transform transform
	{
		get
		{
			if(mTransform == null)
				mTransform = base.transform;
			return mTransform;
		}
	}
	
	public new GameObject gameObject
	{
		get
		{
			if(mGameObject == null)
				mGameObject = base.gameObject;
			return mGameObject;
		}
	}
	
	public new Rigidbody rigidbody
	{
		get
		{
			if(mRigidBody == null)
				mRigidBody = base.rigidbody;
			return mRigidBody;
		}
	}
	
	public new Collider collider
	{
		get
		{
			if(mCollider == null)
				mCollider = base.collider;
			return mCollider;
		}
	}
	
	public new Renderer renderer 
	{
		get 
		{
			if(mRenderer == null)
				mRenderer = base.renderer;
			return mRenderer;
		}
	}
}