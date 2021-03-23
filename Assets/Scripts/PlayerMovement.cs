using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	private Animator animator = null;
	public CharacterController2D controller;
	public GameControl gameControl;
	public float runSpeed = 40f;
	public GameObject star;
	public GameObject health;
	public GameObject hit;
	float horizontalMove = 0f;
	float uiMove = 0f;
	bool jump = false;
	bool cooldown = false;
	
	void Start()
	{
		animator = GetComponent<Animator>();
		gameControl = GameObject.FindWithTag("GameControl").GetComponent<GameControl>();
		// LeftButton.onClick.AddListener(MoveByUI);
	}
	
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
		horizontalMove = (horizontalMove != 0) ? horizontalMove : uiMove;
		horizontalMove *= runSpeed;
		
		if(Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
		if(cooldown)
		{
			jump = false;
			horizontalMove = 0;
		}
		
		animator.SetInteger("walk", (int)horizontalMove);
    }
	
	void FixedUpdate()
    {
		controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		
		jump = false;
    }
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.CompareTag("GoodItem"))
		{
			gameControl.AddPoints(15); 
			Instantiate(star, gameObject.transform.position, Quaternion.identity);
			Destroy(collider.gameObject);
		}
		else if(collider.gameObject.CompareTag("BadItem") && !cooldown)
		{			
			gameControl.AddPoints(-10);
			if(gameControl.AddLives(-1))
			{
				Instantiate(hit, gameObject.transform.position, Quaternion.identity);
				cooldown = true;			
				Destroy(collider.gameObject);
				animator.SetBool("hit", true);
				StartCoroutine(HitToNormal());
			}
		}
		else if(collider.gameObject.CompareTag("LifeItem"))
		{			
			gameControl.AddLives(1);	
			Instantiate(health, gameObject.transform.position, Quaternion.identity);
			Destroy(collider.gameObject);
		}
	}
	
	IEnumerator HitToNormal()
	{
		yield return new WaitForSeconds(2);
		cooldown = false;
		animator.SetBool("hit", false);
	}
	
	public void MoveByUI(int movement)
	{
		Debug.Log("MOVE");
		uiMove = movement;
	}
}
