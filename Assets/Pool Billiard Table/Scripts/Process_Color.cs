using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.IO;

public class Process_Color : MonoBehaviour {
    string[] coord;
   // public Button start_Button;

    public GameObject cue;
    public GameObject black;
    public GameObject blue;
    public GameObject red;
    public GameObject yellow;
    public GameObject green;

    GameObject Wall1;
    GameObject Wall2;
    GameObject Wall3;
    GameObject Wall4;
    Collider Wall1_collider;
    Collider Wall2_collider;
    Collider Wall3_collider;
    Collider Wall4_collider;

    GameObject ball;
    // Use this for initialization
    void Start()
    {
        /*
        Button btn = start_Button.GetComponent<Button>();
        btn.onClick.AddListener(Execute);
        */
        Wall1 = GameObject.FindGameObjectWithTag("Wall_Top");
        Wall2 = GameObject.FindGameObjectWithTag("Wall_Bottom");
        Wall3 = GameObject.FindGameObjectWithTag("Wall_Right");
        Wall4 = GameObject.FindGameObjectWithTag("Wall_Left");

        Wall1_collider = Wall1.GetComponent<Collider>();
        Wall2_collider = Wall2.GetComponent<Collider>();
        Wall3_collider = Wall3.GetComponent<Collider>();
        Wall4_collider = Wall4.GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update () {
		
	}

    /*
    public void Execute_Copy()
    {
        //This section is for using a jpg file for object creation
        Process process = new Process();
        Clear();
        process.StartInfo.FileName = "Colour_Process_jpg.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Arguments = "state.jpg";
        process.Start();
        process.WaitForExit();
        string[] circles = File.ReadAllLines("colour_coordinates.txt");
        char[] split = new char[2];
        split[0] = '-';
        split[1] = ',';
        foreach (string circle in circles)
        {
            coord = circle.Split(split);
            switch (Convert.ToInt32(coord[0]))
            {
                case 1:
                    ball = cue;
                    break;
                case 2:
                    ball = black;
                    break;
                case 3:
                    ball = purple;
                    break;
                case 4:
                    ball = orange;
                    break;
            }
            Vector3 ball_transform = new Vector3(Convert.ToSingle(coord[2]), 0, -1 * Convert.ToSingle(coord[3]));
            Quaternion ball_rotation = new Quaternion(0, 0, 0, 0);
            Instantiate(ball, ball_transform, ball_rotation);
        }
    }
    */

    public void Execute()
    {
        //This section is for using a jpg file for object creation
        Start();
        Clear();
        Process process = new Process();
        process.StartInfo.FileName = "Image_Process_TF.exe";
        process.StartInfo.CreateNoWindow = true;
        //process.Start();
        //process.WaitForExit();
        string[] circles = File.ReadAllLines("colour_coordinates.txt");
        char split = ',';
        foreach (string circle in circles)
        {
            coord = circle.Split(split);
            if (coord[0] == "cueball")
                ball = cue;
            else if (coord[0] == "blueball")
                ball = blue;
            else if (coord[0] == "blackball")
                ball = black;
            else if (coord[0] == "greenball")
                ball = green;
            else if (coord[0] == "redball")
                ball = red;
            else
                ball = yellow;
            Vector3 ball_transform = new Vector3(Convert.ToSingle(coord[1]), 0, -1 * Convert.ToSingle(coord[2]));
            //if (ball_transform.x == 9)

            Collider ball_collider = ball.GetComponent<Collider>();
            if (ball_collider.bounds.Intersects(Wall1_collider.bounds) ||
                ball_collider.bounds.Intersects(Wall2_collider.bounds) ||
                ball_collider.bounds.Intersects(Wall3_collider.bounds) ||
                ball_collider.bounds.Intersects(Wall4_collider.bounds)
                )
            {
                UnityEngine.Debug.Log("Something Clipped");
                continue;
            }

            Quaternion ball_rotation = new Quaternion(0, 0, 0, 0);
            Instantiate(ball, ball_transform, ball_rotation);
        }
    }

    public void Clear()
    {
        GameObject[] Solid = GameObject.FindGameObjectsWithTag("Solid");
        GameObject[] Stripe = GameObject.FindGameObjectsWithTag("Stripe");
        GameObject[] Lines = GameObject.FindGameObjectsWithTag("Solution");

        Destroy(GameObject.FindWithTag("Cue"));
        Destroy(GameObject.FindWithTag("Black"));
        
        foreach (GameObject ball in Solid)
        {
            GameObject.Destroy(ball);
        }

        foreach (GameObject ball in Stripe)
        {
            GameObject.Destroy(ball);
        }

        foreach (GameObject line in Lines)
        {
            GameObject.Destroy(line);
        }
    }
}
