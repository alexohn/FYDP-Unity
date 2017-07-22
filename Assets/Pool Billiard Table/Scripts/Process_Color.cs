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
    
}
