using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastForward : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		float theDistance;
		Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
		Debug.DrawRay (transform.position, forward, Color.white);

		if (Physics.Raycast (transform.position, forward, out hit)) {
			theDistance = hit.distance;
			print (theDistance + " " + hit.collider.gameObject.name);
		}
	}
}
