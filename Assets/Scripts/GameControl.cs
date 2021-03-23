using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
	[Header("UI Elements")]
	public Text scoreText;	
	public int score = 0;
	public Text livesText;
	public int lives = 3;
	public GameOverPanel gameOverPanel;
	
	[Header("Objects")]
	public Transform itemsParent;
	public Transform playerParent;
	public Transform gridParent;
	public GameObject player;
	public GameObject grid;
	private GameObject playerInstance;
	
	private LevelController currentLevel;
	
    void Start()
    {
		PauseGame();
		// CreatePlayer();
        InvokeRepeating("AddPointsByTime", 1f, 1f);
		
		GameObject level = Instantiate(grid, gridParent);
		currentLevel = level.GetComponent<LevelController>();
		Restart();
    }	
	
	void AddPointsByTime()
	{
		AddPoints(10);
	}
	
	public void Restart()
	{
		score = 0;
		AddLives(10);
		AddPoints(0);
		foreach (Transform child in itemsParent) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in playerParent) {
			GameObject.Destroy(child.gameObject);
		}
		CreatePlayer();
		// playerInstance.transform.localPosition = new Vector3(0f,0f,0f);
		gameOverPanel.SetActive(false);
		ResumeGame();
	}
	
	public void AddPoints(int points)
	{
		score += points;
		score = (score < 0) ? 0 : score;
		scoreText.text = "Score: " + score.ToString();
	}
	
	public bool AddLives(int life)
	{
		lives += life;
		lives = (lives > 3) ? 3 : lives;		
		livesText.text = "Lives: " + lives.ToString();
		if(lives <= 0)
		{
			GameOver();
			return false;
		}
		return true;
	}
	
	public void PauseGame ()
    {
        Time.timeScale = 0;
    }

	public void ResumeGame ()
    {
        Time.timeScale = 1;
    }
	
	public void GameOver()
	{
		PauseGame();		
		gameOverPanel.OpenPanel(score);
	}
	
	void CreatePlayer()
	{
		playerInstance = Instantiate(player, playerParent);
		playerInstance.transform.position = currentLevel.startPosition.position;
	}
	
	public void MovePlayer(int movement)
	{
		playerInstance.GetComponent<PlayerMovement>().MoveByUI(movement);
	}
}
