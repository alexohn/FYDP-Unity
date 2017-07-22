using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Createball : MonoBehaviour {

	public GameObject ball;
	//var instance: GameObject = Instantiate(Resources.Load("enemy"));
	//public GameObject[] ballArray;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//not sure yet how the color detection will work, but it is needed to enter the right case
		//other option is to send sorted array that already says index 0 is cue, 1 is black, 2 is red etc
		//if ball doesnt exist populate that index with 0 so no ball is created
		/*
		for (int i = 0; i < opencv.arraylength; i++) {
			
			{
			case 1:
				gameObject cueball = Instantiate(cueball);
				cueball.transform.position = new Vector3(opencv[i][0], 42.5939, opencv[i][1]);
				break;
			case 2:
				gameObject blackball = Instantiate(blackball);
				cueball.transform.position = new Vector3(opencv[i][0], 42.5939, opencv[i][1]);
				break;
			case 3:
				gameObject solid = Instantiate(redball);
				cueball.transform.position = new Vector3(opencv[i][0], 42.5939, opencv[i][1]);
				break;
			default:
				print("no balls remaining");
				break;   
			}
		}
		*/

	}
}
