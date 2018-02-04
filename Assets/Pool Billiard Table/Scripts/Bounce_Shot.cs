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

        public void SelectWall()
        {
            if (this.pocket.transform.position.z > this.ball.transform.position.z)
            {
                this.wall1 = GameObject.FindGameObjectWithTag("Wall_Bottom");
                //If ball is in front of the pocket
                if (this.pocket.transform.position.x < this.ball.transform.position.x)
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Right");
                }
                //If ball is behind the pocket
                else
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Left");
                }
            }
            //If pocket is below the ball
            else
            {
                this.wall1 = GameObject.FindGameObjectWithTag("Wall_Top");
                //If ball is in front of the pocket
                if (this.pocket.transform.position.x < this.ball.transform.position.x)
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Right");
                }
                //If ball is behind the pocket
                else
                {
                    this.wall2 = GameObject.FindGameObjectWithTag("Wall_Left");
                }
            }
        }

        public bool Obstruction_to_Pocket()
        {
            Shot_Tools path = new Shot_Tools();
            float incident_angle;

            //Initial intercept point
            Vector3 midpoint = (this.pocket.transform.position - this.ball.transform.position) * 0.5f + this.ball.transform.position;
            //First find the intercept using wall1 (either the top or bottom wall)
            Vector3 intercept = new Vector3(midpoint.x, wall1.transform.position.y, wall1.transform.position.z);

            Vector3 start_trajectory = (ball.transform.position - intercept).normalized;
            //The incident angle may change depending on whether the wall is top or bottom
            if (this.wall1.CompareTag("Wall_Top")){
                incident_angle = Vector3.Angle(Vector3.back, start_trajectory);
            }
            else
            {
                incident_angle = Vector3.Angle(Vector3.forward, start_trajectory);
            }
            Debug.Log(incident_angle);
            


            path.Draw_Path(cue.transform.position, ball.transform.position, intercept);
            //Debug.Log(midpoint);
            

            return true;
        }

        public bool Obstruction_to_Ball()
        {
            return true;
        }
        
        public bool Improper_Angle()
        {
            return false;

        }
        
    }

}
