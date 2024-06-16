using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    Queue<DreamlandParticle> q = new Queue<DreamlandParticle>();
    public static ObjectPooling Instance;
    [SerializeField]
    private GameObject particlePrecab;

    void Awake()
    {
        Instance = this;
        Initialize(10);
    }
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
            q.Enqueue(CreateNewObject());
    }
    private DreamlandParticle CreateNewObject()
    {
        var particle = Instantiate(particlePrecab).GetComponent<DreamlandParticle>();
        particle.gameObject.SetActive(false);
        particle.transform.SetParent(transform);
        return particle;
    }
    public static DreamlandParticle GetObject()
    {
        DreamlandParticle obj = null;
        if (Instance.q.Count > 0)
        {
            var particle = Instance.q.Dequeue();
            particle.transform.SetParent(null);
            particle.gameObject.SetActive(true);
            obj = particle;
        }
        else
        {
            var newParticle = Instance.CreateNewObject();
            newParticle.gameObject.SetActive(true);
            newParticle.transform.SetParent(null);
            obj = newParticle;
        }
        return obj;
    }
    public static void ReturnObject(DreamlandParticle particle)
    {
        particle.gameObject.SetActive(false);
        particle.transform.SetParent(Instance.transform);
        Instance.q.Enqueue(particle);
    }
}