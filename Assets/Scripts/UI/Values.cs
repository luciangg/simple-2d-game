using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;


public class Values : MonoBehaviour
{
	public int score = 0;
	public int lives = 3; 
	private LocalizeStringEvent[] strings;
	
	void Start()
	{
		strings = gameObject.GetComponents<LocalizeStringEvent>();
	}
	
	public void setScore(int score)
	{
		this.score = score;
		//stringScore.StringReference.RefreshString();
		updateStrings();
	}
	public void setLives(int lives)
	{
		this.lives = lives;
		updateStrings();
	}
	void updateStrings()
	{
		foreach(LocalizeStringEvent stringEvent in strings)
		{
			stringEvent.StringReference.RefreshString();
		}
	}
}
