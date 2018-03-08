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

        GameObject[] walls;
        GameObject cue;
        GameObject ball;
        GameObject pocket;
        RaycastHit bounce_point;
        static int count;
        static int pocketcount;
        int scenario;
        int quadrant; //find out where the start ball is relative to th end position

        public Bounce_Shot(GameObject cue, GameObject ball, GameObject pocket)
        {
            this.cue = cue;
            this.ball = ball;
            this.pocket = pocket;
            this.scenario = 0;
            this.walls = new GameObject[2];
        }

        public int DetermineCase()
        {
            Shot_Tools path = new Shot_Tools();
            Vector3 incidenttrajectory = new Vector3();
            Vector3 impactpoint = new Vector3();
            Vector3 check;
            int pocket_type = 0;
            count = 0;
            pocketcount = 0;
            

            foreach (Transform child in pocket.transform)
            {
                if (child.gameObject.CompareTag("Middle_Pocket"))
                    pocket_type = 1;
                else if (child.gameObject.CompareTag("Corner_Pocket"))
                    pocket_type = 0;
            }

            //Determine if there is an obstruction between the pocket and the ball
            if (!path.Measure_Collision(pocket.transform.position, ball.transform.position))
            {
                if (pocket_type == 0)
                    incidenttrajectory = Around_Obstruction_Pocket(this.ball.transform.position, this.pocket.transform.position);
                else
                    incidenttrajectory = Around_Obstruction_Pocket_Middle(this.ball.transform.position, this.pocket.transform.position);

            }
            else
            {
                incidenttrajectory = StraightShot(this.ball.transform.position, pocket.transform.position);
            }
       
            if (incidenttrajectory == Vector3.zero)
            {
                Debug.Log("Desired shot not possible. Please select either a different ball or a different pocket NOW");
                return 1;
            }

            impactpoint = path.Impactpoint(this.ball.transform.position, incidenttrajectory);

            //Determine if there is obstruction between required impact point and cue. 
            //if (!path.Measure_Collision_to_Impact(this.cue.transform.position, impactpoint, incidenttrajectory)) {
            if (!path.Measure_Collision_to_Impact(this.cue.transform.position, impactpoint, this.ball.transform.position, incidenttrajectory))
            {
                SelectWall(impactpoint, this.ball.transform.position);
                //There might be an issue here, because this is changing the quadrant of the shot
                check = Around_Obstruction_Ball(this.cue.transform.position, impactpoint, incidenttrajectory); 
            }
            else
            {
                check = StraightShot(this.cue.transform.position, impactpoint);
            }

            if (check == Vector3.zero)
            {
                Debug.Log("Desired shot not possible. Please select either a different ball or a different pocket NOW");
                return 1;
            }

            return 0;
            
        }
        //Determine where to bounce the ball, based on the location of all the balls

        public void SelectWall(Vector3 start, Vector3 end)
        {
            if (end.z > start.z)
            {
                this.wall1 = GameObject.FindGameObjectWithTag("Wall_Bottom");
                this.walls[0] = GameObject.FindGameObjectWithTag("Wall_Bottom");
                //If ball is in front of the pocket
                if (end.x < start.x)
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Right");
                    this.walls[1] = GameObject.FindGameObjectWithTag("Wall_Right");
                    this.quadrant = 2;
                }
                //If ball is behind the pocket
                else
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Left");
                    this.walls[1] = GameObject.FindGameObjectWithTag("Wall_Left");
                    this.quadrant = 1;
                }
            }
            //If pocket is below the ball
            else
            {
                this.wall1 = GameObject.FindGameObjectWithTag("Wall_Top");
                this.walls[0] = GameObject.FindGameObjectWithTag("Wall_Top");
                //If ball is in front of the pocket
                if (end.x > start.x)
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Left");
                    this.walls[1] = GameObject.FindGameObjectWithTag("Wall_Left");
                    this.quadrant = 0;
                }
                //If ball is behind the pocket
                else
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Right");
                    this.walls[1] = GameObject.FindGameObjectWithTag("Wall_Right");
                    this.quadrant = 3;
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


            if (!path.Measure_Collision(ball, intercept) || !path.Measure_Collision(intercept, pocket))
            {
                Debug.Log("The shot is not possible");
                return new Vector3();
            }
            else
            {
                path.Draw_Solution1(ball, intercept, pocket);
                return (ball - intercept).normalized;

            }


        }

        public Vector3 Around_Obstruction_Pocket_Middle(Vector3 ball, Vector3 pocket)
        {
            Shot_Tools path = new Shot_Tools();
            SelectWall(ball, pocket);

            //Find first solution using wall1
            Vector3 midpoint = (pocket - ball) * 0.5f + ball;
            //Determine which wall to react the impact point

            Vector3 intercept = RecursivePocket_Middle(pocket, ball, pocket, ball);

            /*
            if (!path.Measure_Collision(ball, intercept) || !path.Measure_Collision(intercept, pocket))
            {
                Debug.Log("The shot is not possible");
                return new Vector3();
            }
            */
           // else
            //{
                //path.Draw_Solution1(ball, intercept, pocket);
                return (ball - intercept).normalized;

          //  }


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

            Physics.Raycast(impactpoint, incident_trajectory, out wall);

            Vector3 solutionintercept = RecursiveIntercept(impactpoint, ball, impactpoint, ball);

            bool check = path.Measure_Collision(ball, solutionintercept);


            //There is a null exception being thrown here. Don't really know why
            //bool check2 = path.Measure_Collision(solutionintercept, impactpoint);

            /*
            if (!path.Measure_Collision(ball, solutionintercept) || !path.Measure_Collision(solutionintercept, impactpoint))
            {
                Debug.Log("The shot is not possible. Select either a different pocket or ball");
                return new Vector3();
            }
            else
            */
            {
                path.Draw_Solution1(ball, solutionintercept, impactpoint);
                return solutionintercept;
            }
                


        }
    
        public Vector3 StraightShot(Vector3 ball, Vector3 destination)
        {
            Shot_Tools path = new Shot_Tools();
            path.Draw_line(ball, destination);
            return (ball - destination).normalized;
        }

        private Vector3 RecursivePocket (Vector3 left, Vector3 right, Vector3 pocket, Vector3 ball)
        {
            RaycastHit collision1;
            RaycastHit collision2;
            Vector3 error;
            Vector3 error2;
            Vector3 wall1_reflect_shot;
            Vector3 wall2_reflect_shot;
            Shot_Tools path = new Shot_Tools();

            Vector3 wall1_intercept = new Vector3((left.x + right.x) * 0.5f, this.wall1.transform.position.y, this.wall1.transform.position.z);
            Vector3 incident_shot = (wall1_intercept - ball).normalized;
            
            //The incident angle may change depending on whether the wall is top or bottom
            if (this.wall1.CompareTag("Wall_Top"))
            {
                wall1_reflect_shot = Vector3.Reflect(incident_shot, Vector3.back * 1f);
            }
            else
            {
                wall1_reflect_shot = Vector3.Reflect(incident_shot, Vector3.forward * 1f);
            }

            Vector3 wall2_intercept = new Vector3(this.wall2.transform.position.x, this.wall2.transform.position.y, (left.z + right.z) * 0.5f);
            incident_shot = (wall2_intercept - ball).normalized;

            if (this.wall2.CompareTag("Wall_Right"))
            {
                wall2_reflect_shot = Vector3.Reflect(incident_shot, Vector3.right * 1f);
            }
            else
            {
                wall2_reflect_shot = Vector3.Reflect(incident_shot, Vector3.left * 1f);
            }


            Physics.Raycast(wall1_intercept, wall1_reflect_shot, out collision1);
            if (collision1.collider == null)
            {
                Debug.Log("Something weird happened: wall1");
                return wall1_intercept;
            }

            Physics.Raycast(wall2_intercept, wall2_reflect_shot, out collision2);
            if (collision2.collider == null)
            {
                Debug.Log("Something weird happened: wall2");
                //return wall2_intercept;
            }

            pocketcount++;
            if (collision1.collider.CompareTag("Pocket")||pocketcount == 30)
            {
                return wall1_intercept;
            }

            error = collision1.point - pocket;
            //path.Draw_line(wall1_intercept, collision1.point);

            error2 = collision2.point - pocket;
            //path.Draw_line(wall2_intercept, collision2.point);

            if (this.quadrant == 2 || this.quadrant == 3)
            {
                if (Mathf.Abs(error.x) >= Mathf.Abs(error.z))
                {

                    return RecursivePocket(left, wall1_intercept, pocket, ball);
                }
                else
                {
                    return RecursivePocket(wall1_intercept, right, pocket, ball);
                }
            }

            else
            {
                if (Mathf.Abs(error.x) >= Mathf.Abs(error.z))
                {
                    return RecursivePocket(left, wall1_intercept, pocket, ball);

                }
                else
                {
                    return RecursivePocket(wall1_intercept, right, pocket, ball);
                }
            }




        }

        private Vector3 RecursivePocket_Middle(Vector3 left, Vector3 right, Vector3 pocket, Vector3 ball)
        {
            RaycastHit collision1;
            RaycastHit collision2;
            Vector3 error;
            Vector3 error2;
            Vector3 wall1_reflect_shot;
            Vector3 wall2_reflect_shot;
            Shot_Tools path = new Shot_Tools();

            Vector3 wall1_intercept = new Vector3((left.x + right.x) * 0.5f, 0, this.wall1.transform.position.z);
            Vector3 incident_shot = (wall1_intercept - ball).normalized;

            //The incident angle may change depending on whether the wall is top or bottom
            if (this.wall1.CompareTag("Wall_Top"))
            {
                wall1_reflect_shot = Vector3.Reflect(incident_shot, Vector3.back * 1f);
            }
            else
            {
                wall1_reflect_shot = Vector3.Reflect(incident_shot, Vector3.forward * 1f);
            }

            Vector3 wall2_intercept = new Vector3(this.wall2.transform.position.x, this.wall2.transform.position.y, (left.z + right.z) * 0.5f);
            incident_shot = (wall2_intercept - ball).normalized;

            if (this.wall2.CompareTag("Wall_Right"))
            {
                wall2_reflect_shot = Vector3.Reflect(incident_shot, Vector3.right * 1f);
            }
            else
            {
                wall2_reflect_shot = Vector3.Reflect(incident_shot, Vector3.left * 1f);
            }

            if(!Physics.Raycast(wall1_intercept, wall1_reflect_shot, out collision1))
            {
                Debug.Log("Something weird happened: wall1 in middle function");
                return wall1_intercept;
            }
                


            Physics.Raycast(wall2_intercept, wall2_reflect_shot, out collision2);
            if (collision2.collider == null)
            {
                // Debug.Log("Something weird happened: wall2");
                //return wall2_intercept;
            }

            path.Draw_Solution1(ball, wall1_intercept, collision1.point);

            pocketcount++;
            if (collision1.collider.CompareTag("Pocket") || pocketcount == 30)
            {
                return wall1_intercept;
            }

            error = collision1.point - pocket;
            //path.Draw_line(wall1_intercept, collision1.point);

            error2 = collision2.point - pocket;

            if (this.quadrant == 1 || this.quadrant == 0)
            {
                if (error.x < 0)
                {
                    return RecursivePocket_Middle(left, wall1_intercept, pocket, ball);
                }
                else
                {
                    return RecursivePocket_Middle(wall1_intercept, right, pocket, ball);
                }
            }
            else
            {
                if (error.x >= 0)
                {
                    return RecursivePocket_Middle(left, wall1_intercept, pocket, ball);
                }
                else
                {
                    return RecursivePocket_Middle(wall1_intercept, right, pocket, ball);
                }
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
                //Debug.Log(count);
                Vector3 hitpoint = reflect_projection.GetPoint(enter);
                //path.Draw_Solution1(ball, intercept, hitpoint);
                if (Vector3.Distance(hitpoint, impactpoint) < 0.5f)
                    //Debug.Log(count);
                    return intercept;
                if (count == 30)
                {
                    return intercept;
                }
                if (this.quadrant == 2 || this.quadrant == 3)
                {
                    if(this.wall1.CompareTag("Wall_Top"))
                    {
                        if (hitpoint.z > impactpoint.z)
                            return RecursiveIntercept(intercept, right, impactpoint, ball);
                        else
                            return RecursiveIntercept(left, intercept, impactpoint, ball);
                    }
                    else
                    {
                        if (hitpoint.z > impactpoint.z)
                            return RecursiveIntercept(left, intercept, impactpoint, ball);
                        else
                            return RecursiveIntercept(intercept, right, impactpoint, ball);
                    }

                }
                else
                {
                    if (this.wall1.CompareTag("Wall_Top"))
                    {
                        if (hitpoint.z > impactpoint.z)
                            return RecursiveIntercept(intercept, right, impactpoint, ball);
                        else
                            return RecursiveIntercept(left, intercept, impactpoint, ball);
                    }
                    else
                    {
                        if (hitpoint.z > impactpoint.z)
                            return RecursiveIntercept(left, intercept, impactpoint, ball);
                        else
                            return RecursiveIntercept(intercept, right, impactpoint, ball);
                    }
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
