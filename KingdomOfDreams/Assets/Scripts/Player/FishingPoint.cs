using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoint : MonoBehaviour
{
    public GameObject fish;
    public System.Action<GameObject> onFish;
    private void OnTriggerEnter(Collider other)
    {
        //this.onFish(other.gameObject);
        if (other.CompareTag("Fish"))
        {
            this.fish = other.gameObject;
        }
    }
    //
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            this.fish = null;
        }
    }
}
