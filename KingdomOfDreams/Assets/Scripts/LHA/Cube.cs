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
            lastIronPosition = collision.transform.position; // Ã¶ÀÌ ºÎµúÈù À§Ä¡ ±â¾ï
            Destroy(collision.gameObject); // Ã¶ ÆÄ±«
            GenerateIronPiece(); // Ã¶ Á¶°¢ »ý¼º
            Invoke("GenerateIron", ironRespawnTime); // ÀÏÁ¤ ½Ã°£ ÈÄ Ã¶ »ý¼º
        }
    }

    private void GenerateIronPiece()
    {
        ironPiece = Instantiate(ironPiecePrefab, transform.position, transform.rotation);
        ironPiece.transform.position = lastIronPosition; // Ã¶ÀÌ »ç¶óÁø À§Ä¡¿¡ Ã¶ Á¶°¢ »ý¼º
    }

    private void GenerateIron()
    {
        Instantiate(ironPrefab, lastIronPosition, Quaternion.identity);
    }
}
