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
	public string UserNameInput;
	public string CreateUserURL = "localhost:8080/BilliardsBuddy/InserUser.php";
	public string CheckUserURL = "localhost:8080/BilliardsBuddy/CheckUser.php";
	public Button CreateUserBtn;
	public bool CreateBtnisClicked;

	// Use this for initialization
	void Start () {

		loginMenu = loginMenu.GetComponent<Canvas> ();
		userMenu = userMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		scores = scores.GetComponent<Button> ();
		loginMenu.enabled = false;
		userMenu.enabled = false;
//		CreateUserBtn = CreateUserBtn.GetComponent<Button> ();
//		CreateUserBtn.onClick.AddListener (CreatePress);
//		CreateBtnisClicked = false;

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

	public void CreatePress(){
		print (UserNameInput);
		StartCoroutine (CheckAndCreateUser (UserNameInput));
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

	public void EditEnd(string input){
		UserNameInput = input;
	}

	IEnumerator CheckAndCreateUser (string username){

		WWWForm form = new WWWForm();
		form.AddField ("usernamePost",username);
		WWW www = new WWW (CheckUserURL, form);
		yield return www;
		print (www.text);

		if (www.text == "False") {
			WWWForm form2 = new WWWForm ();
			form2.AddField ("usernamePost", username);
			WWW www2 = new WWW (CreateUserURL, form2);
			yield return www2;
		} else {
			print ("Already Exists");
			//add popup here
		}
	}
		
	 
	// Update is called once per frame
	void Update () {
		
	}
}
