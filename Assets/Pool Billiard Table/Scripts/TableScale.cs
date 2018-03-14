using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScale : MonoBehaviour {


	public float z = 1;
	//[Range(0, 10)]
	public float x = 1;
	//[Range(0, 10)]
	public float y = 1;
	// Use this for initialization
	void Start () {

		//[Range(0, 10)]

	}
	public void slide () {
		transform.localScale = new Vector3(x, y, z);
	}
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3(x, y, z);
	}
}
