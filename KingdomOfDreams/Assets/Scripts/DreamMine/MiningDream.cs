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
        lastIronPosition = transform.position; // 꿈이 캐진 위치 기억
        gameObject.SetActive(false); // 꿈 비활성화
        GenerateDreamPiece(); // 꿈 조각 생성
    }

    private void GenerateDreamPiece()
    {
        dreamPiece = Instantiate(dreamPiecePrefab, transform.position, transform.rotation);
        dreamPiece.transform.position = lastIronPosition; // 꿈이 사라진 위치에 꿈 조각 생성
    }

}
