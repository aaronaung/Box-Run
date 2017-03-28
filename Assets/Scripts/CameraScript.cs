using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public Transform player;
	public Transform lastBackground;
	public float lowestY;
	public float farthestX;
	public float cameraSpeed;

	Vector3 offset;
	// Use this for initialization
	void Start () {
		lowestY = GameObject.FindGameObjectWithTag ("Background").transform.position.y;
		farthestX = lastBackground.transform.position.x;
		offset = transform.position - player.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 targetPos = player.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetPos, cameraSpeed * Time.deltaTime);

		if (transform.position.y < lowestY) {
			transform.position = new Vector3 (transform.position.x, lowestY, transform.position.z);
		}
		if (transform.position.x > farthestX) {
			transform.position = new Vector3 (farthestX, transform.position.y, transform.position.z);
		}
	}
}
