using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private bool isPickUp;
    private bool canPickUp;
    private Vector3 dir;
    private float velocity;
    private float acceleration;
    private Transform playerTrans;
    private void Awake()
    {
        this.isPickUp = false;
        this.canPickUp = false;

        Invoke("CanPickUp", 0.75f);
    }
    private void Update()
    {
        if (this.canPickUp && this.playerTrans != null)
        {
            ItemMove();
        }
        if(this.isPickUp)
        {
            this.transform.position = new Vector3(this.transform.position.x + (this.dir.x * this.velocity),
                                                  this.transform.position.y + (this.dir.y * this.velocity),
                                                  this.transform.position.z + (this.dir.z * this.velocity));
            this.gameObject.transform.localScale -= this.gameObject.transform.localScale / 10;
        }
    }
    private void CanPickUp()
    {
        this.canPickUp = true;
    }
    public void ItemMove()
    {
        var playerPos = new Vector3(this.playerTrans.position.x, this.playerTrans.position.y + 0.8f, this.playerTrans.position.z);
        this.dir = (playerPos - this.transform.position).normalized;

        this.acceleration = 1f;
        this.velocity += this.acceleration * Time.deltaTime;
        float distance = Vector3.Distance(playerPos, this.transform.position);

        if(!this.isPickUp)
        {
            this.velocity = 0f;
            if(distance <= 2.5f)
            {
                this.isPickUp = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            this.playerTrans = other.gameObject.transform;
        }
    }
}
