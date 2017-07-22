using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Pocket
{
    public float distance;
    public Vector3 position;
    public bool foundpath;

    public Pocket()
    {
        this.distance = int.MaxValue;
        this.foundpath = false;
    }
}

class Target
{
    public float distance;
    public Vector3 position;
    public bool foundpath;
    public Pocket opt_pocket;

    public Target()
    {
        this.distance = int.MaxValue;
        this.foundpath = false;
    }
}

public class Quick_Shot : MonoBehaviour {

    GameObject[] pockets;
    GameObject[] balls;
    GameObject cue;
    RaycastHit collision;

    int Table_Mask;
    int optimal_distance = int.MaxValue;
    float dist_ball_pocket;
    float opt_dist_ball_pocket = int.MaxValue;
    float dist_cue_ball;


    // Use this for initialization
    void Start () {
        cue = GameObject.FindGameObjectWithTag("Cue");
        balls = GameObject.FindGameObjectsWithTag("Solid");
        pockets = GameObject.FindGameObjectsWithTag("Pocket");
        Table_Mask = LayerMask.GetMask("Table");
	}
	
	// Update is called once per frame
	void Update () {
        foreach (GameObject pocket in pockets)
        {
        
        }
    }

    void Cue_to_Ball()
    {
        Target target_path = new Target();
        foreach (GameObject ball in balls)
        {
            //Check if obstacle free path
            Physics.Linecast(cue.transform.position, ball.transform.position, out collision);
            if (collision.transform.position != ball.transform.position)
            {
                //Find the optimal pocket. Foundpath parameter returns true if valid path is available
                target_path.opt_pocket = Ball_to_Pocket(ball);
                if (target_path.opt_pocket.foundpath)
                {
                    target_path.foundpath = true;
                    target_path.distance = Vector3.Distance(cue.transform.position, ball.transform.position);
                    target_path.position = ball.transform.position;
                }
            }
        }
    }

    Pocket Ball_to_Pocket(GameObject ball)
    {
        //Vector3 optimal_pocket = new Vector3( 0, 0, 0 );
        Pocket optimal_pocket = new Pocket();
        foreach (GameObject pocket in pockets)
        {
            Physics.Linecast(ball.transform.position, pocket.transform.position, out collision);
            if (collision.transform.position != pocket.transform.position)
            {
                // Look into direction of cue ball to solid
                //There may be an error here, if the optimal pocket distance is not correctly set to Inf
                if (Vector3.Distance(collision.transform.position, ball.transform.position) < optimal_pocket.distance)
                {
                    optimal_pocket.foundpath = true;
                    optimal_pocket.distance = dist_ball_pocket;
                    optimal_pocket.position = pocket.transform.position;
                }
            }
        }
        //Need to hold two values: Shortest distance and pocket that results in shortest distance
        return optimal_pocket;

    }
}
