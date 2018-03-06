using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace TargetPath
{
    public class Ricochet : MonoBehaviour
    {
        GameObject bounce_pocket;
        GameObject bounce_ball;
        GameObject cue;
        RaycastHit collision;
        double best_weight = 100;
        bool algo_start = false;
        bool cue_select = false;


        // Use this for initialization
        void Start()
        {
            cue = GameObject.FindGameObjectWithTag("Cue");


        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Ricochet_Shot()
        {
            Start();
            StartCoroutine(Select_Shot());
        }

        IEnumerator Select_Shot()
        {
            Shot_Tools path = new Shot_Tools();
            bool flag = true;
            
            while(flag)
            {
                Mouse_Select ball = new Mouse_Select(this, BallSelect());
                yield return ball.coroutine;
                Mouse_Select pocket = new Mouse_Select(this, PocketSelect());
                yield return pocket.coroutine;

                System.DateTime start = System.DateTime.Now;
                Debug.Log("Attempting a bounce shot");
                start = System.DateTime.Now;
                Bounce_Shot bounce = new Bounce_Shot(cue, bounce_ball, bounce_pocket);
                bounce.DetermineCase();
                System.DateTime end = System.DateTime.Now;
                System.TimeSpan duration = end - start;
                Debug.Log(duration);

                /*
                if (path.Measure_Collision(cue.transform.position, (Vector3)ball.result) || path.Measure_Collision((Vector3)ball.result, (Vector3)pocket.result))
                {
                    if (path.Calculate_Angle_toPocket((Vector3)pocket.result, (Vector3)ball.result, cue.transform.position))
                    {
                        path.Draw_Solution1(cue.transform.position, (Vector3)ball.result, (Vector3)pocket.result);
                        flag = false;
                        System.DateTime end = System.DateTime.Now;
                        System.TimeSpan duration = end - start;
                        Debug.Log(duration);
                        //Debug.Log("Take the shot!");

                    }
                    else{
                        Debug.Log("Attempting a bounce shot");
                        start = System.DateTime.Now;
                        Bounce_Shot bounce = new Bounce_Shot(cue, bounce_ball, bounce_pocket);
                        bounce.DetermineCase();
                        System.DateTime end = System.DateTime.Now;
                        System.TimeSpan duration = end - start;
                        Debug.Log(duration);
                        path.ScreenShot();
                        flag = false;
                    }
                }
                */
            }
            
        }

        IEnumerator PocketSelect()
        {
            RaycastHit pocket_select = new RaycastHit();
            while (true)
            {
                print("Select a pocket");
                yield return new WaitUntil(() => Input.GetMouseButton(0) == true);

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out pocket_select))
                {
                    if (!pocket_select.transform.gameObject.CompareTag("Pocket"))
                    {
                        print("Improper Pocket seleciton. Try again");
                        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) == true);
                        continue;
                    }
                    else
                    {
                        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) == true);
                        break;
                    }
                }
            }
            this.bounce_pocket = pocket_select.transform.gameObject;
            yield return (pocket_select.transform.gameObject.transform.position);
        }
        
        IEnumerator BallSelect()
        {
            RaycastHit ball_select = new RaycastHit();
            while (true)
            {
                print("Select a ball");
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true);
                
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ball_select))
                {
                    if (!ball_select.transform.gameObject.CompareTag("Solid") && !ball_select.transform.gameObject.CompareTag("Stripe"))
                    {
                        print("Improper Ball Selection. Try again");
                        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) == true);
                        continue;
                    }
                    else
                    {
                        yield return new WaitUntil(() => Input.GetMouseButtonUp(0) == true);
                        break;
                    }
                }
            }
            this.bounce_ball = ball_select.transform.gameObject;
            yield return (ball_select.transform.gameObject.transform.position);
        }
    }

}
