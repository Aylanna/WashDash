﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Canvas rollADice;
    public Canvas gameMenu;
	public Canvas playerOrder;
	public bool isMinigamePlayed;
	public GameObject mainScene;
    public int playerIDDice;
	public int playerIDMovement;
    public int diceValue;
    public GameObject activePlayer; 
    public int state = 0;
	private BoardController bc;
    public List <GameObject> players;
    public int numbersOfPlayers = 0;
    public GameObject[] orderOfPlayer;
    public bool isDice = false;
    private Dice dice;
	private int round = 1;
	public GameObject startField;


    void Start()
    {
		//orderOfPlayer = new GameObject[numbersOfPlayers];
		//dice = GetComponent<Dice> ();
		//bc = GetComponent<BoardController> ();
       
    }
 
    void Update()
    {
        switch(state)
        {
            // Game preparation
		case 0:
			orderOfPlayer = new GameObject[numbersOfPlayers];
			dice = GetComponent<Dice> ();
			bc = GetComponent<BoardController> ();
			gameMenu.GetComponent<Canvas> ().enabled = false;
			gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage (" ");


                break;
			//Player role for highscore
            case 1:
                if(playerIDDice < numbersOfPlayers) {
					DiceForHighscore ();
                }
                else
                {
                 SortOrderOfPlayers();
				StartCoroutine (HighScoreState());
                   // TestPlayerOrder();
				isDice = false;
				dice.SetDiceActive ();
				dice.SetMessage ("Round: " + round.ToString());
				 playerIDDice = 0;
				dice.InActivateNextPlayer ();
				StartCoroutine (RollADiceState());
				//state = 2;
			}
			break;
		//Player dice
		case 2:
			//Invoke ("SetDiceCanvasActive", 1);
			rollADice.enabled = true;
			activePlayer = orderOfPlayer [playerIDDice];
			if (activePlayer.GetComponent<PlayerController> ().GetSkipAt () == round) {
				state = 10;

			} else {
				dice.messageText.text = activePlayer.GetComponent<PlayerController> ().playerName;
				if (isDice) {
					//Invoke ("SetDiceCanvasInactive", 1);
					//StartCoroutine (MoveState());
					rollADice.enabled = false;
					gameMenu.enabled = true;
					isDice = false;
					gameMenu.GetComponent<GameMenu> ().UpdateView ();
					gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage (" ");
					gameMenu.GetComponent<Canvas> ().enabled = true;
					state = 3;
				}
			}
			break;
		//Player moves
		case 3:
			if (!rollADice.GetComponent<Canvas> ().isActiveAndEnabled) {
				bc.HandleBoardEvent ();
				state = 4; 
			}

			break;
		//FieldAct
		case 4:
			bc.HandleFieldAct ();
			break;
		//ExtraDice
		case 5:
			gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage ("Extra Dice!!!");
			state = 2;
			break;
		//ExtraLife
		case 6:
			gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage ("Extra Life!!!");
			activePlayer.GetComponent<PlayerController> ().AddLifePoints ();
			state = 10;
			break;
		//Minigame
		case 7:
			gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage ("Minigame!!!");
			if (isMinigamePlayed) {
				isMinigamePlayed = false;
				state = 10;

			}

			//Else Wait for the end of the minigame
			break;
		//FieldAction Movement
		case 8:
			gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage ("Extra Move!!!");
			state = 3;
			break;

		case 9:
			gameMenu.GetComponent<GameMenu> ().SetFieldEventMessage ("Skip!!!");
			activePlayer.GetComponent<PlayerController> ().SetSkipAt (round + 1);
			state = 10;
			break;

		//next player or new round
		case 10:
			if (playerIDDice < orderOfPlayer.Length - 1) {
				playerIDDice++;
					
			} else {
				playerIDDice = 0;
				round++;
				RemoveSkip ();
				dice.SetMessage ("Round: " + round.ToString());

			}
			state = 2;
                break;
        }
    }

	/**
	private void SetGameMenuInActive(){
		gameMenu.GetComponent<Canvas> ().enabled = false;
	}

    private void SetDiceCanvasInactive()
    {
		rollADice.GetComponent<Canvas>().enabled = false;
    }

	private void SetDiceCanvasActive()
	{
		rollADice.GetComponent<Canvas>().enabled = true;
	}
   

    public void TestPlayerOrder()
    {
        Debug.Log("Order of Player: ");
        foreach(GameObject p in orderOfPlayer)
        {
            Debug.Log(p.GetComponent<PlayerController>().playerName + "   " + p.GetComponent<PlayerController>().GetDiceValue().ToString());
        }
    }
	*/
	public void SortOrderOfPlayers()
	{
		Array.Sort(orderOfPlayer,
			delegate (GameObject player1, GameObject player2) { return - player1.GetComponent<PlayerController>().GetDiceValue().
				CompareTo(player2.GetComponent<PlayerController>().GetDiceValue()); });
	}
 

	void DiceForHighscore() {
		rollADice.GetComponent<Canvas>().enabled = true;
		activePlayer = orderOfPlayer [playerIDDice];
		dice.messageText.text = activePlayer.GetComponent<PlayerController> ().playerName;
	}

	void RemoveSkip() {
		for (int i = 0; i < orderOfPlayer.Length; i++) {
			if (orderOfPlayer [i].GetComponent<PlayerController> ().GetSkipAt () == (round - 1) && (round > - 1)) {
				orderOfPlayer [i].GetComponent<PlayerController> ().SetSkipAt (-1);
			}
		}

	}

	protected IEnumerator HighScoreState()
	{
		yield return new WaitForSeconds(1.0f);
		rollADice.enabled = false;
		playerOrder.enabled = true;
		playerOrder.GetComponent<OrderOfPlayers> ().ShowOrderOfPlayer ();

	}

	protected IEnumerator RollADiceState()
	{
		yield return new WaitForSeconds(3.0f);
		playerOrder.enabled = false;
		rollADice.GetComponent<Canvas>().enabled = true;
		state = 2;


	}

	protected IEnumerator MoveState()
	{
		yield return new WaitForSeconds(1.0f);
		rollADice.GetComponent<Canvas>().enabled = false;
		gameMenu.GetComponent<Canvas>().enabled = true;


	}

}
