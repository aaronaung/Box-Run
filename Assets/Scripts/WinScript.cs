using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour {
	public Text winText;
	public GameObject restart;
	public Text cannon1;
	public Text cannon2;
	Text restartText;
	Color winColor;
	public GameObject player1;
	public GameObject player2;

	// Use this for initialization
	void Start () {
		restartText = restart.gameObject.transform.GetChild (0).GetChild (0).gameObject.GetComponent<Text>();
	}

	void OnTriggerEnter2D(Collider2D player){
		if (player.tag == "cannon1" || player.tag == "cannon2") {
			return;
		}
		if (player.tag == "player1") {
			winColor = new Color (1.0f, 0.32f, 0.32f);
			endGame (winColor, "Winner: P1");
		}
		if (player.tag == "player2") {
			winColor = new Color (0.32f, 0.32f, 1.0f);
			endGame (winColor,"Winner: P2");
		}

		disable (player1);
		disable (player2);
	}

	private void endGame(Color color, string win){
		winText.text = win;
		winText.color = color;
		cannon1.enabled = false;
		cannon2.enabled = false;
		restart.SetActive (true);
		restartText.color = color;
	}

	private void disable (GameObject player){
		player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0f, 0f);
		player.GetComponent<Animator> ().SetBool ("grounded", true);
		player.GetComponent<Animator> ().SetFloat ("speed", 0);
		player.GetComponent<PlayerMovement> ().enabled = false;
		player.GetComponent<PlayerShooting> ().enabled = false;
	}

	public void restartGame(){
		SceneManager.LoadScene ("Menu");
	}
}
