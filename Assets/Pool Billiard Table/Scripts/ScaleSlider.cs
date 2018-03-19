using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSlider : MonoBehaviour {

	// Use this for initialization
	GameObject table;

	void Start () {
		table = GameObject.Find("Table");
	}
	public void btnChangeHeight() {
		table.gameObject.transform.localScale += new Vector3(0.02F,0,0);
	}

	public void btnChangeWidth() {
		table.gameObject.transform.localScale += new Vector3(0,0,0.02F);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
