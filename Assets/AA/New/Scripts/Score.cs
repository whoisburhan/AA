using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Score : MonoBehaviour {


	public static int PinCount;

	public Text text;

	void Start ()
	{
		PinCount = 0;
	}

	void Update ()
	{
		text.text = PinCount.ToString();
	}

	public void UpdateScore(int score)
	{
		text.text = score.ToString();
	}

}
