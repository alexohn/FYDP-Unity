using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour {


	// Use this for initialization
	void Start () {


	}

	public void BackToMenu () {
		SceneManager.LoadScene ("Starting_Screen");

	}

	// Update is called once per frame
	void Update () {

	}
}
