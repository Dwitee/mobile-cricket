using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour 
{
	public static GameManager mInstance = null;

	//particle effect
	public ParticleSystem mFanfair = null;

	//timers
	public float _GameTime = 40;
	protected bool mGamePaused = false;
	public float mGameTimer = 0;
	public bool mGameTimerActive = false;
	public float _WarningTime = 10;
	public Text TimerCountDown = null;
	public Canvas HUD = null;

	//ball
	public GameObject _ball = null;
	[HideInInspector] public CricketBall _ballScript = null;
	protected Rigidbody ballRigidbody = null;
	public Vector3 bowlingPosition = new Vector3(0,5,0);
	public MinMax bowlingForce =  new MinMax(10,12);
	public Vector3 bowlingDirection = new Vector3 (0, 0, -1);
	public float bolwingInterval = 3.0f;
	public GameObject _ballOnPitch = null;
	protected List<GameObject> mObjectPool = null;
	public Vector3 airDrag  = new Vector3();
	public MinMax _AirDragForce = new MinMax(0.1f, 20.0f);
	public float spin = 10.0f;

	//flicks
	public HitShot _FlickObj = null;
	public float _AngleOfHit = 60.0f;

	//sign post
	public GameObject _SignPosts = null;

	//game states
	protected bool mResultsActive = false;
	[HideInInspector]
	private bool mIsGameRunning = false; 
	
	public bool pIsGameRunning
	{
		get { return mIsGameRunning; }
		set { mIsGameRunning = value; }
	}


	// Use this for initialization
	void Start () 
	{
		GameState.state = GameState.States.IDLE;
		ballRigidbody = _ball.GetComponent<Rigidbody>();
		_ballScript = _ball.GetComponent<CricketBall> ();

		bowlingDirection.Normalize();
		bowlingDirection *=  Random.Range( bowlingForce.Min,bowlingForce.Max);
		mGameTimer = _GameTime;
		mObjectPool = new List<GameObject> ();
	}

	protected virtual void Awake()
	{
		mInstance = this;
		if (HUD == null)
			Debug.LogError ("canvas reference is empty in gameManager");
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTimerinHUD ();
		UpdateScore ();
		if (mGameTimerActive && !mGamePaused)
		{
		
			mGameTimer -= Time.deltaTime;
			OnTimerUpdate();
			if (mGameTimer <= 0.0f)
			{
				mGameTimer = 0.0f;
				OnTimerExpired();
				mGameTimerActive = false;
			}
		}
		

	}

	/// <summary>
	/// Updates the score.
	/// </summary>
	public void UpdateScore()
	{
		HUDManager.GetInstance().UpdateText("Runs/Text", ScoreManager.GetInstance ().GetCurrentScore ().ToString());
	}

	/// <summary>
	/// Launchs the ball.
	/// </summary>
	public void LaunchBall()
	{
		if( _ballOnPitch != null)
			_ballScript = _ballOnPitch.GetComponent<CricketBall> ();

		if(_ballScript != null && pIsGameRunning)
		{
			_ballScript._BallLaunched = true;

			_FlickObj._AllowInput = false;
					
			// inDirection becomes the X/Z force of the ball. We also then add an upwards force
			Vector3 flickDir = _FlickObj.pFlickVector;
			if (flickDir.y < 0.0f)		// temporary, to be capped between angle limits
				flickDir.y = -flickDir.y;
			Vector3 kickDir = new Vector3(_FlickObj.pFlickVector.x, Mathf.Sin(_AngleOfHit * Mathf.Deg2Rad), _FlickObj.pFlickVector.y).normalized;
			// kick force needs to be moved into camera space
			kickDir = Camera.main.transform.TransformDirection(kickDir);
			
			_ballScript.Launch(kickDir, _FlickObj.pFlickForce);
			_ballScript.AddTorque(_ballScript.transform.right, _FlickObj.pFlickForce);

			int score = ScoreManager.GetInstance().CalculateScore(_AngleOfHit,_FlickObj.pFlickForce);
			ScoreManager.GetInstance ().AddToScore (score);
			HUDManager.GetInstance().DisplayScoreOnScreen(score,"Score");
			//DisplayScoreOnScreen(4);

		}
		else
			Debug.LogError(" Theres no ball on the pitch  wait for the ball");
	}

	void UpdateTimerinHUD()
	{
		HUDManager.GetInstance().UpdateText("Timer/Text",((int)mGameTimer).ToString ());
		//TimerCountDown.text = ((int)mGameTimer).ToString ();
	}

	

	/// <summary>
	/// Event handler for the time up event.
	/// </summary>
	protected virtual void OnTimerExpired()
	{
		mIsGameRunning = false;

		OnGameOver();
	}
	/// <summary>
	/// Event handler for the game over event.
	/// </summary>
	protected virtual void OnGameOver()
	{
		CancelInvoke("SpawnBall");

		ballRigidbody.isKinematic = true;
		StartCoroutine("ShowResultScreen");
	}

	private IEnumerator ShowResultScreen()

	{
		yield return new WaitForSeconds(3);
		HUD.transform.FindChild ("GameOver").gameObject.SetActive (true);
		GameState.state = GameState.States.RESULTSCREEN;
		
		if ( ScoreManager.GetInstance ().GetCurrentScore () > ScoreManager.GetInstance().GetBestScore())
			ScoreManager.GetInstance ().SetBestScore (ScoreManager.GetInstance ().GetCurrentScore ());
		
		string gameOverTxt  = "Your Score is " + ScoreManager.GetInstance ().GetCurrentScore ().ToString () +" \n"	+ "Your best score is " + ScoreManager.GetInstance ().GetBestScore ().ToString ();
		HUDManager.GetInstance().UpdateText("GameOver/Panel/TextResult",gameOverTxt);
	}

	/// <summary>
	/// Raises the timer update event.
	/// </summary>
	protected virtual void OnTimerUpdate()
	{
		if(mGameTimer < _WarningTime)
		{
			Debug.Log(" Show warning that timer is about finish");
			TimerCountDown.color = Color.red;
		}
		else
		{
			TimerCountDown.color = Color.white;
            
        }
	}

	/// <summary>
	/// Restarts the game.
	/// </summary>
	public virtual void ResetGame()
	{
		ScoreManager.GetInstance ().ResetScore ();
		// if game is running cancel invoke
		if ( GameState.IsState(GameState.States.COUNTDOWN) ||  GameState.IsState(GameState.States.PLAYING) || IsInvoking("SpawnBall") )
			CancelInvoke("SpawnBall");
		StopAllCoroutines ();
		DestroyAllBalls ();
		HUDManager.GetInstance().UpdateText("GameOver/Panel/TextResult","");

		//reset all sign posts
		Destroy( _SignPosts);
		_SignPosts =  Instantiate(Resources.Load("SignPosts")) as GameObject;        

	}
	/// <summary>
	/// Initial Setup for the game
	/// </summary>
	public virtual void StartGame()
	{ 
		ResetGame ();

		InitializeGameData();
		StartCountdown();
		mResultsActive = false;
		SetTimer(_GameTime);
		StartTimer(false);
		mGamePaused = false;
		TimerCountDown.color = Color.white;
		HUD.transform.FindChild ("GameOver").gameObject.SetActive (false);

	}
	/// <summary>
	/// Initializes the game data.
	/// </summary>
	protected virtual void InitializeGameData()
	{
		// Do basic init here

	}

	public virtual void StartCountdown()
	{
		GameState.state = GameState.States.COUNTDOWN;
		StartCoroutine ("PrintCountDown");

	}

	IEnumerator PrintCountDown() 
	{
		HUDManager.GetInstance().FlashText("CountDown","3");
		yield return new WaitForSeconds(1);
		HUDManager.GetInstance().FlashText("CountDown","2");
		yield return new WaitForSeconds(1);
		HUDManager.GetInstance().FlashText("CountDown","1");
		yield return new WaitForSeconds(1);

		HUDManager.GetInstance().FlashText("Ready","Ready Go");
		yield return new WaitForSeconds(1);
		OnCountdownComplete ();
	}
	public virtual void OnCountdownComplete()
	{
		GameState.state = GameState.States.PLAYING;

		pIsGameRunning = true;
		StartTimer(true);
		ballRigidbody.isKinematic = false;
		ballRigidbody.useGravity = true;
		InvokeRepeating ("SpawnBall", 0, bolwingInterval);
	}


	public void SetTimer(float time)
	{
		if (time >= 0.0f)
			mGameTimer = time;
		else
			mGameTimer = _GameTime;
	}
	
	public float GetTimer()
	{
		return mGameTimer;
	}
	
	public void StartTimer(bool bActive)
	{
		mGameTimerActive = bActive;
	}

	public void SpawnBall()
	{ 
		if ( mIsGameRunning ==true)
		{
			_ball.GetComponent<Rigidbody>().useGravity = true;
			_ball.GetComponent<Rigidbody>().isKinematic = false;
			GameObject ballClone = (GameObject) Instantiate (_ball, bowlingPosition, Quaternion.identity);
			ballClone.SetActive(true);
			ballClone.GetComponent<Rigidbody>().AddForce(bowlingDirection, ForceMode.Impulse);

			airDrag = GetRandVectorInXYPlain() * Random.Range(_AirDragForce.Min, _AirDragForce.Max);
			ballClone.GetComponent<Rigidbody>().AddForce(airDrag,ForceMode.Force);
			ballClone.GetComponent<Rigidbody>().AddTorque(Vector3.left * spin, ForceMode.Impulse);
			//Add to object pool
			mObjectPool.Add(ballClone);
		}	


	}

	Vector3 GetRandVectorInXYPlain()
	{
		Vector3 airDrag = new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f));
		airDrag.Normalize();
		return airDrag;
	}

	void DestroyAllBalls()
	{
		if (mObjectPool != null) 
		{
			for (int i = 0; i < mObjectPool.Count; i++) 
			{
				Destroy (mObjectPool [i]);
			}
		}
	}
	void OnDestroy()
	{
		DestroyAllBalls ();

	}

}
