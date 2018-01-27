using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TargetPath {
    public class Pocket
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

    public class Target
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

    public class Shot_Tools
    {
        public bool Measure_Collision(Vector3 ball, Vector3 destination)
        {
            RaycastHit collision; 
            Physics.Linecast(ball, destination, out collision);
            if (collision.transform.position == destination)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw_Path(Vector3 cue, Vector3 ball, Vector3 pocket)
        {
            GameObject line1 = new GameObject("line");
            line1.tag = "Solution";
            LineRenderer shot1 = line1.AddComponent<LineRenderer>();
            shot1.SetWidth(0.3f, 0.3f);

            GameObject line2 = new GameObject("line");
            line2.tag = "Solution";
            LineRenderer shot2 = line2.AddComponent<LineRenderer>();
            shot2.SetWidth(0.3f, 0.3f);

            Vector3[] coord1 = new Vector3[2] { cue, ball };
            Vector3[] coord2 = new Vector3[2] { ball, pocket };
            shot1.SetPositions(coord1);
            shot2.SetPositions(coord2);
        }

        public bool Calculate_Angle_toPocket(Vector3 end, Vector3 start, Vector3 cue)
        {

            Shot_Tools path = new Shot_Tools();
            Pocket optimal_pocket = new Pocket();
            if (path.Measure_Collision(end, start))
            {
                // Look into direction of cue ball to solid
                Vector3 start_trajectory = (cue - start).normalized;
                Vector3 final_trajectory = (end - start).normalized;

                //Determine if it's possible to make the shot. If within the angle threshold, it is a possible match for a shot
                float angle = Vector3.Angle(final_trajectory, start_trajectory);
                if (angle > 135.0f)
                {
                    return true;
                }
                else
                {
                    return false;
                    //Create some cases for the bounce shot
                }
            }
            //You must go around the obstruction
            return false;
        }

    }
}

namespace TargetPath {
    public class Quick_Shot : MonoBehaviour
    {

        GameObject[] pockets;
        GameObject[] balls;
        GameObject cue;
        RaycastHit collision;
        Shot_Tools path;
        //LinkedList<Target> options;
        double best_weight = 100;
        //LineRenderer linerenderer;

        // Use this for initialization
        void Start()
        {
            cue = GameObject.FindGameObjectWithTag("Cue");
            balls = GameObject.FindGameObjectsWithTag("Solid");
            pockets = GameObject.FindGameObjectsWithTag("Pocket");
            best_weight = 100;
            path = new Shot_Tools();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Cue_to_Ball()
        {
            Start();
            Clear();
            //ScreenShot();
            Target Final_shot = new Target();
            foreach (GameObject ball in balls)
            {
                Target target_path = new Target();
                if (path.Measure_Collision(cue.transform.position, ball.transform.position))
                {
                    //Direction from cue to target ball
                    Vector3 ball_cue_trajectory = (cue.transform.position - ball.transform.position).normalized;
                    target_path.opt_pocket = Ball_to_Pocket(ball, ball_cue_trajectory);
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
                    double shot_risk = 0.45 * target_path.distance + 0.55 * target_path.opt_pocket.distance;
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
            path.Draw_Path(cue.transform.position, Final_shot.position, Final_shot.opt_pocket.position);

        }

        Pocket Ball_to_Pocket(GameObject ball, Vector3 cue_ball_trajectory)
        {
            Pocket optimal_pocket = new Pocket();
            foreach (GameObject pocket in pockets)
            {
                if (path.Measure_Collision(ball.transform.position, pocket.transform.position))
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

        public void Draw_Path(Vector3 cue, Vector3 ball, Vector3 pocket)
        {
            GameObject line1 = new GameObject("line");
            line1.tag = "Solution";
            LineRenderer shot1 = line1.AddComponent<LineRenderer>();
            shot1.SetWidth(0.3f, 0.3f);

            GameObject line2 = new GameObject("line");
            line2.tag = "Solution";
            LineRenderer shot2 = line2.AddComponent<LineRenderer>();
            shot2.SetWidth(0.3f, 0.3f);

            Vector3[] coord1 = new Vector3[2] { cue, ball };
            Vector3[] coord2 = new Vector3[2] { ball, pocket };
            shot1.SetPositions(coord1);
            shot2.SetPositions(coord2);
        }

        public bool Measure_Collision(Vector3 ball, Vector3 destination)
        {
            Physics.Linecast(ball, destination, out collision);
            if (collision.transform.position == destination)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            GameObject[] Lines = GameObject.FindGameObjectsWithTag("Solution");

            foreach (GameObject line in Lines)
            {
                GameObject.Destroy(line);
            }
        }

		public void ScreenShot()
		{
			Application.CaptureScreenshot("Screenshot.png");
		}
    }
}

