using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Scores : MonoBehaviour {


	public Canvas CurrentScore;
	public int madeShotCount = 0;
	public int shotCount=0;
	public double accuracy;
	// Use this for initialization
	void Start () {
		CurrentScore = CurrentScore.GetComponent<Canvas> ();
		print (menuScript.UserName);
	}

	public void MadeShot(){
		Transform child = CurrentScore.transform.Find("Text");
		Text t = child.GetComponent<Text>();
		shotCount++;
		madeShotCount++;
		accuracy = (double)madeShotCount / (double)shotCount;
		t.text=accuracy.ToString("0%"); 
	}

	public void MissedShot(){
		Transform child = CurrentScore.transform.Find("Text");
		Text t = child.GetComponent<Text>();
		shotCount++;
		accuracy = (double)madeShotCount / (double)shotCount;
		t.text=accuracy.ToString("0%"); 
	}

	IEnumerator InsertScore(string username,int accuracy)
	{
		
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField ("scorePost", accuracy);
		WWW www = new WWW("http://localhost:8080/BilliardsBuddy/InsertScores.php", form);
		yield return www;
		print(www.text);
	}



	public void ExitGame () {
		StartCoroutine (InsertScore(menuScript.UserName,Convert.ToInt32(accuracy*100)));
		SceneManager.LoadScene ("Starting_Screen");

	}

	// Update is called once per frame
	void Update () {

	}
}
