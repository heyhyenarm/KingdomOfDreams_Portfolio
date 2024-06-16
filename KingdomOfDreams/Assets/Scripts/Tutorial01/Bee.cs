using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    public Transform targetPos;

    private NavMeshAgent nav;
    private Rigidbody rBody;
    private Animator anim;
    private bool isAttack = false;
    private AudioSource AS;

    void Awake()
    {
        Debug.LogFormat("Awake");
        this.nav = GetComponent<NavMeshAgent>();
        this.rBody = GetComponent<Rigidbody>();
        this.anim = GetComponent<Animator>();
        StartCoroutine(DisappearBee());
        this.AS = SoundManager.PlayCappedSFX("41497941_bees-buzzing-01", "Nature");
    }

    void Update()
    {
        if (this.targetPos != null)
        {
            this.nav.enabled = true;
            this.nav.SetDestination(this.targetPos.position);
        }
    }
    private void FixedUpdate()
    {
        FreezeVelocity();
    }
    private void FreezeVelocity()
    {
        this.rBody.velocity = Vector3.zero;
        this.rBody.angularVelocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!this.isAttack)
            {
                this.anim.SetInteger("State", 1);
                StartCoroutine(WaitAttack());
                this.isAttack = true;
            }
        }
    }
    IEnumerator WaitAttack()
    {
        this.targetPos = null;
        yield return new WaitForSeconds(0.93f);

        Debug.LogFormat("벌 공격");
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.ATTACKED_BEE);  //플레이어 머리 커짐

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }
    IEnumerator DisappearBee()
    {
        yield return new WaitForSeconds(10f);

        Destroy(this.gameObject);
    }
    private void OnDisable()
    {
        SoundManager.CrossOut(1f, this.AS);
    }
}
