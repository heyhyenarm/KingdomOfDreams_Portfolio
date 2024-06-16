using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingCabbage : MonoBehaviour
{
    public Coroutine routine;

    public int transNum;

    public Enums.eCabbageType type;
    public GameObject normalCabbageGo;
    public GameObject poisonCabbageGo;
    public GameObject poisonEffectGo;

    public PlayerMono player;

    public FarmRow farmRow;

    public GameObject[] dreamPiecesPrefab;
    private GameObject dreamPiece;
    public GameObject magicPiecePrefab;
    private GameObject magicPiece;

    //아이템
    private Item itemScript;
    private SphereCollider sphere;
    public FarmingCabbage Clone()
    {
        return Instantiate(this);
    }
    private void Awake()
    {
        itemScript = GetComponent<Item>();
        sphere = GetComponent<SphereCollider>();

        itemScript.enabled = false;
        sphere.enabled = false;
    }

    public void Init()
    {
        //Debug.LogFormat("Find player clone:{0}", GameObject.Find("Player(Clone)"));
        //Debug.LogFormat("playermono:{0}", GameObject.Find("Player(Clone)").GetComponent<PlayerMono>());

        this.player = GameObject.Find("Player(Clone)").GetComponent<PlayerMono>();
        //Debug.LogFormat("player:{0}, rigid:{1}", this.player, this.player.GetComponent<Rigidbody>());


        this.farmRow = this.transform.parent.parent.GetComponent<FarmRow>();
        //양배추 위치
        startPos = this.transform.position;

    }

    public void CheckPos() 
    {
        if (this.farmRow.stage == 3)
        {
            float zdir = startPos.z - player.transform.position.z;
            zdir = zdir / Mathf.Abs(zdir);
            Debug.LogFormat("z:{0}", zdir);
            endPos = startPos + new Vector3(0, 0, 2) * zdir;
        }
        if (this.farmRow.stage == 6)
        {
            float xdir = startPos.x - player.transform.position.x;
            xdir = xdir / Mathf.Abs(xdir);
            Debug.LogFormat("x:{0}", xdir);
            endPos = startPos + new Vector3(2, 0, 0) * xdir;
        }
        if (this.farmRow.stage == 7)
        {
            float xdir = startPos.x - player.transform.position.x;
            xdir = xdir / Mathf.Abs(xdir);
            Debug.LogFormat("x:{0}", xdir);
            endPos = startPos + new Vector3(2, 0, 0) * xdir;
        }
        if (this.farmRow.stage == 8)
        {
            float xdir = startPos.x - player.transform.position.x;
            xdir = xdir / Mathf.Abs(xdir);
            Debug.LogFormat("x:{0}", xdir);
            endPos = startPos + new Vector3(2, 0, 0) * xdir;
        }
    }

    private Vector3 startPos, endPos;
    //땅에 닿기까지 걸리는 시간
    protected float timer;
    protected float timeToFloor;
    //public System.Action onFloor;

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        System.Func<float, float> f = x => -4 * 2 * height * x * x + 4 * height * x;
        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
    private float waitTime = 0.38f;
    public IEnumerator MoveParabolic()
    {
        //yield return new WaitForSeconds(waitTime);
        timer = 0;
        while (transform.position.y >= startPos.y)
        {
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, endPos, 1.5f, timer);
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
        this.tag = "Cabbage";
        this.gameObject.layer = 0;
        itemScript.enabled = true;
        sphere.enabled = true;
        yield break;
    }
    public void FarmingNormal()
    {
        Debug.Log("일반 양배추");
        this.CheckPos();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.CABBAGE_INACTIVE, this);
        StartCoroutine(this.MoveParabolic());
        this.GenerateDreamPieces();
        this.farmRow.Poses[transNum].GetComponent<TargetPos>().cabbageList.Clear();

        
    }
    public void FarmingPoison()
    {
        Debug.Log("독 양배추");
        this.poisonEffectGo.SetActive(true);
        this.poisonEffectGo.GetComponent<ParticleSystem>().Play();

        //Destroy(this.poisonCabbageGo);
        this.poisonCabbageGo.SetActive(false);

        EventManager.instance.onPoison();
        this.GenerateDreamPieces();
        EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.CABBAGE_INACTIVE, this);

        this.farmRow.Poses[transNum].GetComponent<TargetPos>().cabbageList.Clear();
        // StartCoroutine(WaitDestroy(this.gameObject, 0.8f));
    }

    private void GenerateDreamPieces()
    {
        Debug.Log("꿈 조각 생성");
        int randomValue = Random.Range(1,101);
        Debug.LogFormat("randomValue:{0}", randomValue) ;


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
            Debug.Log("마법 도구 없을 때 꿈 조각 생성");

            foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
            {
                Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
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
                    Debug.Log("진짜로 꿈 조각 생성");

                    magicPiece = Instantiate(magicPiecePrefab);
                    magicPiece.transform.position = this.transform.position;
                }

            }

        }

    }
}
