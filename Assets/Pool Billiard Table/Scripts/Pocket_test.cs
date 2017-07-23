using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket_test : MonoBehaviour {

    GameObject pocket;
    GameObject ball;
    GameObject cue;
    RaycastHit collision;

    int optimal_distance = int.MaxValue;
    float dist_ball_pocket;
    float opt_dist_ball_pocket = int.MaxValue;
    float dist_cue_ball;


    // Use this for initialization
    void Start()
    {
        cue = GameObject.FindGameObjectWithTag("Cue");
        ball = GameObject.FindGameObjectWithTag("Solid");
        pocket = GameObject.FindGameObjectWithTag("Pocket");
    }

    void Update()
    {
            
        Debug.DrawLine(cue.transform.position, ball.transform.position, Color.red);
        Debug.DrawLine(ball.transform.position, pocket.transform.position);
        Vector3 ball_pocket_trajectory = (pocket.transform.position - ball.transform.position).normalized;
        Vector3 ball_cue_trajectory = (cue.transform.position - ball.transform.position).normalized;
        float angle = Vector3.Angle(ball_cue_trajectory, ball_pocket_trajectory);
        print(Vector3.Distance(pocket.transform.position, ball.transform.position));
        //print(angle);


    }

}
