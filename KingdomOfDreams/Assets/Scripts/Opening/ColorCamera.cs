using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCamera : MonoBehaviour
{
    public Transition transition;
    private float distance;

    private void Start()
    {

    }
    public void ColorCameraTransition()
    {
        this.distance = 550;
        this.transition.ScanDistance = this.distance;
    }

    public void GrayCameraTransition()
    {
        this.distance = 0;
        this.transition.ScanDistance = this.distance;
    }

    public void ColorFlower()
    {
        this.distance = 550;
        this.transition.ScanDistance = this.distance;
    }


}
