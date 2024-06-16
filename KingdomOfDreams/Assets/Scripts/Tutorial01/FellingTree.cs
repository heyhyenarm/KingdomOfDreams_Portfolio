using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FellingTree : MonoBehaviour
{
    private MeshCollider mColl;
    private GameObject[] fellGo;
    public bool isFallDown;

    public Material mat;
    private Vector3 tempPos;
    private Quaternion tempRot;
    private Rigidbody rigid;

    public GameObject woodPrefab;

    //�� ����
    public GameObject[] dreamPiecesPrefab;
    private GameObject dreamPiece;
    public GameObject magicPiecePrefab;
    private GameObject magicPiece;

    private int stageNum;

    private void Awake()
    {
        this.mColl = this.GetComponent<MeshCollider>();
        this.fellGo = MeshCut.Cut(this.gameObject, transform.position + new Vector3(0, 0.75f, 0), Vector3.up, mat);
        this.fellGo[1].transform.localScale = this.fellGo[0].transform.localScale;
        tempPos = fellGo[1].transform.position;
        tempRot = fellGo[1].transform.rotation;

        //�и��� Mesh
        var mColl1 = this.fellGo[1].AddComponent<MeshCollider>();
        mColl1.convex = true;
        rigid = this.fellGo[1].AddComponent<Rigidbody>();
        rigid.isKinematic = true;

        this.isFallDown = false;

        for (int i = 0; i < fellGo.Length; i++) this.fellGo[i].tag = this.gameObject.tag;
    }
    public void Init()
    {
        this.fellGo[1].transform.position = tempPos;
        this.fellGo[1].transform.rotation = tempRot;
        this.rigid.isKinematic = true;
        this.fellGo[1].SetActive(true);

        this.isFallDown = false;
        this.gameObject.layer = 6;
    }
    public void TreeFell()
    {
        //���� ��������, ������ ���
        Debug.Log("���� ��� ���");
        var woodGo = Instantiate(woodPrefab);
        var pos = this.transform.position;
        woodGo.transform.position = new Vector3(pos.x, pos.y + 1, pos.z);

        //�� ���� ���
        this.GenerateDreamPieces();

        int ran = Random.Range(0, 10);   //0~9
                                         //10%
        if (ran < 1)
        {
            //�� ����
            EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.APPEAR_BEE);
        }
        //�и��� ���� ������
        this.rigid.isKinematic = false;
        this.gameObject.layer = 0;

        this.isFallDown = true;

        StartCoroutine(RegenTree());
    }
    private IEnumerator RegenTree()
    {
        //���� �������� �ð�
        yield return new WaitForSeconds(3f);

        //������ �κ� ��Ȱ��ȭ
        this.fellGo[1].SetActive(false);

        //���� ���� �ð�
        yield return new WaitForSeconds(7f);

        //����(�ʱ�ȭ)
        Init();
    }

    //�� ���� ���
    private void GenerateDreamPieces()
    {
        float randomValue = Random.value;

        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;

        if (myLevel >= 1) //���������� ������
        {
            if (randomValue <= 0.1f) // 10%�� Ȯ���� ����
            {
                dreamPiece = Instantiate(dreamPiecesPrefab[Random.Range(0, dreamPiecesPrefab.Length)], transform.position, transform.rotation);
                dreamPiece.transform.position = this.transform.position;
            }
        }
        else if (myLevel == 0)  //���������� ������
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
            if (stageNum > 4) //6������������
            {
                if (randomValue <= 0.1f) // 10% Ȯ���� ����
                {
                    magicPiece = Instantiate(magicPiecePrefab, transform.position, transform.rotation);
                    magicPiece.transform.position = this.transform.position; //���� ��ġ�� ������ �� ���� ����
                }
            }
        }
    }
}