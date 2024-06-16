using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDream : MonoBehaviour
{
    public GameObject dreamPiecePrefab;

    private GameObject dreamPiece;
    private Vector3 lastIronPosition;

    private void Start()
    {
        lastIronPosition = transform.position;
    }


    public void Dream()
    {
        lastIronPosition = transform.position; // ���� ĳ�� ��ġ ���
        gameObject.SetActive(false); // �� ��Ȱ��ȭ
        GenerateDreamPiece(); // �� ���� ����
    }

    private void GenerateDreamPiece()
    {
        dreamPiece = Instantiate(dreamPiecePrefab, transform.position, transform.rotation);
        dreamPiece.transform.position = lastIronPosition; // ���� ����� ��ġ�� �� ���� ����
    }

}
