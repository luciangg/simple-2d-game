using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
	[Header("UI Elements")]
	public Values uiValues;	
	public int score = 0;
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
	private ItemRespawn itemRespawn;
	
	struct ScoreData {
		public float lastTime;
		public float timeToNewPoint;
		public float lastDifficultyIncrease;
		public int points;
		
		public ScoreData(float lastTime, float lastDifficultyIncrease, float timeToNewPoint, int points)
		{
			this.lastTime = lastTime;
			this.timeToNewPoint = timeToNewPoint;
			this.lastDifficultyIncrease = lastDifficultyIncrease;
			this.points = points;
		}
	}
	private ScoreData scoreData; 
	
	void Awake () {
		#if UNITY_EDITOR
			QualitySettings.vSyncCount = 0;  // VSync must be disabled
			Application.targetFrameRate = 45;
		#endif
	}
	
    void Start()
    {
		scoreData = new ScoreData(Time.time, Time.time, 1f, 10);
		
		PauseGame();
		// CreatePlayer();
        // InvokeRepeating("AddPointsByTime", 1f, 1f);
		
		GameObject level = Instantiate(grid, gridParent);
		currentLevel = level.GetComponent<LevelController>();
		itemRespawn = gameObject.GetComponent<ItemRespawn>();
		Restart();
    }
	void Update()
	{
		if(Time.time > scoreData.lastTime + scoreData.timeToNewPoint)
		{
			scoreData.lastTime = Time.time;
			AddPoints(scoreData.points);
		}
		if(Time.time > scoreData.lastDifficultyIncrease + 15f)
		{
			scoreData.lastDifficultyIncrease = Time.time;
			scoreData.points += 5;
			itemRespawn.IncreaseBadRespawnTime(-0.01f);			
		}			
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
		// scoreText.text = "Score: " + score.ToString();
		uiValues.setScore(score);
	}
	
	public bool AddLives(int life)
	{
		lives += life;
		lives = (lives > 3) ? 3 : lives;		
		// livesText.text = "Lives: " + lives.ToString();
		uiValues.setLives(lives);
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
