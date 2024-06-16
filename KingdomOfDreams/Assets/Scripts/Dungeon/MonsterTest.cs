using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterTest : MonoBehaviour
{
    //public HSYEnum.eMonsterState state;
    // public float hp;
    private PlayerMono player;
    public int transNum;
    private Animator anim;
    public GameObject character;
    private Renderer monsterRenderer;

    public System.Action onHit;
    public System.Action onDie;
    public Enums.eMonsterState state;
    private Enums.eMonsterState preState;
    public Color baseColor;

    public GameObject meatPrefab;

    private NavMeshAgent agent;
    private float attackDistance = 1.3f;
    private float cognitiveDistance = 5f;
    private float distance;
    private Rigidbody rbody;
    private float knockbackForce = 10f;
    private bool canAttack;

    private float hitTime = 0.467f - 0.25f;
    private float dieTime = 1.333f - 0.25f;
    private Coroutine routine;

    //꿈조각 생성
    private int stageNum;
    private int myLevel;
    public GameObject[] dreamPiecesPrefab;
    private GameObject dreamPiece;
    public GameObject magicPiecePrefab;
    private GameObject magicPiece;

    public GameObject[] bloodEffects;

    public void Init()
    {
        //Debug.LogFormat("<color=cyan>monster Init</color>");

        //Debug.LogFormat("player:{0}", this.player);
        this.player = GameObject.Find("Player(Clone)").GetComponent<PlayerMono>();
        this.rbody = this.player.GetComponent<Rigidbody>();

        this.canAttack = true;
        this.state = Enums.eMonsterState.Idle;
        this.character = transform.Find("Character_Goblin_Male").gameObject;
        this.monsterRenderer = this.character.GetComponent<Renderer>();
        this.baseColor = this.monsterRenderer.material.color;

        this.anim = this.GetComponent<Animator>();

        agent = this.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = this.attackDistance;
        distance = Vector3.Distance(this.transform.position, this.player.transform.position);

        this.routine = StartCoroutine(this.coScout());
    }

    private void Update()
    {
        //Debug.LogFormat("player state:{0}",this.player.state);
        if (this.player != null)
            this.distance = Vector3.Distance(this.player.transform.position, this.transform.position);

        if (this.player != null && this.player.state == Enums.ePlayerState.Attacking_Stun)
            this.canAttack = false;

        //this.bloodEffects[0].transform.LookAt(Camera.main.transform);
        //this.bloodEffects[1].transform.LookAt(Camera.main.transform);


        if (this.distance > this.cognitiveDistance)
        {
            int selectAction = Random.Range(1, 3);
            float waitTime = Random.Range(1f, 2f);

            if (selectAction == 1)
                this.ResetPath();
            else
                this.SetRandomDestination();
        }

        if (this.state == Enums.eMonsterState.Idle)
        {
            this.state = Enums.eMonsterState.None;
            this.anim.SetInteger("State", 0);
        }
        if(this.state==Enums.eMonsterState.Walk)
        {
            this.state = Enums.eMonsterState.None;
            this.anim.SetInteger("State", 1);
        }


        if (this.state == Enums.eMonsterState.Track)
        {
            this.state = Enums.eMonsterState.None;
            StartCoroutine(this.coScout());
        }
        if (this.state == Enums.eMonsterState.Attack)
        {

        }
        if (this.state == Enums.eMonsterState.Hit)
        {

        }
        if (this.state == Enums.eMonsterState.Die)
        {

        }
    }

    public IEnumerator CoHit()
    {
        if (this.routine != null)
            StopCoroutine(this.routine);
        this.state = Enums.eMonsterState.Hit;
        //this.hp -= 4;
        //StartCoroutine(this.CoChangeColor());
        this.bloodEffects[0].SetActive(true);
        this.bloodEffects[1].SetActive(true);
        this.bloodEffects[0].GetComponent<ParticleSystem>().Play();
        this.bloodEffects[1].GetComponent<ParticleSystem>().Play();
        this.anim.SetInteger("State", 3);
        yield return YieldCache.WaitForSeconds(this.hitTime);

        this.bloodEffects[0].SetActive(false);
        this.bloodEffects[1].SetActive(false);

        StartCoroutine(this.CoDie());
        yield break;
    }

    private IEnumerator CoChangeColor()
    {
        while (this.monsterRenderer.material.color.r < 0.7f)
        {
            //Debug.LogFormat("몬스터 맞아서 빨개짐 {0}", this.monsterRenderer.material.color.r);
            this.monsterRenderer.material.color += new Color(0.01f, 0, 0);
            yield return null;
        }

        this.monsterRenderer.material.color = baseColor;
        yield break;
    }

    public IEnumerator CoDie()
    {
        this.state = Enums.eMonsterState.Die;
        this.anim.SetInteger("State", 4);
        yield return YieldCache.WaitForSeconds(this.dieTime);

        Color color = this.monsterRenderer.material.color;
        float deltaAlpha = color.a;
        while (deltaAlpha >= 0)
        {
            if (deltaAlpha < 0.2f)
            {
                deltaAlpha = 0;
                this.monsterRenderer.material.color = new Color(color.r, color.g, color.b, deltaAlpha);
                break;
            }
            //Debug.LogFormat("alpha:{0}, deltaAlpha{1}", color.a, deltaAlpha);
            deltaAlpha -= 0.05f;
            this.monsterRenderer.material.color = new Color(color.r, color.g, color.b, deltaAlpha);
            yield return null;
        }

        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.MONSTER_DIE, this);

        //this.onDie();
        //Destroy(this.gameObject);
        yield break;
    }

    public void DropMeat()
    {
        var meatGo = Instantiate(this.meatPrefab);
        meatGo.transform.position = (this.transform.position + new Vector3(0, 1, 0));

    }
    public void DropPiece()
    {
        //Debug.Log("꿈 조각 생성");
        int randomValue = Random.Range(1, 101);
        //Debug.LogFormat("randomValue:{0}", randomValue);


        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

        int stageNum = 0;

        if (myLevel >= 1) //마법도구가 있을때
        {
            if (randomValue <= 10f) // 10%의 확률로 실행
            {
                dreamPiece = Instantiate(dreamPiecesPrefab[Random.Range(0, dreamPiecesPrefab.Length)]);
                dreamPiece.transform.position = this.transform.position;
            }
        }

        if (myLevel == 0)  //마법도구가 없을때
        {
            //Debug.Log("마법 도구 없을 때 꿈 조각 생성");

            foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
            {
                //Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
                if (stageInfo.isClear == false)
                {
                    stageNum = stageInfo.stage;
                    break;
                }
            }

            if (stageNum <= 4) //5스테이지 이하에서는 실행 안함
            {

            }
            else if (stageNum >= 5)//6스테이지부터
            {
                if (randomValue <= 10f) // 10% 확률로 실행
                {
                    //Debug.Log("진짜로 꿈 조각 생성");

                    magicPiece = Instantiate(magicPiecePrefab);
                    magicPiece.transform.position = this.transform.position;
                }
            }
        }
    }

    private IEnumerator coScout()
    {
        while (true)
        {
            distance = Vector3.Distance(this.transform.position, this.player.transform.position);
            //Debug.LogFormat("player state:{0}", this.player.state);
            //Debug.LogFormat("distance:{0}", this.distance);
            if (this.distance > this.cognitiveDistance)
            {
                int selectAction = Random.Range(1, 3);
                float waitTime = Random.Range(1f, 2f);

                if (selectAction == 1)
                    this.ResetPath();
                else
                    this.SetRandomDestination();

                yield return YieldCache.WaitForSeconds(waitTime);
            }
            if (this.distance <= this.cognitiveDistance && this.distance > this.agent.stoppingDistance)
            {
                this.TrackPlayer();
                yield return YieldCache.WaitForSeconds(0.5f);
            }
            if (distance <= this.attackDistance && this.canAttack)
            {
                this.preState = this.state;

                this.Attack();
                yield return YieldCache.WaitForSeconds(0.9f);
                if (this.player.state != Enums.ePlayerState.Attacking_Stun && this.player.state != Enums.ePlayerState.Attacking_Hit)
                {
                    this.PlayerHit();
                }

                if (this.preState == Enums.eMonsterState.Idle)
                {
                    this.state = this.preState;
                    this.anim.SetInteger("State", 0);
                }
                else if (this.preState == Enums.eMonsterState.Walk)
                {
                    this.state = this.preState;
                    this.anim.SetInteger("State", 1);
                }
                yield return null;
            }
            if (!this.canAttack)
            {
                int selectAction = Random.Range(1, 3);
                float waitTime = Random.Range(1f, 2f);

                if (selectAction == 1)
                    this.ResetPath();
                else
                    this.SetRandomDestination();

                yield return YieldCache.WaitForSeconds(waitTime);

            }
            yield return null;
        }
    }


    public void CanAttack()
    {
        this.canAttack = true;
    }


    private void Attack()
    {
        this.canAttack = false;
        Invoke("CanAttack", 4);

        //this.transform.LookAt(this.player.transform);
        this.state = Enums.eMonsterState.Attack;
        this.anim.SetInteger("State", 2);


    }

    private void PlayerHit()
    {
        this.player.canMove = false;

        Vector3 dir = this.player.transform.position - this.transform.position;
        dir.Normalize();
        rbody.AddForce(dir * knockbackForce, ForceMode.Impulse);
        //Debug.Log("몬스터한테 맞음");

        if (GameObject.FindObjectOfType<Tutorial05Main>() != null)
        {
            this.player.canMove = true;
        }
        else
            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.GET_HIT);
        //foreach (var info in InfoManager.instance.StageInfos)
        //{
        //    if (!info.isClear)
        //    {
        //        if (info.stage == 4)
        //        {
        //            this.player.canMove = true;
        //        }
        //        else
        //        {
        //            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.GET_HIT);

        //        }
        //        break;
        //    }
        //}
        //this.player.canMove = true;
    }

    private void ResetPath()
    {
        //Debug.Log("<color=yellow>정지</color>");
        this.state = Enums.eMonsterState.Idle;
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
    private void TrackPlayer()
    {
        //Debug.Log("<color=yellow>플레이어 추적</color>");

        //Debug.LogFormat("trackPlayer");
        this.state = Enums.eMonsterState.Track;
        //this.transform.LookAt(this.player.transform);
        agent.SetDestination(player.transform.position);
        agent.isStopped = false;
    }

    private void SetRandomDestination()
    {
        //Debug.Log("<color=yellow>경로 재설정</color>");

        //Debug.LogFormat("setrandomdestination");
        this.state = Enums.eMonsterState.Walk;
        float coordinateX = Random.Range(-3f, 3f);
        float coordinateZ = Random.Range(-3f, 3f);
        Vector3 ranPos = this.transform.position + new Vector3(coordinateX, this.transform.position.y, coordinateZ);
        //Debug.LogFormat("ranPos:{0}", ranPos);
        //this.transform.LookAt(ranPos + new Vector3(0, 1, 0));
        agent.SetDestination(ranPos);
        agent.isStopped = false;

    }

}
