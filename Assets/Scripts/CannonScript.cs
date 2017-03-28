using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour {

	public int freezeTime; 
	PlayerMovement playerScript;
	float time =0;
	bool test = false; 
	// Use this for initialization
	void Start () {

	}

	void Update(){
		if (test == true) {
			time += Time.deltaTime;
		}
		if (time >= 2.0f) {
			playerScript.enabled = true;
			time = 0;
			test = false;
			playerScript.player.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
		}

	}
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D player){
		if ((player.tag == "player1" && this.tag == "cannon2") || (player.tag == "player2" && this.tag == "cannon1")) {
			playerScript = player.GetComponent<PlayerMovement> ();
			playerScript.enabled = false;
			test = true;
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			Destroy(this.transform.GetChild (0).gameObject);
			player.GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 0f);
		}
	}
	 
	IEnumerator afterFreeze(){
		yield return new WaitForSeconds (2);
		playerScript.enabled = true;
	}

}
