using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour {
	public GameObject player = null;
	public GameObject cannon = null;
	PlayerMovement playerScript;

	//UI Variables
	public Text cannonCount;

	//Cannon Variables
	float xRange = 7.0f;
	float yRange = 5.0f;
	float deleteTime = 5.0f;
	int numCannons = 5;
	float attackFreq = 0.3f;
	GameObject instantiated;
	bool shoot = false;

	//Timers
	float reloadTime = 0f;
	float attackTime = 0f;

	// Use this for initialization
	void Start () {
		playerScript = player.GetComponent<PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update(){
		reloadTime += Time.deltaTime;
		attackTime += Time.deltaTime;
		if (reloadTime > 10f) {
			reloadTime = 0f;
			numCannons += 5;
		}
		if (instantiated) {
			Destroy (instantiated , deleteTime);
		}
		cannonCount.text = "Cannon: " + numCannons;
	}
	void FixedUpdate () {
		if (player.tag == "player1") {
			shoot = Input.GetKeyDown (KeyCode.RightAlt);

		}
		if (player.tag == "player2") {
			shoot = Input.GetKeyDown (KeyCode.LeftAlt);

		}

		if (shoot && numCannons > 0 && attackTime >= attackFreq) {
			instantiated = Instantiate (cannon, player.transform.position, Quaternion.identity);
			if (playerScript.inZeroGravity ()) {
				instantiated.GetComponent<Rigidbody2D> ().gravityScale = -1;
				yRange = -5.0f;
			}
			if ((!playerScript.getDirection () && xRange > 0) || (playerScript.getDirection () && xRange < 0))
				xRange = -xRange;
			
			instantiated.gameObject.GetComponent<Rigidbody2D>().AddForce (new Vector2(xRange, yRange), ForceMode2D.Impulse);
			if (player.tag == "player1") {
				instantiated.gameObject.tag = "cannon1";
			}
			if (player.tag == "player2") {
				instantiated.gameObject.tag = "cannon2";
			}
			numCannons--;
			attackTime = 0f;
		}
	}

	public void setYRange(float range){
		this.yRange = range;
	}
}
