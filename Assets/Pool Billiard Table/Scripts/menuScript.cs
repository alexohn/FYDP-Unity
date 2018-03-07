using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {

	public Canvas loginMenu;
	public Canvas userMenu;
	//public Button login; //login button
	public Button startText; //play as guest
	public Button scores; //database interaction



	// Use this for initialization
	void Start () {

		loginMenu = loginMenu.GetComponent<Canvas> ();
		userMenu = userMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		scores = scores.GetComponent<Button> ();
		loginMenu.enabled = false;
		userMenu.enabled = false;

	}

	public void LoginPress () {
		loginMenu.enabled = true;
		userMenu.enabled = false;
		startText.enabled = false;
		scores.enabled = false; 
	
	}

	public void userPress () {
		loginMenu.enabled = false;
		userMenu.enabled = true;
		startText.enabled = false;
		scores.enabled = false; 

	}


	public void NoPress () {
		loginMenu.enabled = false;
		userMenu.enabled = false;
		startText.enabled = true;
		scores.enabled = true; 

	}

	public void PlayAsGuest () {
		SceneManager.LoadScene ("Pool table");

	}


	 
	// Update is called once per frame
	void Update () {
		
	}
}
