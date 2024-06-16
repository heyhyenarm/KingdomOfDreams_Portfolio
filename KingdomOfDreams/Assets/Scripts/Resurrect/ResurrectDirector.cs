using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ResurrectDirector : MonoBehaviour
{
    public Transition transition;
    public PostProcessVolume post;
    private float distance = 0;
    private float frame = 0;
    private bool isTransition = true;

    void Start()
    {
        this.transition.ScanDistance = this.distance;
        this.post.weight = 0;
    }


    void Update()
    {
        if (isTransition)
        {
            this.frame++;
            //Debug.LogFormat("frame : {0}", frame);
            if (this.frame >= 0)
            {
                this.distance += 10 * Time.deltaTime;
                //Debug.LogFormat("distance : {0}", distance);
                this.transition.ScanDistance = this.distance;
                //Debug.LogFormat("ScanDistance : {0}", this.transition.ScanDistance);
                if(this.post.weight<=1) this.post.weight = this.distance / 20;
                if (this.distance >= 40) this.distance += 500 * Time.deltaTime;
                if (this.distance >= 550) this.isTransition = false;
            }

        }
    }

}

