using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void LoadSceneMethod()
    {
        EventManager.instance.EndOpening();
    }


}
