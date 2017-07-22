using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;
using System.IO;
//using System.IO.Pipes;
using System;

public class Image_process_Script : MonoBehaviour {
    string[] coord;
    public GameObject ball;
	// Use this for initialization
	void Start ()
    {

        //This section is for using a jpg file for object creation
        Process process = new Process();
        process.StartInfo.FileName = "Colour_Process_jpg.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Arguments = "state.jpg";
        process.Start();
        process.WaitForExit();
        string[] circles = File.ReadAllLines("colour_coordinates.txt");

        char[] split = new char[2];
        split[0] = '-';
        split[1] = ',';
        foreach(string circle in circles)
        {
            coord = circle.Split(split);
            //Uncomment this line when you want to use the actual circle coordinates
            Vector3 ball_transform = new Vector3(Convert.ToSingle(coord[1]), (float) 42.5939, Convert.ToSingle(coord[2]));
            //Vector3 ball_transform = new Vector3((float) -3, (float)42.5939, (float) -0.661);
            Quaternion ball_rotation = new Quaternion(0, 0, 0,0);
            Instantiate(ball, ball_transform, ball_rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
