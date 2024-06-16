using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDirector : MonoBehaviour
{
    public Transition transition;
    private float distance = 550;
    private float frame = 0;
    private bool isTransition = true;

    void Start()
    {
        //Debug.Log("start");
        //Debug.Log(isTransition);
        this.transition.ScanDistance = this.distance;

        //Debug.LogFormat("distance : {0}", distance);
        //Debug.LogFormat("frame : {0}", frame);

    }


    void Update()
    {
        //Debug.Log("Update");
        if (isTransition)
        {
            this.frame++;
            //Debug.LogFormat("frame : {0}", frame);
            if (this.frame >= 150)
            {
                this.distance -= (550 / 5) * Time.deltaTime;
                //Debug.LogFormat("distance : {0}", distance);
                this.transition.ScanDistance = this.distance;
                //Debug.LogFormat("ScanDistance : {0}", this.transition.ScanDistance);
                if (this.distance <= 0) this.isTransition = false;
            }

        }
    }

}

