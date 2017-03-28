using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public GameObject player;
	public GameObject otherPlayer;
	Rigidbody2D myRB;

	//Jump variables
	public float jumpHeight;
	float jump;
	
	//Checking grounded
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask ground;
	bool grounded;

	//Movement variables
	public float speed;
	private float move;
	private bool faceRight;
	private bool frozen;
	private bool speedBoosted;
	private bool jumpBoosted;
	private float speedTimer;
	private bool lWallJump;
	private bool rWallJump;
	private bool wallFr;
	private bool wallFl;

	private bool zeroGravity;
	private Vector3 groundCheckPos;
	private int speedBoostCtr; 
	private float colliderYOffSet;

	//CheckPoint
	Vector3 lastCheckPoint;
	float lowestY = -47.0f; 
	float highestY = 22.0f;

	Animator myAnim;

	// Use this for initialization
	void Start () {
		lWallJump = false;
		rWallJump = false;
		groundCheckPos = player.gameObject.transform.GetChild (0).localPosition;
		colliderYOffSet = player.GetComponent<BoxCollider2D> ().offset.y;
		speedBoostCtr = 0;
		myRB = player.GetComponent<Rigidbody2D> ();
		lastCheckPoint = player.transform.position;	
		myAnim = player.GetComponent<Animator> ();
		faceRight = true;
		if (player.tag == "player1")
			otherPlayer = GameObject.FindGameObjectWithTag ("player2");
		if (player.tag == "player2")
			otherPlayer = GameObject.FindGameObjectWithTag ("player1");
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), otherPlayer.GetComponent<Collider2D> ());


	}

	void OnTriggerEnter2D(Collider2D other){
		if (player.tag == "player1" && other.tag == "RedFlag") { 
			lastCheckPoint = other.transform.position;
			other.gameObject.SetActive (false);
		}
		if (player.tag == "player2" && other.tag == "BlueFlag" ) {
			lastCheckPoint = other.transform.position;
			other.gameObject.SetActive (false);
		}
		if (other.tag == "speedBoost") {
			speedBoostCtr++;
			speed += 8.0f;
			speedBoosted = true;
			other.gameObject.SetActive (false);
		}
		if (other.tag == "jumpBoost") {
			jumpBoosted = true;
			other.gameObject.SetActive (false);
		}
		if (other.tag == "RightWall") {
			if (faceRight) {
				flip ();
			}
			rWallJump = true;
		}
		if (other.tag == "LeftWall") {
			if (!faceRight) {
				flip ();
			}
			lWallJump = true;
		}
		if (other.tag == "Entrance") {
			zeroGravity = true;
			player.GetComponent<SpriteRenderer> ().flipY = true;
			myRB.gravityScale = -2;
			Vector2 offset = player.GetComponent<BoxCollider2D> ().offset;
			player.GetComponent<BoxCollider2D> ().offset = new Vector2 (offset.x, -colliderYOffSet);
		}
		if (other.tag == "Exit") {
			zeroGravity = false;
			player.GetComponent<SpriteRenderer> ().flipY = false;
			myRB.gravityScale = 2;
			Vector2 offset = player.GetComponent<BoxCollider2D> ().offset;
			player.GetComponent<BoxCollider2D> ().offset = new Vector2 (offset.x, colliderYOffSet);
			player.GetComponent<PlayerShooting> ().setYRange (5.0f);
		}
		if (other.tag == "platform" && zeroGravity) {
			player.gameObject.transform.GetChild (0).localPosition = new Vector3 (groundCheckPos.x, -groundCheckPos.y, groundCheckPos.z);
		}
		if (other.tag == "platform" && !zeroGravity) {
			player.gameObject.transform.GetChild (0).localPosition = new Vector3 (groundCheckPos.x, groundCheckPos.y, groundCheckPos.z);
		}

		if (other.tag == "CheckPoint1") {
			lowestY = -26.0f;
		}
		if (other.tag == "Zone3Gate") {
			lastCheckPoint = GameObject.FindGameObjectWithTag ("CheckPoint2").transform.position;
		}
		if (other.tag == "respawn") {
			Vector3 temp = new Vector3 (lastCheckPoint.x, lastCheckPoint.y + 1, lastCheckPoint.z);
			player.transform.position = temp;
			if (zeroGravity) {
				zeroGravity = false;
				myRB.velocity = new Vector2 (0f, 0f);
				player.GetComponent<SpriteRenderer> ().flipY = false;
				myRB.gravityScale = 2;
				Vector2 offset = player.GetComponent<BoxCollider2D> ().offset;
				player.GetComponent<BoxCollider2D> ().offset = new Vector2 (offset.x, colliderYOffSet);
				player.GetComponent<PlayerShooting> ().setYRange (5.0f);
			}
			myRB.velocity = new Vector2 (0f, 0f);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "RightWall" && faceRight == true) {
			faceRight = true;
		}
		if (other.tag == "LeftWall" && faceRight == false) {
			faceRight = false;
		}
		if (other.tag == "LeftWall" || other.tag == "RightWall") {
			rWallJump = false;
			lWallJump = false;
		}
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, ground);
		if (grounded) {
			myRB.velocity = new Vector2(myRB.velocity.x, 0);
		}
		if (speedBoosted) {
			speedTimer += Time.deltaTime;
			player.GetComponent<SpriteRenderer> ().color = Color.Lerp (Color.green, Color.white, Mathf.PingPong(Time.time, 0.5f));
		}
		if (jumpBoosted) {
			myRB.velocity = new Vector2(myRB.velocity.x, 0);
			myRB.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
			jumpBoosted = false;
		}

		if (speedTimer >= 2.0f) {
			speed -= 8.0f * speedBoostCtr;
			speedBoostCtr = 0;
			speedBoosted = false;
			speedTimer = 0f;
			player.GetComponent<SpriteRenderer> ().color = new Color (255f, 255f, 255f);
		}

		if (lWallJump) {
			if (move > 0 && jump > 0) {
				myRB.velocity = new Vector2 (myRB.velocity.x, jumpHeight);
				lWallJump = false;
			}
		}
		if(rWallJump){
			if (move < 0 && jump > 0) {
				myRB.velocity = new Vector2 (myRB.velocity.x, jumpHeight);
				rWallJump = false;
			}
		}
		if (player.tag == "player1") {
			jump = Input.GetAxis ("P1-Vertical");
			move = Input.GetAxis ("P1-Horizontal");
			myAnim.SetFloat ("speed", Mathf.Abs(move));
		}
		if (player.tag == "player2") {
			jump = Input.GetAxis ("P2-Vertical");
			move = Input.GetAxis ("P2-Horizontal");
			myAnim.SetFloat ("speed", Mathf.Abs(move));
		}
		if (grounded && jump > 0) {
			if (zeroGravity == true) {
				myRB.velocity = new Vector2(myRB.velocity.x, -jumpHeight);
				myAnim.SetFloat ("verticalSpeed", myRB.velocity.y);
			} else {
				myRB.velocity = new Vector2 (myRB.velocity.x, jumpHeight);
			}
			myAnim.SetBool ("grounded", grounded);
			grounded = false; 
		}
		if (move < 0 && faceRight && !lWallJump) {
			flip ();
		} else if (move > 0 && !faceRight && !rWallJump) {
			flip ();		
		}
		if (player.transform.position.y < lowestY || player.transform.position.y > highestY) {
			Vector3 temp = new Vector3 (lastCheckPoint.x, lastCheckPoint.y + 1, lastCheckPoint.z);
			player.transform.position = temp;
			if (zeroGravity) {
				zeroGravity = false;
				myRB.velocity = new Vector2 (0f, 0f);
				player.GetComponent<SpriteRenderer> ().flipY = false;
				myRB.gravityScale = 2;
				Vector2 offset = player.GetComponent<BoxCollider2D> ().offset;
				player.GetComponent<BoxCollider2D> ().offset = new Vector2 (offset.x, colliderYOffSet);
				player.GetComponent<PlayerShooting> ().setYRange (5.0f);
			}
			myRB.velocity = new Vector2 (0f, 0f);
		}
		myRB.velocity = new Vector2 (speed * move, myRB.velocity.y);
		myAnim.SetBool ("grounded", grounded);
		myAnim.SetFloat ("verticalSpeed", myRB.velocity.y);
	}

	private void flip(){
		faceRight = !faceRight;
		Vector3 temp = player.transform.localScale;
		temp.x *= -1;
		player.transform.localScale = temp;
	}

	public bool getDirection(){
		return faceRight;
	}

	public void freezePlayer(bool freeze){
		frozen = freeze;
	}

	public bool inZeroGravity(){
		return zeroGravity;
	}
}
