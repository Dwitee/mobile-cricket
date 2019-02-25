using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flick : MonoBase 
{
	// these are in percentage of screen space
	public float _MinDistBeforeRecording = 0.05f;
	public float _AngleDivisor = 0.5f;
	public MinMax _Force = new MinMax(1,10);
	public MinMax _ForceLength = new MinMax(0.1f, 0.5f);
	
	//trail of swipe
	public bool _AllowInput = true;
	public bool _DebugDraw = false;
	public bool _ShowTrail = true;
	public Transform _TrailParticle = null;
	public float _ParticleDistance = 5f;

	// touch
	protected float mStartTime = 0.0f;
	protected List<Vector3> mPositions = new List<Vector3>();
	protected int mMaxPositions = 2;	//  we only need first and last
	private bool isMousePressed;
	private List<Vector3> pointsList;
	private LineRenderer line;
	private Vector3 mousePos;

	// externals
	public static float mFlickForce;
	public static Vector3 mFlickVector;
	
	protected float mFlickTime;
	

	
	public float pFlickForce
	{
		get { return mFlickForce; }
	}
	
	public Vector3 pFlickVector
	{
		get {return mFlickVector; }
	}
	
	public float pFlickTime
	{
		get {return mFlickTime; }
	}
	

	public virtual void Awake()
	{
		isMousePressed = false;
		pointsList = new List<Vector3>();
		line = this.gameObject.GetComponent<LineRenderer> ();
		if (line != null)
			SetLineRenderer ();
	}

	void SetLineRenderer()
	{
		line.enabled = true;
		line.SetVertexCount (0);
		line.SetWidth (1.0f, 1.0f);
	}

	public virtual void Update () 
	{
			
		Vector3 curPos;

		// If mouse button down or touch down, remove old curve
		if (isMousePressed ==false && InputManager.myInstance.pInputState == InputManager.InputState.POINTER_DOWN)
		{
			curPos = InputManager.myInstance.pInputScreenPos;
			//Reset flick data here this if first touch
			isMousePressed = true;
	
			
			if (mPositions.Count == 0)
				mPositions.Add(curPos );

			mStartTime = Time.time;
			_ShowTrail = false;
            
        }
		
		if ( isMousePressed == true && InputManager.myInstance.pInputState == InputManager.InputState.POINTER_UP)
		{
			curPos = InputManager.myInstance.pInputScreenPos;


			//this is after the swipe the finger touch up
			isMousePressed = false;
			
			FinalizeFlick();
			_ShowTrail = false;
		}

	
        
        // Drawing line when mouse is moving(presses)
		if (isMousePressed) 
		{
			curPos = InputManager.myInstance.pInputScreenPos;

			if (mPositions.Count < mMaxPositions)
				mPositions.Add(curPos);
			else
				mPositions[mPositions.Count-1] = curPos;
			
			UpdateFlickForce();

			// draw trail renderer of swipe
			_ShowTrail = true;
			DrawSWipeTrail();
			
		}


	}

	private void DrawSWipeTrail()
	{
		if (_ShowTrail && GameState.IsState(GameState.States.PLAYING))
		{
			if(_TrailParticle != null )
				_TrailParticle.position = Camera.main.ScreenToWorldPoint(new Vector3(InputManager.myInstance.pInputScreenPos.x, InputManager.myInstance.pInputScreenPos.y, _ParticleDistance));
			
			if(_DebugDraw && mPositions.Count > 0)
			{
	            DrawCurve();
	        }
	    }
	}
    
    private void DrawCurve()
	{
		Debug.Log(" Drawing Curve");
		mousePos  = InputManager.myInstance.pInputScreenPos;
		mousePos.z = 100;
		if (!pointsList.Contains (mousePos) )
		{

			pointsList.Add (mousePos);
			line.SetVertexCount (pointsList.Count);
			line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);

		}
	}


	private void ResetCanvas()
	{
		line.SetVertexCount (0);
		pointsList.RemoveRange (0, pointsList.Count);
		line.SetColors(Color.white, Color.grey);
		
	}


    void UpdateFlickForce()
	{
		if (mPositions.Count > 0)
		{
			Vector3 first = mPositions[0];
			Vector3 last = mPositions[mPositions.Count - 1];
			mFlickForce = CalculateForce(first, last);
			
			mFlickVector = (last-first);
			mFlickVector.x *= _AngleDivisor;
			mFlickVector.Normalize();
		}
		else
		{
			mFlickForce = 0.0f;
			mFlickVector = Vector3.zero;
		}
		
	}


	void FinalizeFlick()
	{
		if (mPositions.Count > 0 )
		{
			mFlickTime = Time.time - mStartTime;
			OnFlick();
		}
		
		mPositions.Clear();
	}


	public virtual float CalculateForce(Vector3 first, Vector3 last)
	{
		// we want to scale 0 to 1, between the LengthForMinForce and LengthForMaxForce
		float length = PSS((last-first).magnitude);
		length = Mathf.Clamp(length, _ForceLength.Min, _ForceLength.Max);					// truncate between range
		length = (length - _ForceLength.Min) / (_ForceLength.Max - _ForceLength.Min);		// scale back to 0 - 1
		return Mathf.Lerp(_Force.Min, _Force.Max, length);
	}
	

	public virtual void OnFlick()
	{
	}


	// returns a 0-1 value representing the percentage of screen space that inValue is (inValue is screen coordinates)
	public float PSS(float inValue)
	{
		return Mathf.Clamp(inValue / (float)Screen.height, 0.0f, 1.0f);
	}

}