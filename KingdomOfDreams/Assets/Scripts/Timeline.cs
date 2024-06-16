using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public PlayableDirector director;

    void Start()
    {
        
    }

    public void Repeat()
    {
        this.director.Play();
    }
}
