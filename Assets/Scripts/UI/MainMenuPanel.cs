using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
	public Text record;	
	
	public void Start()
	{
		OpenPanel();
	}
	
	public void OpenPanel()
	{
		record.text = PlayerPrefs.GetFloat("Record", 0).ToString();
		gameObject.SetActive(true);
	}
	
	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}
}
