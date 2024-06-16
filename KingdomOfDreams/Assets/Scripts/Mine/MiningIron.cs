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
        lastIronPosition = transform.position; // ö�� ĳ�� ��ġ ���
        gameObject.SetActive(false); // ö ��Ȱ��ȭ
        GenerateIronPiece(); // ö ���� ����
        GenerateDreamPieces(); //�� ���� ����
        Invoke("GenerateIron", ironRespawnTime); // ���� �ð� �� ö ����
    }

    private void GenerateIronPiece()
    {
        ironPiece = Instantiate(ironPiecePrefab, transform.position, transform.rotation);
        ironPiece.transform.position = lastIronPosition; // ö�� ����� ��ġ�� ö ���� ����
    }

    private void GenerateDreamPieces()
    {
        float randomValue = Random.value;

        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

        if (myLevel >= 1) //���������� ������
        {
            if (randomValue <= 0.1f) // 10%�� Ȯ���� ����
            {
                dreamPiece = Instantiate(dreamPiecesPrefab[Random.Range(0, dreamPiecesPrefab.Length)], transform.position, transform.rotation);
                dreamPiece.transform.position = lastIronPosition;
            }
        }

        if (myLevel == 0)  //���������� ������
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

            if (stageNum <= 4) //5�������� ���Ͽ����� ���� ����
            {

            }
            else if (stageNum > 4)//6������������
            {
                if (randomValue <= 0.1f) // 10% Ȯ���� ����
                {
                    magicPiece = Instantiate(magicPiecePrefab, transform.position, transform.rotation);
                    magicPiece.transform.position = lastIronPosition; // ö�� ����� ��ġ�� ������ �� ���� ����
                }

            }

        }

    }

    public void GenerateIron()
    {
        gameObject.SetActive(true);
    }


}
