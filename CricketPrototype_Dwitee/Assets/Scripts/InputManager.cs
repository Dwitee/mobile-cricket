using UnityEngine;
using System.Collections;

/// <summary>
/// Input manager. To handle editor as well as device
/// </summary>
public class InputManager : MonoBehaviour 
{
	public enum InputState { POINTER_DOWN, POINTER_MOVING, POINTER_UP, WAITING_FOR_INPUT };		//The possible states of mouse/touch
	
	public bool useTouch = true;                                   								//Use touch controls
	
	public static InputManager myInstance;                         							 	//Hold a reference to this script
	
	private InputState inputState  = InputState.WAITING_FOR_INPUT;     							//The state of the input
	public InputState pInputState 
	{
		get
		{
			return inputState;
		}
		set
		{
			inputState = value;
		}
	}
	
	private Vector3 inputWorldPos;
	public Vector3 pInputWorldPos { get { return inputWorldPos; } }
	private Vector3 inputScreenPos;
	public Vector3 pInputScreenPos { get { return inputScreenPos; } }
	private Vector3 inputViewPort;
	public Vector3 pInputViewPort { get { return inputViewPort; } }

	//Returns the instance 
	public static InputManager Instance { get { return myInstance; } }
	
	//Called at the start of the level
	void Start()
	{
		myInstance = this;
	}
	
	
	//Called at every frame, tracks input
	void Update()
	{
		useTouch = Utility.IsMobilePlatform();
		
		if (useTouch)
			TouchControls();
		else
			MouseControls(); 
	}
	
	//Mouse controls
	void MouseControls()
	{
		//Get the position of the input
		inputWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		inputScreenPos = Input.mousePosition;
		inputViewPort = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		
		if (Input.GetMouseButtonDown(0))
		{
			inputState = InputState.POINTER_DOWN;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			inputState = InputState.POINTER_UP;
		}
	}

	//Touch controls for devices
	void TouchControls()
	{
		foreach (Touch touch in Input.touches)
		{
			//Get the position of the input
			inputWorldPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
			inputScreenPos = Input.touches[0].position;
			
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				inputState = InputState.POINTER_UP;
			}
			else if (touch.phase == TouchPhase.Began)
			{
				inputState = InputState.POINTER_DOWN;
			}
			
		}
	}
}
