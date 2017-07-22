using UnityEngine;
using System.Collections;

public class Remove : MonoBehaviour {

	public float delay = 1.0f;

	void OnCollisionEnter (Collision collision) {

		Destroy(collision.gameObject, delay);

	}
}
