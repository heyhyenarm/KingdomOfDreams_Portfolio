using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform mid;
    public Transform per;
    public float vectorLength = 0;
    public float ratio = 0.5f;
    public int numSegments = 10;
    public bool isfishing = false;

    [SerializeField]
    private LineRenderer lineRenderer;
    private Vector3[] curvePoints;
    private Vector3 startPoint;
    public Vector3 endPoint;
    private Vector3 midPoint;

    // Start is called before the first frame update
    void Start()
    {
        //startPoint = start.position;
        //endPoint = end.localPosition;
        //midPoint = Vector3.Lerp(startPoint, endPoint, ratio);
        //this.mid.position = midPoint;

        //// �� ���� �մ� ���͸� ���
        //Vector3 c = endPoint - startPoint;

        //// �ð�������� 90�� ȸ���� ���͸� ����
        //Vector3 perpendicularVector = new Vector3(-c.y, c.x, 0f);
        //perpendicularVector = perpendicularVector.normalized * this.vectorLength;

        //this.per.position = this.mid.position + perpendicularVector;

        // ������ ��� ������, ������, ���� ����
        curvePoints = new Vector3[3];
        //curvePoints[0] = startPoint;  // ������
        //curvePoints[1] = this.per.position;
        //curvePoints[2] = endPoint;  // ����

        // LineRenderer ������Ʈ ����
        this.lineRenderer = GameObject.FindObjectOfType<LineRenderer>();
        this.lineRenderer.positionCount = this.numSegments + 1;
        this.lineRenderer.startWidth = 0.05f;
        this.lineRenderer.endWidth = 0.05f;

        // ������ ��� �� ���� ����ϰ� LineRenderer�� �߰�
        //for (int i = 0; i <= numSegments; i++)
        //{
        //    float t = (float)i / (float)numSegments;
        //    Vector3 p = GetPointOnBezierCurve(curvePoints, t);
        //    this.lineRenderer.SetPosition(i, p);
        //}
    }

    // ������ ��� t���� �ش��ϴ� ���� ��ȯ�ϴ� �Լ�
    private Vector3 GetPointOnBezierCurve(Vector3[] points, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * points[0];
        p += 2 * u * t * points[1];
        p += tt * points[2];
        return p;
    }
    // Update is called once per frame
    void Update()
    {
        startPoint = start.position;
        //Debug.LogFormat("start : {0}", startPoint);
        //endPoint = end.position;
        if (!this.isfishing)
        {
            endPoint = start.position;
        }
        midPoint = Vector3.Lerp(startPoint, endPoint, ratio);
        //Debug.LogFormat("mid : {0}", midPoint);
        this.mid.position = midPoint;

        // �� ���� �մ� ���͸� ���
        Vector3 c = endPoint - startPoint;

        // �ð�������� 90�� ȸ���� ���͸� ����
        Vector3 perpendicularVector = new Vector3(0, -c.y, 0);
        perpendicularVector = perpendicularVector.normalized * this.vectorLength;

        this.per.position = this.mid.position + perpendicularVector;

        curvePoints[0] = startPoint;  // ������
        curvePoints[1] = this.per.position;
        curvePoints[2] = endPoint;  // ����
                                    // ������ ��� �� ���� ����ϰ� LineRenderer�� �߰�
        for (int i = 0; i <= numSegments; i++)
        {
            float t = (float)i / (float)numSegments;
            Vector3 p = GetPointOnBezierCurve(curvePoints, t);
            this.lineRenderer.SetPosition(i, p);
        }
    }
}
