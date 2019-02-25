using System;
using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[Serializable]
public class MinMax
{
	[XmlElement(ElementName = "Min")]
	public float Min;
	
	[XmlElement(ElementName = "Max")]
	public float Max;
	
	public MinMax()
	{
	}
	
	public MinMax(float inMin, float inMax)
	{
		Max = inMax;
		Min = inMin;
	}
	
	public float GetRandomValue() { return UnityEngine.Random.Range(Min * 1000.0f, Max * 1000.0f) / 1000.0f; }
	
	public bool IsInRange(float inValue) 
	{ 
		if(inValue < Min || inValue > Max)
			return false;
		return true;
	}
	
	public override string ToString() 
	{ 
		return ("Min : "+ Min + ", Max : " + Max);
	}
}

public class Utility 
{

	public static bool IsMobilePlatform()
	{
		return (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android );
	}
	
}
