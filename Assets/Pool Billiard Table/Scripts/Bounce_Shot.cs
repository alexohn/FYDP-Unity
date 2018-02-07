using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetPath
{
    public class Bounce_Shot
    {
        public int num_bounces;
        GameObject wall1; //Either top or bottom wall. This means it will always have a constant x
        GameObject wall2; //Either left or right wall. This means it will always have a constant z
        GameObject cue;
        GameObject ball;
        GameObject pocket;
        RaycastHit bounce_point;
        int scenario;
        int quadrant; //find out where the start ball is relative to th end position

        public Bounce_Shot(GameObject cue, GameObject ball, GameObject pocket)
        {
            this.cue = cue;
            this.ball = ball;
            this.pocket = pocket;
            this.scenario = 0;
        }

        private int DetermineCase()
        {
            Shot_Tools path = new Shot_Tools();
            //Determine if there is obstruction between ball and cue. This scenario is assigned 4
            if (!path.Measure_Collision(cue.transform.position, ball.transform.position)) {
                this.scenario = this.scenario + 4;
            }

            //Determine if there is obstruction between pocket and ball. This scenario is assigned 2
            if (!path.Measure_Collision(pocket.transform.position, ball.transform.position)) {
                this.scenario = this.scenario + 2;
            }

            //Determine if the cue ball can not reach the angle required to sink the ball in the associated pocket. This scenario is assigned 1
            if (!path.Calculate_Angle_toPocket(pocket.transform.position, ball.transform.position, cue.transform.position)) {
                this.scenario = this.scenario + 1;
            }

            if (this.scenario != 4 && this.scenario != 2 && this.scenario != 1){
                Debug.Log("The bounce shot is currently not possible with todays technology");
                return -1;
            }
            else{
                return 0;
            }  
        }
        //Determine where to bounce the ball, based on the location of all the balls

        public void SelectWall(Vector3 start, Vector3 end)
        {
            if (end.z > start.z)
            {
                this.wall1 = GameObject.FindGameObjectWithTag("Wall_Bottom");
                //If ball is in front of the pocket
                if (end.x < start.x)
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Right");
                    this.quadrant = 2;
                }
                //If ball is behind the pocket
                else
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Left");
                    this.quadrant = 1;
                }
            }
            //If pocket is below the ball
            else
            {
                this.wall1 = GameObject.FindGameObjectWithTag("Wall_Top");
                //If ball is in front of the pocket
                if (end.x < start.x)
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Right");
                    this.quadrant = 3;
                }
                //If ball is behind the pocket
                else
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Left");
                    this.quadrant = 0;
                }
            }
        }

        public Vector3 Error_Correction(RaycastHit collision, Vector3 destination, Vector3 intercept, GameObject wall)
        {
            Vector3 error;
            if (Vector3.Distance(collision.point, destination) < 1f)
            {
                return intercept;
            }
            error = collision.point - destination;
            if (wall.CompareTag("Wall_Top") || wall.CompareTag("Wall_Bottom"))
            {
                intercept.Set(intercept.x - error.x * 0.75f, intercept.y, intercept.z);
            }
            else
            {
                intercept.Set(intercept.x, intercept.y, intercept.z - error.z * 0.75f);
            }
            return intercept;
        }

        public Vector3 Around_Obstruction_Pocket(Vector3 ball, Vector3 pocket)
        {
            Shot_Tools path = new Shot_Tools();
            RaycastHit collision;
            Vector3 reflect_shot = new Vector3();
            Vector3 intercept;
            Vector3 error;
            int count = 0;
            SelectWall(ball, pocket);
            Debug.Log(this.wall1.tag);
            Debug.Log(this.wall2.tag);

            //Find first solution using wall1
            Vector3 midpoint = (pocket - ball) * 0.5f + ball;
            do
            {
                //First find the intercept using wall1 (either the top or bottom wall)
                intercept = new Vector3(midpoint.x, wall1.transform.position.y, wall1.transform.position.z);
                Vector3 incident_shot = (intercept - ball).normalized;

                //The incident angle may change depending on whether the wall is top or bottom
                if (this.wall1.CompareTag("Wall_Top"))
                {
                    reflect_shot = Vector3.Reflect(incident_shot, Vector3.back * 1f);
                }
                else
                {
                    reflect_shot = Vector3.Reflect(incident_shot, Vector3.forward* 1f);
                }

                Physics.Raycast(intercept, reflect_shot, out collision);
                if (Vector3.Distance(collision.point, pocket) < 1f)
                {
                    break;
                }
                error = collision.point - pocket;
                if (collision.point.z > pocket.z)
                {
                    midpoint = (midpoint - ball) * 0.5f + ball;
                    count++;
                }
                else
                {
                    midpoint = (pocket - midpoint) * 0.5f + midpoint;
                    count++;
                }
            } while (count < 3);
            Vector3 solution1_intercept = intercept;
            path.Draw_Solution1(ball, solution1_intercept, collision.point);

            if (path.Measure_Collision(intercept, pocket))
            {
                //Will most likely need to return the incident trajectory
                Debug.Log((ball - intercept).normalized);
                return (ball-intercept).normalized;
            }
            else
            {

                Debug.Log("The shot cannot be bounced in");
                return (ball-intercept).normalized;
            }
        }

        public Vector3 Around_Obstruction_Ball(Vector3 ball, Vector3 destination)
        {
            Shot_Tools path = new Shot_Tools();
            RaycastHit collision;
            Vector3 reflect_shot = new Vector3();
            Vector3 intercept;
            Vector3 error;
            int count = 0;
            SelectWall(ball, destination);

            //First solution using wall1
            Vector3 midpoint = (destination - ball) * 0.5f + ball;
            intercept = new Vector3(midpoint.x, wall1.transform.position.y, wall1.transform.position.z);
            do
            {
                //First find the intercept using wall1 (either the top or bottom wall)
                
                Vector3 incident_shot = (intercept - ball).normalized;
                //The incident angle may change depending on whether the wall is top or bottom
                if (this.wall1.CompareTag("Wall_Top"))
                {
                    reflect_shot = Vector3.Reflect(incident_shot, Vector3.back * 1f);
                }
                else
                {
                    reflect_shot = Vector3.Reflect(incident_shot, Vector3.forward * 1f);
                }
                Physics.Raycast(intercept, reflect_shot, out collision);
                intercept = Error_Correction(collision, destination, intercept, this.wall1);
                /*
                if (Vector3.Distance(collision.point, destination) < 1f)
                {
                    break;
                }
                error = collision.point - destination;
                if (Mathf.Abs(error.x) > 0f)
                {
                    intercept.Set(intercept.x - error.x*0.75f, intercept.y, intercept.z);
                    count++;
                }
                else
                {
                    break;
                }
                */
                count++;
            } while (count < 10);
            Vector3 Solution1_intercept = intercept;
            path.Draw_Solution1(ball, Solution1_intercept, collision.point);
            return (ball - Solution1_intercept).normalized;
            /*
            //Determine Second Solution using wall2
            midpoint = (destination - ball) * 0.5f + ball;
            intercept = new Vector3(wall2.transform.position.x, wall2.transform.position.y, midpoint.z);
            do
            {
                //First find the intercept using wall1 (either the top or bottom wall)

                Vector3 incident_shot = (intercept - ball).normalized;
                //The incident angle may change depending on whether the wall is top or bottom
                if (this.wall1.CompareTag("Wall_Left"))
                {
                    reflect_shot = Vector3.Reflect(incident_shot, Vector3.left * 1f);
                }
                else
                {
                    reflect_shot = Vector3.Reflect(incident_shot, Vector3.right * 1f);
                }
                Physics.Raycast(intercept, reflect_shot, out collision);
                path.Draw_Solution2(ball, intercept, collision.point);

                if (Vector3.Distance(collision.point, destination) < 1f)
                {
                    break;
                }
                error = collision.point - destination;
                if (Mathf.Abs(error.z) > 0f)
                {
                    intercept.Set(intercept.x, intercept.y, intercept.z-error.z*0.75f);
                    count++;
                }
                else
                {
                    break;
                }
                
            } while (count < 10);
            Vector3 Solution2_intercept = intercept;
            path.Draw_Solution2(ball, Solution2_intercept, collision.point);


            if (path.Measure_Collision(intercept, destination))
            {
                //Will most likely need to return the incident trajectory
                return (ball - Solution1_intercept).normalized;
            }
            else
            {

                Debug.Log("The shot cannot be bounced in");
                return (ball - Solution1_intercept).normalized;
            }
            */
        }

        public bool Cue_to_Ball_Bounce_Angle(Vector3 required_trajectory)
        {
            Shot_Tools path = new Shot_Tools();
            if (path.Measure_Collision(this.cue.transform.position, this.ball.transform.position))
            {
                Vector3 start_trajectory = (this.cue.transform.position - this.ball.transform.position).normalized;
                float angle = Vector3.Angle(required_trajectory, start_trajectory);
                if (angle > 135.0f)
                {
                    //Not required to do bounce shot, just find the actual place to hit from
                    path.Draw_Solution1 (this.cue.transform.position, this.ball.transform.position, required_trajectory);//------------------------------------------------------------------------This part is most likely wrong
                    return true;
                }
                else
                {
                    //return Obstruction_to_Ball(required_trajectory);
                    return true;
                }
            }
            else
            {
                //return Obstruction_to_Ball(required_trajectory);
                return true;
            }
            
        }
        public bool Obstruction_to_Ball(Vector3 Incident_Trajectory)
        {
            Shot_Tools path = new Shot_Tools();
            //If you know the trajectory, you know the exact position the ball needs to be, which is one diameter away from target ball, in the direction of required trajectory

            Vector3 impactpoint = Vector3.MoveTowards(this.ball.transform.position, Incident_Trajectory, -0.0575f * 5); //Need to determine radius of ball
            Around_Obstruction_Ball(this.cue.transform.position, impactpoint);
            return true;
        }
        
        public bool Improper_Angle()
        {
            return false;
        }
        
    }

}
