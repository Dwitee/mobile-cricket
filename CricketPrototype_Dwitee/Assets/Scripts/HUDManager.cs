using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	private static HUDManager mInstance = null;

	public static HUDManager GetInstance()
	{
		return mInstance;
	}

	protected void Awake()
	{
		mInstance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayScoreOnScreen(int score, string ScoreType)
	{	
		if (transform.FindChild (ScoreType) == null)
			return;

		transform.FindChild (ScoreType).gameObject.SetActive (true);

		if (score > 0)
		{
			transform.FindChild (ScoreType).gameObject.GetComponent<Text> ().text = "+" + score.ToString ();
		}
		else
		{
			transform.FindChild (ScoreType).gameObject.GetComponent<Text> ().text = "MISS!";
			
		}
		StartCoroutine (HideHudText (ScoreType, 1));
		
	}

	
	public void DisplayBonusScoreOnScreen(string ScoreType)
	{	
		if (transform.FindChild (ScoreType) == null)
			return;
		
		transform.FindChild (ScoreType).gameObject.SetActive (true);
		StartCoroutine (HideHudText (ScoreType, 1));
		
	}

	
	IEnumerator HideHudText(string text, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		transform.FindChild (text).gameObject.SetActive (false);
		//transform.FindChild (text).gameObject.GetComponent<Text>().text = "";
		
	}
	public void UpdateText( string textType, string txt)
	{
		if (transform.FindChild (textType) == null)
			return;
		
		transform.FindChild (textType).gameObject.SetActive (true);
		transform.FindChild (textType).gameObject.GetComponent<Text> ().text =  txt;

	}
	public void FlashText( string displayType, string text)
	{
		if (transform.FindChild (displayType) == null)
			return;
		
		transform.FindChild (displayType).gameObject.SetActive (true);
		transform.FindChild (displayType).gameObject.GetComponent<Text>().text = text;
		StartCoroutine (HideHudText (displayType, 1));
	}
}
