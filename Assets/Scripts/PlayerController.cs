using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private Animator animator = null;
	public Text scoreText;
	public float speed = 10f;
	public int score = 0;
	public bool cooldown = false;
		
	void Start()
	{
		animator = GetComponent<Animator>();
		InvokeRepeating("AddPoints", 1f, 1f);
	}

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
		int movement = 0;
		
		if(horizontal != 0)
		{
			movement = (horizontal > 0) ? 1 : -1;
		}
		
		animator.SetInteger("walk", movement);	
		
		Vector3 newScale = transform.localScale;
		
		newScale.x =  (horizontal >= 0) ? 1 : -1;
		// transform.localScale = newScale;
		// transform.Translate(horizontal * speed * Time.deltaTime, 0f, 0f);
		
		scoreText.text = score.ToString();
    }
	
	void _OnTriggerEnter2D(Collider2D col)
	{
		if(!cooldown)
		{
			animator.SetBool("hit", true);
			score -= 100; 
			cooldown = true;
			StartCoroutine(HitToNormal());		
		}
	}
	
	IEnumerator HitToNormal()
	{
		yield return new WaitForSeconds(2);
		cooldown = false;
		animator.SetBool("hit", false);
	}
	
	void AddPoints()
	{
		score += 10;
	}	
}
