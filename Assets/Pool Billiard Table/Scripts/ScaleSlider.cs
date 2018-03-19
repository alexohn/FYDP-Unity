using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSlider : MonoBehaviour {

	// Use this for initialization
	GameObject table;
    public Slider heightSlider;
    public Slider widthSlider;

	void Start () {
		table = GameObject.Find("Table");
	}
	public void btnChangeHeight() {
        Debug.Log(heightSlider.value);
		table.gameObject.transform.localScale = new Vector3(heightSlider.value, table.gameObject.transform.localScale.y , table.gameObject.transform.localScale.z);
	}

	public void btnChangeWidth() {
        table.gameObject.transform.localScale = new Vector3(table.gameObject.transform.localScale.x, table.gameObject.transform.localScale.y, widthSlider.value);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
