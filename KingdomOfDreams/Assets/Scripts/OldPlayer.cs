using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayer : MonoBehaviour
{
    public VariableJoystick joy;

    public Coroutine routine;

    //플레이어 기본 정보
    public Enums.ePlayerLocation location;
    public Enums.ePlayerState state;
    private Animator anim;
    public bool isPlayer;
    //이동
    private Vector3 dir;
    private Vector3 dir1;
    private float speed = 3f;
    private float rotateSpeed = 8f;
    public bool canMove;
    //행동
    public bool canAction;
    public float distance;
    private float sight = 60;
    private Vector3 targetDir;

    public GameObject target;


    public void Init()
    {
        var character = this.transform.GetChild(InfoManager.instance.PlayerInfo.nowCharacterId).gameObject;
        character.SetActive(true);
        this.anim = character.GetComponent<Animator>();
    }
    void Update()
    {
        //*this.distance = Vector3.Distance(this.transform.position, monster.transform.position);
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        dir = Vector3.Normalize(new Vector3(h, 0, v));

        //조이스틱 움직임
        var h1 = joy.Horizontal;
        var v1 = joy.Vertical;
        dir1 = Vector3.Normalize(new Vector3(h1, 0, v1));

        this.canAction = CanAction();
        //타겟의 방향
        //*this.targetDir = (this.monster.transform.position - this.transform.position).normalized;

        //DrawExtension.DrawArrow.ForDebug(this.transform.position, targetDir.normalized, 0f, Color.cyan, DrawExtension.ArrowType.Solid);
    }
    private void FixedUpdate()
    {
        Move();
        //MoveJoy();
    }
    private void MoveJoy()
    {
        if (this.state != Enums.ePlayerState.Attacking
            && this.state != Enums.ePlayerState.Farming)
        {

            if (dir1 != Vector3.zero)
            {
                if (Mathf.Sign(dir1.x) != Mathf.Sin(transform.position.x) || Mathf.Sign(dir1.z) != Mathf.Sin(transform.position.z))
                {
                    transform.Rotate(0, 1, 0);
                }
                transform.forward = Vector3.Lerp(transform.forward, dir1, Time.deltaTime * rotateSpeed);
                this.anim.SetInteger("State", 1);
                this.transform.Translate(this.transform.forward * this.speed * Time.deltaTime, Space.World);
                this.state = Enums.ePlayerState.Run;
                //Debug.Log("run");
            }
            else if (dir1 == Vector3.zero)
            {
                this.anim.SetInteger("State", 0);
                this.state = Enums.ePlayerState.Idle;
                //Debug.Log("idle");
            }
        }
    }
    private void Move()
    {
        if (this.state != Enums.ePlayerState.Attacking
            && this.state != Enums.ePlayerState.Farming)
        {
            if (dir != Vector3.zero)
            {
                if (Mathf.Sign(dir.x) != Mathf.Sin(transform.position.x) || Mathf.Sign(dir.z) != Mathf.Sin(transform.position.z))
                {
                    transform.Rotate(0, 1, 0);
                }
                transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotateSpeed);
                this.anim.SetInteger("State", 1);
                this.transform.Translate(this.transform.forward * this.speed * Time.deltaTime, Space.World);
                this.state = Enums.ePlayerState.Run;
                //Debug.Log("run");
            }
            else if (dir == Vector3.zero)
            {
                this.anim.SetInteger("State", 0);
                this.state = Enums.ePlayerState.Idle;
                //Debug.Log("idle");
            }
        }
    }
    private bool CanAction()
    {
        //if (distance <= 3.5f)
        //    this.location = Enums.ePlayerLocation.Dungeon;
        //else
        //    this.location = Enums.ePlayerLocation.None;

        //if (this.monster.state == Enums.eMonsterState.Die) return false;

        bool result = false;
        this.distance = Vector3.Distance(this.transform.position, target.transform.position);

        float dot = Vector3.Dot(this.transform.forward, targetDir);

        //내적을 이용한 각 계산하기
        // thetha = cos^-1( a dot b / |a||b|)
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        //Debug.Log("타겟과 AI의 각도 : " + theta);

        //if (theta > sight || this.distance > this.attackRange)
        //{
        //    Debug.LogFormat("몬스터가 공격 범위에 없음 theta:{0}, distacne:{1}", theta, this.distance);
        //    result = false;
        //}
        //else result = true;
        if (theta > sight || this.distance < 2.5f) result = true;
        else result = false;

        return result;
    }
    public void FellingStart(FellingTree tree)      //벌목 시작
    {
        //벌목
        //if (tree.treeHP == 0)
        //{
        //    Debug.Log("쓰러진 나무");
        //}
        //else
        //{
        //    //도끼
        //    this.transform.LookAt(tree.transform.position);
        //    Debug.LogFormat("벌목");
        //    this.routine = StartCoroutine(Felling(tree));
        //}
    }
    private IEnumerator Felling(FellingTree tree)
    {
        this.anim.SetInteger("State", 4);
        this.canMove = false;

        yield return new WaitForSeconds(1f);    //벌목 애니메이션

        //this.onGetWood(tree);   //Main에 알림

        yield return new WaitForSeconds(0.25f); //Idle전환

        this.canMove = true;
        this.routine = null;
    }
    //public IEnumerator CorTakeEquipment()
    //{
    //    Debug.Log("장비");
    //    if (this.location == Enums.ePlayerLocation.Dungeon && !isTakeWeapon)
    //    {
    //        Debug.Log("무기 들기");
    //        this.isTakeWeapon = true;
    //        this.equipGo = Instantiate(this.swordPrefab, this.swordPos);
    //        this.addAnimNum = 10;
    //        if (this.state == Enums.ePlayerState.Run)
    //            this.anim.SetInteger("State", addAnimNum + 1);
    //        if (this.state == Enums.ePlayerState.Idle)
    //            this.anim.SetInteger("State", addAnimNum);
    //    }
    //    if (this.location == Enums.ePlayerLocation.Farm && !isTakeShovel)
    //    {
    //        Debug.Log("삽 들기");
    //        this.isTakeShovel = true;
    //        this.equipGo = Instantiate(this.shovelPrefab, this.shovelTrans);
    //        this.addAnimNum = 20;
    //        if (this.state == Enums.ePlayerState.Run)
    //            this.anim.SetInteger("State", addAnimNum + 1);
    //        if (this.state == Enums.ePlayerState.Idle)
    //            this.anim.SetInteger("State", addAnimNum);
    //    }
    //    yield break;
    //}

    //private IEnumerator CorAttack()
    //{
    //    var prevState = this.state;
    //    this.state = Enums.ePlayerState.Attacking;
    //    this.anim.SetInteger("State", 2);
    //    Debug.Log("공격");
    //    yield return new WaitForSeconds(1.055f);

    //    if (this.attackingTarget != null)
    //        this.onAttacking();

    //    if (prevState == Enums.ePlayerState.Run)
    //    {
    //        this.anim.SetInteger("State", 1);
    //        this.state = Enums.ePlayerState.Run;
    //    }
    //    else if (prevState == Enums.ePlayerState.Idle)
    //    {
    //        this.anim.SetInteger("State", 0);
    //        this.state = Enums.ePlayerState.Idle;
    //    }
    //}

    //public void SelectInteraction()
    //{
    //    Debug.LogFormat("cur location:{0}", this.location);
    //    if (this.location == Enums.ePlayerLocation.Dungeon
    //        && this.state != Enums.ePlayerState.Attacking)
    //    {
    //        if (this.routine != null) StopCoroutine(this.routine);
    //        this.routine = StartCoroutine(this.CorAttack());
    //    }
    //    if (this.location == Enums.ePlayerLocation.Farm
    //        && this.state != Enums.ePlayerState.Farming)
    //    {
    //        if (this.routine != null) StopCoroutine(this.routine);
    //        this.routine = StartCoroutine(this.CorFarming());
    //    }
    //}
}
