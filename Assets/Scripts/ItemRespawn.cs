using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawn : MonoBehaviour
{
	[Header("Good Items")]
	public GameObject[] good;
	public float respawnGood;
	
	[Header("Bad Items")]
	public GameObject[] bad;
	public float respawnBad;
	
	[Header("Lives Items")]
	public GameObject[] lives;
	public float respawnLives = 5f;
	
	[Header("Configuration")]
	public Transform parent;
		
	private float lastGoodRespawn;
	private float lastBadRespawn;
	private float lastLivesRespawn;
	
	private Vector2 xRange = new Vector2(-7.0f, 7.0f);
	private float yRespawnPosition = 9f;
		
	
    // Start is called before the first frame update
    void Start()
    {
        // InvokeRepeating("CreateGood", 1.5f, respawnGood);
		// InvokeRepeating("CreateBad", 0.5f, respawnBad);
		lastGoodRespawn = Time.time;
		lastBadRespawn = Time.time;
		lastLivesRespawn = Time.time;
    }
	
	void Update()
	{
		// Debug.Log("Time.time " + Time.time);
		
		if(Time.time > lastGoodRespawn + respawnGood)
		{
			CreateObject(good);
			lastGoodRespawn = Time.time;
		}
		if(Time.time > lastBadRespawn + respawnBad)
		{
			// CreateBad();
			CreateObject(bad);
			lastBadRespawn = Time.time;
		}
		if(Time.time > lastLivesRespawn + respawnLives)
		{
			// CreateLives();
			CreateObject(lives);
			lastLivesRespawn = Time.time;
		}
	}	
	
	// void CreateGood()
	// {		
		// int index = Random.Range(0, good.Length);
		// if(good[index])
		// {
			// CreateObject(good[index]);
		// }
	// }
	// void CreateBad()
	// {		
		// int index = Random.Range(0, bad.Length);
		// if(bad[index])
		// {
			// CreateObject(bad[index]);
		// }
	// }
	// void CreateLives()
	// {		
		// int index = Random.Range(0, lives.Length);
		// if(lives[index])
		// {
			// CreateObject(lives[index]);
		// }
	// }
	void CreateObject(GameObject[] array)
	{
		int index = Random.Range(0, array.Length);
		if(array[index])
		{
			GameObject newObject;
			if(parent)
			{
				newObject = Instantiate(array[index], new Vector3(Random.Range(xRange.x, xRange.y), yRespawnPosition, 0), Quaternion.identity, parent);
			}
			else
			{
				newObject = Instantiate(array[index], new Vector3(Random.Range(xRange.x, xRange.y), yRespawnPosition, 0), Quaternion.identity);
			}
			newObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), 0));
		}		
	}
	
	public void IncreaseBadRespawnTime(float toAdd)
	{
		if(respawnBad + toAdd >= 0.15f)
		{
			respawnBad += toAdd;
		}
	}
}
