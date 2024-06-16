using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    void Start()
    {
    }

    public void ShowEffect()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<ParticleSystem>().Play();

    }

}
