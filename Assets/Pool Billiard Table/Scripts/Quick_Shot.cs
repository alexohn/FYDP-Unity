using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Pocket
{
    public float distance;
    public Vector3 position;
    public bool valid_path;

    public Pocket()
    {
        this.distance = int.MaxValue;
        this.valid_path = false;
    }
}

class Target
{
    public float distance;
    public Vector3 position;
    public bool valid_path;
    public Pocket opt_pocket;

    public Target()
    {
        this.distance = int.MaxValue;
        this.valid_path = false;
    }
}

public class Quick_Shot : MonoBehaviour {

    GameObject[] pockets;
    GameObject[] balls;
    GameObject cue;
    RaycastHit collision;
    //LinkedList<Target> options;
    double best_weight = 100;
    //LineRenderer linerenderer;

    // Use this for initialization
    void Start () {
        cue = GameObject.FindGameObjectWithTag("Cue");
        balls = GameObject.FindGameObjectsWithTag("Solid");
        pockets = GameObject.FindGameObjectsWithTag("Pocket");
        
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Cue_to_Ball()
    {
    	Start();
        Target Final_shot = new Target();
        foreach (GameObject ball in balls)
        {
            Target target_path = new Target();
            Physics.Linecast(cue.transform.position, ball.transform.position, out collision);
            if (collision.transform.position == ball.transform.position)
            {
                //Direction from cue to target ball
                Vector3 ball_cue_trajectory = (cue.transform.position- ball.transform.position).normalized;
                target_path.opt_pocket = Ball_to_Pocket(ball,ball_cue_trajectory);
                if (target_path.opt_pocket.valid_path)
                {
                    target_path.valid_path = true;
                    target_path.distance = Vector3.Distance(cue.transform.position, ball.transform.position);
                    target_path.position = ball.transform.position;
                }
            }
            if (target_path.valid_path == true)
            {
                //Determine if it's the best trajectory found 
                double shot_risk = 0.45* target_path.distance + 0.55* target_path.opt_pocket.distance;
                print(shot_risk);
                print(target_path.position);
                print(target_path.distance);
                print(target_path.opt_pocket.distance);
                if (shot_risk < best_weight)
                {
                    Final_shot = target_path;
                    best_weight = shot_risk;
                }
            }
        }
        GameObject line1 = new GameObject("line");
        LineRenderer shot1 = line1.AddComponent<LineRenderer>();
        shot1.SetWidth(0.3f, 0.3f);

        GameObject line2 = new GameObject("line");
        LineRenderer shot2 = line2.AddComponent<LineRenderer>();
        shot2.SetWidth(0.3f, 0.3f);

        Vector3[] coord1 = new Vector3[2] {cue.transform.position, Final_shot.position};
        Vector3[] coord2 = new Vector3[2] { Final_shot.position, Final_shot.opt_pocket.position };
        shot1.SetPositions(coord1);
        shot2.SetPositions(coord2);
        
    }

    Pocket Ball_to_Pocket(GameObject ball, Vector3 cue_ball_trajectory)
    {
        Pocket optimal_pocket = new Pocket();
        foreach (GameObject pocket in pockets)
        {
            Physics.Linecast(ball.transform.position, pocket.transform.position, out collision);
            if (collision.transform.position == pocket.transform.position)
            {
                // Look into direction of cue ball to solid
                Vector3 ball_pocket_trajectory = (pocket.transform.position - ball.transform.position).normalized;

                //Determine if it's possible to make the shot. If within the angle threshold, it is a possible match for a shot
                float angle = Vector3.Angle(cue_ball_trajectory, ball_pocket_trajectory);
                if (angle > 135.0f)
                {
                    
                    if (Vector3.Distance(pocket.transform.position, ball.transform.position) < optimal_pocket.distance)
                    {
                        optimal_pocket.valid_path = true;
                        optimal_pocket.distance = Vector3.Distance(pocket.transform.position, ball.transform.position);
                        optimal_pocket.position = pocket.transform.position;
                    }
                    
                }

            }
        }

        return optimal_pocket;

    }
}
