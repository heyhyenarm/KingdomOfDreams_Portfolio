using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject ironPrefab;
    public GameObject ironPiecePrefab;
    public float ironRespawnTime = 3f;

    private GameObject ironPiece;
    private Vector3 lastIronPosition;


    private void Start()
    {
        lastIronPosition = transform.position;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Iron"))
        {
            lastIronPosition = collision.transform.position; // ö�� �ε��� ��ġ ���
            Destroy(collision.gameObject); // ö �ı�
            GenerateIronPiece(); // ö ���� ����
            Invoke("GenerateIron", ironRespawnTime); // ���� �ð� �� ö ����
        }
    }

    private void GenerateIronPiece()
    {
        ironPiece = Instantiate(ironPiecePrefab, transform.position, transform.rotation);
        ironPiece.transform.position = lastIronPosition; // ö�� ����� ��ġ�� ö ���� ����
    }

    private void GenerateIron()
    {
        Instantiate(ironPrefab, lastIronPosition, Quaternion.identity);
    }
}
