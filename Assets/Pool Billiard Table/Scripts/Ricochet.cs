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

                if (path.Measure_Collision(cue.transform.position, (Vector3)ball.result) || path.Measure_Collision((Vector3)ball.result, (Vector3)pocket.result))
                {
                    if (path.Calculate_Angle_toPocket((Vector3)pocket.result, (Vector3)ball.result, cue.transform.position))
                    {
                        path.Draw_Path(cue.transform.position, (Vector3)ball.result, (Vector3)pocket.result);
                        flag = false;
                        Debug.Log((Vector3)pocket.result);
                        Debug.Log((Vector3)ball.result);
                        //Debug.Log("Take the shot!");
                        
                    }
                    else{
                        Debug.Log("Attempting a bounce shot");
                        Bounce_Shot bounce = new Bounce_Shot(cue, bounce_ball, bounce_pocket);
                        Vector3 incident = bounce.Around_Obstruction_Pocket(bounce_ball.transform.position, bounce_pocket.transform.position);
                        bounce.Obstruction_to_Ball(incident);
                        //Debug.Log("This shot is not possible. Please select another ball and pocket");
                    }
                }
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
