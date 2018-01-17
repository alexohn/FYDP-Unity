using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetPath
{
    public class Ricochet : MonoBehaviour
    {
        public GameObject pocket;
        public GameObject ball;
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
            if(algo_start)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit ball_select = new RaycastHit();
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ball_select))
                    {
                        if (ball_select.transform.gameObject.CompareTag("Cue"))
                        {
                            print("Now choose a pocket");
                            algo_start = false;
                            return;
                        }
                        else
                        {
                            print("What are you looking at?");
                            algo_start = false;
                        }
                    }
                }
            }

        }

        public void Ricochet_Shot()
        {
            Start();
            print("Select something");
            StartCoroutine(Mouseclick());
            

        }

        IEnumerator Mouseclick()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true);
            RaycastHit ball_select = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out ball_select))
            {
                if (ball_select.transform.gameObject.CompareTag("Cue"))
                {
                    print("You selected the cue ball");
                    algo_start = false;
                }
                else
                {
                    print("Not a proper selection?");
                    algo_start = false;
                }
            }
        }

    }

}
