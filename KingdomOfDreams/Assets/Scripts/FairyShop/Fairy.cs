using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fairy : MonoBehaviour
{
    public Canvas fairyDirector;

    void Start()
    {
        this.fairyDirector.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player on FairyZone");
            this.fairyDirector.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player on FairyZone");
            this.fairyDirector.gameObject.SetActive(false);
        }
    }
}
