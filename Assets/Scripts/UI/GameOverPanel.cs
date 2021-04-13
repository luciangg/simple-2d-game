using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
	public Text score;
	public Text record;	
	private PlayFabScore login;
	
	public void OpenPanel(int currentScore)
	{
		GetLogin().AddScore(currentScore);
		if(PlayerPrefs.GetFloat("Record", 0) < currentScore)
		{
			PlayerPrefs.SetFloat("Record", currentScore);
		}
		score.text = currentScore.ToString();
		record.text = PlayerPrefs.GetFloat("Record", 0).ToString();
		gameObject.SetActive(true);
	}
	
	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}
	
	private PlayFabScore GetLogin()
	{
		if(!login)
		{
			login = GameObject.FindGameObjectWithTag("PlayFabControl").GetComponent<PlayFabScore>();
		}
		return login;
	}
}
