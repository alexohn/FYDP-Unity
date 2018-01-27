using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetPath
{
    public class Bounce_Shot
    {
        public int num_bounces;
        GameObject cue;
        GameObject ball;
        public Bounce_Shot(GameObject cue, GameObject ball)
        {
            this.cue = cue;
            this.ball = ball;
        }
    }

}
