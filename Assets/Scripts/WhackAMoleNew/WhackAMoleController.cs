﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WhackAMoleController : MonoBehaviour {

	private int timerSeconds = 30;
	private SpawnCharacters spawner;
	private InputController ic;
	public Canvas startCanvas;
	public Canvas gameOverCanvas;
	public GameObject additiveScene;
	private Sceneloader scl;
	private bool extraLife;


	void Start () {
		
		spawner = GetComponent<SpawnCharacters> ();
		ic = GetComponent<InputController> ();
		gameOverCanvas.enabled = false;
	}

	void Update() {
		
		if (ic.gameOver) {
			gameOverCanvas.enabled = true;
			CancelInvoke ("CountDown");
			spawner.StopSpawn ();
			ic.gameOver = false;
		}
		if (ic.score == 3) {
			gameOverCanvas.enabled = true;
			CancelInvoke ("CountDown");
			spawner.StopSpawn ();
			ic.gameOver = false;
		}

	}

	void CountDown() {
		
		timerSeconds--;
		if (timerSeconds < 1) {
			CancelInvoke ("CountDown");
			spawner.StopSpawn ();
		}
	}

	public void StartGame() {
		
		InvokeRepeating ("Countdown", 1, 1);
		spawner.StartSpawnInterval ();
		startCanvas.enabled = false;
	}

	public void BackToBoard() {
		
		scl = GameObject.Find("Sceneloader").GetComponent<Sceneloader> ();
		scl.SetExtraLife (extraLife);
		scl.UnLoadMinigame ();
		Destroy (additiveScene);
	}
}
