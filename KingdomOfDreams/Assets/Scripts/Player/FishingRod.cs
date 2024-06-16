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

        //// 두 점을 잇는 벡터를 계산
        //Vector3 c = endPoint - startPoint;

        //// 시계방향으로 90도 회전한 벡터를 구함
        //Vector3 perpendicularVector = new Vector3(-c.y, c.x, 0f);
        //perpendicularVector = perpendicularVector.normalized * this.vectorLength;

        //this.per.position = this.mid.position + perpendicularVector;

        // 베지어 곡선의 시작점, 제어점, 끝점 설정
        curvePoints = new Vector3[3];
        //curvePoints[0] = startPoint;  // 시작점
        //curvePoints[1] = this.per.position;
        //curvePoints[2] = endPoint;  // 끝점

        // LineRenderer 컴포넌트 설정
        this.lineRenderer = GameObject.FindObjectOfType<LineRenderer>();
        this.lineRenderer.positionCount = this.numSegments + 1;
        this.lineRenderer.startWidth = 0.05f;
        this.lineRenderer.endWidth = 0.05f;

        // 베지어 곡선의 각 점을 계산하고 LineRenderer에 추가
        //for (int i = 0; i <= numSegments; i++)
        //{
        //    float t = (float)i / (float)numSegments;
        //    Vector3 p = GetPointOnBezierCurve(curvePoints, t);
        //    this.lineRenderer.SetPosition(i, p);
        //}
    }

    // 베지어 곡선의 t값에 해당하는 점을 반환하는 함수
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

        // 두 점을 잇는 벡터를 계산
        Vector3 c = endPoint - startPoint;

        // 시계방향으로 90도 회전한 벡터를 구함
        Vector3 perpendicularVector = new Vector3(0, -c.y, 0);
        perpendicularVector = perpendicularVector.normalized * this.vectorLength;

        this.per.position = this.mid.position + perpendicularVector;

        curvePoints[0] = startPoint;  // 시작점
        curvePoints[1] = this.per.position;
        curvePoints[2] = endPoint;  // 끝점
                                    // 베지어 곡선의 각 점을 계산하고 LineRenderer에 추가
        for (int i = 0; i <= numSegments; i++)
        {
            float t = (float)i / (float)numSegments;
            Vector3 p = GetPointOnBezierCurve(curvePoints, t);
            this.lineRenderer.SetPosition(i, p);
        }
    }
}
