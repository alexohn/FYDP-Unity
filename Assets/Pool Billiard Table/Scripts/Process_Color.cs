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
    public GameObject purple;
    public GameObject orange;

    GameObject ball;
    // Use this for initialization
    void Start()
    {
        /*
        Button btn = start_Button.GetComponent<Button>();
        btn.onClick.AddListener(Execute);
        */
    }
    // Update is called once per frame
    void Update () {
		
	}

    public void Execute()
    {
        //This section is for using a jpg file for object creation
        Process process = new Process();
        process.StartInfo.FileName = "Image_Process.exe";
        process.StartInfo.CreateNoWindow = true;
        //process.StartInfo.Arguments = "state.jpg";
        process.Start();
        process.WaitForExit();
        Clear();
        string[] circles = File.ReadAllLines("colour_coordinates.txt");
        /*
        char[] split = new char[1];
        split[0] = '-';
        split[1] = ',';
        */
        char split = ',';
        foreach (string circle in circles)
        {
            coord = circle.Split(split);
            if (coord[0] == "white")
                ball = cue;
            if (coord[0] == "black")
                ball = black;
            if (coord[0] == "yellow")
                ball = orange;
            if (coord[0] == "purple")
                ball = purple;
            //if (coord[0] == "blue")
                //ball = blue;

            /*
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
            */
            Vector3 ball_transform = new Vector3(Convert.ToSingle(coord[1]), 0, -1 * Convert.ToSingle(coord[2]));
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
