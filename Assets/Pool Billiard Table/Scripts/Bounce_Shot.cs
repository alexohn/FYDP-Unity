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
        static int count;
        int scenario;
        int quadrant; //find out where the start ball is relative to th end position

        public Bounce_Shot(GameObject cue, GameObject ball, GameObject pocket)
        {
            this.cue = cue;
            this.ball = ball;
            this.pocket = pocket;
            this.scenario = 0;
        }

        public int DetermineCase()
        {
            Shot_Tools path = new Shot_Tools();
            Vector3 incidenttrajectory = new Vector3();
            Vector3 impactpoint = new Vector3();
            count = 0;

            //Determine if there is an obstruction between the pocket and the ball
            if (!path.Measure_Collision(pocket.transform.position, ball.transform.position))
            {
                incidenttrajectory = Around_Obstruction_Pocket(this.ball.transform.position, this.pocket.transform.position);
                impactpoint = path.Impactpoint(this.ball.transform.position, incidenttrajectory);
            }
            else
            {
                incidenttrajectory = StraightShot(this.ball.transform.position, pocket.transform.position);
                impactpoint = path.Impactpoint(this.ball.transform.position, incidenttrajectory);
            }

            //Determine if there is obstruction between required impact point and cue. 
            if (!path.Measure_Collision_to_Impact(this.cue.transform.position, impactpoint, incidenttrajectory)) {
                SelectWall(impactpoint, this.ball.transform.position);
                Around_Obstruction_Ball(this.cue.transform.position, impactpoint, incidenttrajectory); 
            }
            else
            {
                StraightShot(this.cue.transform.position, impactpoint);
            }
            
            return 0;
            
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
            if(Mathf.Abs(error.x) < 1f)
            {
                return intercept;
            }
            if (wall.CompareTag("Wall_Top") || wall.CompareTag("Wall_Bottom"))
            {
                intercept.Set(intercept.x - (error.x/-error.x), intercept.y, intercept.z);
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
            Vector3 error;
            int count = 0;
            SelectWall(ball, pocket);

            //Find first solution using wall1
            Vector3 midpoint = (pocket - ball) * 0.5f + ball;
            //Determine which wall to react the impact point
            
            Vector3 intercept = RecursivePocket(pocket, ball, pocket, ball);
            path.Draw_Solution1(ball, intercept, pocket);
            
            /*
            Vector3 intercept = new Vector3(midpoint.x, wall1.transform.position.y, wall1.transform.position.z);
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
                if (Vector3.Distance(collision.point, pocket) < 1f)
                {
                    break;
                }
                error = collision.point - pocket;
                path.Draw_Solution1(ball, intercept, collision.point);
                if (Mathf.Abs(error.x) >= Mathf.Abs(error.z))
                {
                    if (this.quadrant == 2 || this.quadrant == 3)
                    {
                        intercept.Set(intercept.x - Mathf.Abs(error.x*0.75f), intercept.y, intercept.z);
                    }
                    else
                    {
                        intercept.Set(intercept.x + Mathf.Abs(error.x*0.75f), intercept.y, intercept.z);
                    }
                }
                else
                {
                    if (this.quadrant == 2 || this.quadrant == 3)
                    {
                        intercept.Set(intercept.x + Mathf.Abs(error.x*0.75f), intercept.y, intercept.z);
                    }
                    else
                    {
                        intercept.Set(intercept.x - Mathf.Abs(error.x*0.75f), intercept.y, intercept.z);
                    }
                    
                }
                count++;

            } while (count < 5);
            Vector3 solution1_intercept = intercept;
            path.Draw_Solution1(ball, solution1_intercept, collision.point);
            */
            return (ball-intercept).normalized;
        }

        public Vector3 Around_Obstruction_Ball(Vector3 ball, Vector3 impactpoint, Vector3 incident_trajectory)
        {
            Shot_Tools path = new Shot_Tools();
            RaycastHit collision;
            RaycastHit wall;

            Vector3 reflect_shot = new Vector3();
            Vector3 intercept;
            int count = 0;

            Vector3 midpoint = (impactpoint - ball) * 0.5f + ball;
            //Determine which wall to react the impact point

            Physics.Raycast(impactpoint, incident_trajectory, out wall);
            /*
            SelectWall(ball, impactpoint);

            if(wall.collider.tag != this.wall1.tag)
            {
                this.wall1 = GameObject.FindGameObjectWithTag(wall.collider.tag);
            }
            */
            Vector3 solutionintercept = RecursiveIntercept(impactpoint, ball, impactpoint, ball);
            path.Draw_Solution1(ball, solutionintercept, impactpoint);
            return solutionintercept;
            /*
            intercept = new Vector3(midpoint.x, wall1.transform.position.y, wall1.transform.position.z);
            intersect = new Plane(Vector3.left, impactpoint);
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
                reflect_projection = new Ray(intercept, reflect_shot);
                if (intersect.Raycast(reflect_projection, out enter))
                {
                    Vector3 hitpoint = reflect_projection.GetPoint(enter);
                    path.Draw_line(intercept, hitpoint);
                }

                Physics.Raycast(intercept, reflect_shot, out collision);
                //path.Draw_Solution1(ball, intercept, collision.point);
                intercept = Error_Correction(collision, impactpoint, intercept, this.wall1);
                count++;
            } while (count < 3);
            Vector3 Solution1_intercept = intercept;
            //path.Draw_Solution1(ball, Solution1_intercept, collision.point);
            return (ball - Solution1_intercept).normalized;
            */
        }

        public Vector3 StraightShot(Vector3 ball, Vector3 destination)
        {
            Shot_Tools path = new Shot_Tools();
            path.Draw_line(ball, destination);
            return (ball - destination).normalized;
        }

        private Vector3 RecursivePocket (Vector3 range1, Vector3 range2, Vector3 pocket, Vector3 ball)
        {
            RaycastHit collision;
            Vector3 error;
            Vector3 reflect_shot;
            Shot_Tools path = new Shot_Tools();


            Vector3 intercept = new Vector3((range1.x + range2.x) * 0.5f, this.wall1.transform.position.y, this.wall1.transform.position.z);

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

            if(!Physics.Raycast(intercept, reflect_shot, out collision));
            {
                Debug.Log("Something weird happened");
                return intercept;
            }

            if (Vector3.Distance(collision.point, pocket) < 0.5f)
            {
                return intercept;
            }
            error = collision.point - pocket;
            path.Draw_line(intercept, collision.point);

            if (Mathf.Abs(error.x) >= Mathf.Abs(error.z))
            {
                return RecursivePocket(intercept, pocket, pocket, ball);
            }
            else
            {
                return RecursivePocket(ball, intercept, pocket, ball);
            }

        }
        private Vector3 RecursiveIntercept(Vector3 left, Vector3 right, Vector3 impactpoint, Vector3 ball)
        {
            Plane intersect_plane;
            
            Ray reflect_projection;
            float enter;
            Vector3 reflect_shot;
            Shot_Tools path = new Shot_Tools();

            Vector3 intercept = new Vector3((left.x + right.x) * 0.5f, this.wall1.transform.position.y, this.wall1.transform.position.z);
            if (this.quadrant == 1 || this.quadrant == 2)
            {
                intersect_plane = new Plane(Vector3.left, impactpoint);
            }
            else
            {
                intersect_plane = new Plane(Vector3.left, impactpoint);
            }
                
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

            reflect_projection = new Ray(intercept, reflect_shot);
            if (intersect_plane.Raycast(reflect_projection, out enter))
            {
                count++;
                Debug.Log(count);
                Vector3 hitpoint = reflect_projection.GetPoint(enter);
                path.Draw_Solution1(ball, intercept, hitpoint);
                if (Vector3.Distance(hitpoint, impactpoint) < 0.5f)
                    //Debug.Log(count);
                    return intercept;
                if (count == 30)
                {
                    return intercept;
                }
                if (this.quadrant == 2 || this.quadrant == 1)
                {
                    if (hitpoint.z > impactpoint.z)
                        return RecursiveIntercept(intercept, right, impactpoint, ball);
                    else
                        return RecursiveIntercept(left, intercept, impactpoint, ball);
                }
                else
                {
                    if (hitpoint.z < impactpoint.z)
                        return RecursiveIntercept(left, intercept, impactpoint, ball);
                    else
                        return RecursiveIntercept(intercept, right, impactpoint, ball);
                }

            }
            else
            {
                Debug.Log("Missed the target");
                return intercept;
            }
            
        }
        
    }

}
