using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningIron : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject ironPrefab;
    public GameObject ironPiecePrefab;
    public float ironRespawnTime = 7f;

    private GameObject ironPiece;
    private Vector3 lastIronPosition;

    public GameObject[] dreamPiecesPrefab;
    private GameObject dreamPiece;
    public GameObject magicPiecePrefab;
    private GameObject magicPiece;

    private int stageNum;

    private void Start()
    {
        lastIronPosition = transform.position;
    }

    void Update()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void Iron()
    {
        lastIronPosition = transform.position; // 철이 캐진 위치 기억
        gameObject.SetActive(false); // 철 비활성화
        GenerateIronPiece(); // 철 조각 생성
        GenerateDreamPieces(); //꿈 조각 생성
        Invoke("GenerateIron", ironRespawnTime); // 일정 시간 후 철 생성
    }

    private void GenerateIronPiece()
    {
        ironPiece = Instantiate(ironPiecePrefab, transform.position, transform.rotation);
        ironPiece.transform.position = lastIronPosition; // 철이 사라진 위치에 철 조각 생성
    }

    private void GenerateDreamPieces()
    {
        float randomValue = Random.value;

        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

        if (myLevel >= 1) //마법도구가 있을때
        {
            if (randomValue <= 0.1f) // 10%의 확률로 실행
            {
                dreamPiece = Instantiate(dreamPiecesPrefab[Random.Range(0, dreamPiecesPrefab.Length)], transform.position, transform.rotation);
                dreamPiece.transform.position = lastIronPosition;
            }
        }

        if (myLevel == 0)  //마법도구가 없을때
        {
            foreach (StageInfo stageInfo in InfoManager.instance.StageInfos)
            {
                Debug.LogFormat("<color>stageInfo.stage : {0}, stageInfo.isClear : {1}</color>", stageInfo.stage, stageInfo.isClear);
                if (stageInfo.isClear == false)
                {
                    this.stageNum = stageInfo.stage;
                    break;
                }
            }

            if (stageNum <= 4) //5스테이지 이하에서는 실행 안함
            {

            }
            else if (stageNum > 4)//6스테이지부터
            {
                if (randomValue <= 0.1f) // 10% 확률로 실행
                {
                    magicPiece = Instantiate(magicPiecePrefab, transform.position, transform.rotation);
                    magicPiece.transform.position = lastIronPosition; // 철이 사라진 위치에 마법의 꿈 조각 생성
                }

            }

        }

    }

    public void GenerateIron()
    {
        gameObject.SetActive(true);
    }


}
