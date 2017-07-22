using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycast_test : MonoBehaviour {
    GameObject cue_ball;
    GameObject ball;
    GameObject[] balls;

    int ballMask;
    Ray shootRay;
    RaycastHit shootHit;

    // Use this for initialization
    void Start() {

        ball = GameObject.FindGameObjectWithTag("Black");
        //balls = GameObject.FindGameObjectsWithTag("Solid");
        cue_ball = GameObject.FindGameObjectWithTag("Cue");
        ballMask = LayerMask.GetMask("Ball");


    }
	
	// Update is called once per frame
	void Update () {
        balls = GameObject.FindGameObjectsWithTag("Solid");
        Cast_Shot(cue_ball, ball);
        foreach (GameObject solid_ball in balls)
        {
            Cast_Shot(cue_ball, solid_ball);
        }
	}

    void Cast_Shot(GameObject cue_ball, GameObject ball)
    {
        //This if case is important when finding a collision between two balls, primarily the second check of the if statements
        //The second object will always have a collider, making the linecast return true. Therefore require second check to maek sure colliison isnt the original target
        if (Physics.Linecast(cue_ball.transform.position, ball.transform.position, out shootHit, ballMask))
//        if (Physics.Linecast(cue_ball.transform.position, ball.transform.position, out shootHit, ballMask))
        {

            //print("There's a child in the way! Look out!");

            Debug.DrawLine(cue_ball.transform.position, shootHit.point, Color.cyan);
        }
        else
        {
            print("Coast is clear!");
        }

    }
}
