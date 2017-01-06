﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PaddleController : MonoBehaviour {

	public Rigidbody2D rb;
	public float speed;
	public float maxX;

	private bool currentPlatformAndroid = false; 

	void Awake() {
		#if UNITY_ANDROID
		currentPlatformAndroid = true; 
		#endif 
	}


	// Use this for initialization
	void Start () {
		if (currentPlatformAndroid) {
			Debug.Log ("Android"); 
		} else {
			Debug.Log ("Windows");
		}
	}

	// Update is called once per frame
	void Update () {

		if (currentPlatformAndroid) {
			AccelerometerMove ();
		} else {
			MoveInput ();
		}
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x,-maxX,maxX);
		transform.position = pos;
	}

	void MoveLeft(){
		rb.velocity = new Vector2 (-speed,0);
	}

	void MoveRight(){

		rb.velocity = new Vector2 (speed,0);
	}

	void Stop(){
		rb.velocity = Vector2.zero;
	}

	void AccelerometerMove () {
		float x = Input.acceleration.x; 

		if (x < -0.1f) {
			MoveLeft ();
		} else if (x > 0.1f) {
			MoveRight (); 
		} else {
			Stop (); 
		}
	}

	void MoveInput() {
		float x = Input.GetAxis ("Horizontal");

		if (x < 0) {
			MoveLeft ();
		}

		if (x > 0) {
			MoveRight ();
		}

		if (x == 0) {
			Stop ();
		}
	}
}