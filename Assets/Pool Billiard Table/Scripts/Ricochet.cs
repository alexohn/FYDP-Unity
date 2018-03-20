using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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
        public Canvas popup;


        // Use this for initialization, hides game object
        void Start()
        {
            cue = GameObject.FindGameObjectWithTag("Cue");
            popup = popup.GetComponent<Canvas>();
            popup.enabled = false;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Ricochet_Shot()
        {
            Start();
            Clear();
            StartCoroutine(Select_Shot());
        }

        IEnumerator Select_Shot()
        {
            Shot_Tools path = new Shot_Tools();
            bool flag = true;
            int run = 1;
            popup.enabled = true;
            Transform child = popup.transform.Find("Text");
            Text t = child.GetComponent<Text>();

            while (run != 0)
            {
                Clear();
                Mouse_Select ball = new Mouse_Select(this, BallSelect());
                yield return ball.coroutine;
                Mouse_Select pocket = new Mouse_Select(this, PocketSelect());
                yield return pocket.coroutine;

                t.text = "Calculating Shot....";
                Bounce_Shot bounce = new Bounce_Shot(cue, bounce_ball, bounce_pocket);
                run = bounce.DetermineCase();
                if (run == 1)
                {
                    t.text = "The shot is not possible. Please select a different ball or different pocket";
                    //Console.Readline();
                    //System.Threading.Thread.Sleep(3000);
                }


            }

        }

        IEnumerator PocketSelect()
        {
            RaycastHit pocket_select = new RaycastHit();
            //popup.enabled = true;
            Transform child = popup.transform.Find("Text");
            Text t = child.GetComponent<Text>();
            while (true)
            {
                //print("Select a pocket");
                t.text = "Select a pocket";
                yield return new WaitUntil(() => Input.GetMouseButton(0) == true);

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out pocket_select))
                {
                    //Debug.Log(pocket_select.transform.gameObject.tag);
                    /*
                    foreach (Transform child in pocket_select.transform)
                    {
                        if (child.gameObject.CompareTag("Middle_Pocket") || child.gameObject.CompareTag("Corner_Pocket"))
                        assignedchild = child.gameObject;
                    }
                    */

                    if (!pocket_select.transform.gameObject.CompareTag("Pocket"))
                    {
                        //print("Improper Pocket seleciton. Try again");
                        t.text = "Improper Pocket seleciton. Try again";
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
            //popup.enabled = false;
            yield return (pocket_select.transform.gameObject.transform.position);

        }

        IEnumerator BallSelect()
        {
            RaycastHit ball_select = new RaycastHit();
            //popup.enabled = true;
            Transform child = popup.transform.Find("Text");
            Text t = child.GetComponent<Text>();

            while (true)
            {
                //print("Select a ball");
                t.text = "Select a ball";
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true);

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ball_select))
                {
                    if (!ball_select.transform.gameObject.CompareTag("Solid"))
                    {
                        Debug.Log(ball_select.transform.gameObject.tag);
                        //print("Improper Ball Selection. Try again");
                        t.text = "Improper Ball seleciton. Try again";
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
            //popup.enabled = false;
            yield return (ball_select.transform.gameObject.transform.position);
        }

        public void Clear()
        {
            GameObject[] Lines = GameObject.FindGameObjectsWithTag("Solution");

            foreach (GameObject line in Lines)
            {
                GameObject.Destroy(line);
            }
        }
    }

}
