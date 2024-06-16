using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class ZoomTest : MonoBehaviour
{
    public float perspectiveZoomSpeed;
    public float orthoZoomSpeed;
    public Camera mainCamera;
    public CinemachineVirtualCamera followCam;
    private int stage;

    private float deltaMagnitudeDiff;

    public Transform center;
    private Transform followTrans;

    public Vector2 minBounds;
    public Vector2 maxBounds;

    private void Start()
    {
        this.perspectiveZoomSpeed = 0.1f;
        this.orthoZoomSpeed = 0.1f;

        if (GameObject.FindObjectOfType<PondMain>() != null)
        {
            this.stage = 0;
        }
        if (GameObject.FindObjectOfType<Stage06Main>() != null)
        {
            this.stage = 6;
            this.minBounds = new Vector2(-20f, -10f); // 스테이지 6의 최소 범위 설정
            this.maxBounds = new Vector2(20f, 10f);
        }
        if (GameObject.FindObjectOfType<Stage07Main>() != null)
        {
            this.stage = 7;
        }
        if (GameObject.FindObjectOfType<Stage08Main>() != null)
        {
            this.stage = 8;
        }
    }

    private void Update()
    {
        if (this.followCam.Follow != null && this.followTrans == null)
        {
            this.followTrans = this.followCam.Follow;
        }

        if (Input.touchCount == 2) //손가락 2개가 눌렸을 때
        {
            bool uiTouched = false;

            // 터치된 포인트들을 확인하며 UI가 터치되었는지 확인합니다.
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // 터치된 포인트가 UI와 겹치는지 확인합니다.
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    uiTouched = true;
                    break;
                }
            }

            // UI가 터치되지 않았을 때만 원하는 동작을 수행합니다.
            if (!uiTouched)
            {
                this.CalculateDiff();

                if (this.deltaMagnitudeDiff > 0)
                {
                    this.followCam.m_LookAt = center;
                    this.followCam.m_Follow = center;
                    this.followCam.m_Follow = null;

                    this.ZoomOut();
                }
                else if (this.deltaMagnitudeDiff < 0)
                {
                    this.followCam.m_LookAt = this.followTrans;
                    this.followCam.m_Follow = this.followTrans;
                    this.followCam.m_Follow = null;

                    this.ZoomIn();
                }

                this.followCam.m_LookAt = this.followTrans;
                followCam.m_Follow = this.followTrans;
            }
        }
    }

    private void CalculateDiff()
    {
        Touch touch0 = Input.GetTouch(0); //첫번째 손가락 터치를 저장
        Touch touch1 = Input.GetTouch(1); //두번째 손가락 터치를 저장

        Vector2 preTouchPos0 = touch0.position - touch0.deltaPosition;
        Vector2 preTouchPos1 = touch1.position - touch1.deltaPosition;

        float prevTouchDeltaMag = (preTouchPos0 - preTouchPos1).magnitude;
        float touchDeltaMag = (touch0.position - touch1.position).magnitude;

        this.deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
    }

    private void ZoomIn()
    {
        followCam.m_Lens.OrthographicSize += this.deltaMagnitudeDiff * orthoZoomSpeed;

        if (followCam.m_Lens.OrthographicSize < 3)
        {
            followCam.m_Lens.OrthographicSize = 3;
        }
    }

    private void ZoomOut()
    {
        followCam.m_Lens.OrthographicSize += this.deltaMagnitudeDiff * orthoZoomSpeed;

        if (this.stage == 0)
        {
            if (followCam.m_Lens.OrthographicSize > 13f)
            {
                followCam.m_Lens.OrthographicSize = 13f;
            }
        }
        if (this.stage == 6)
        {
            if (followCam.m_Lens.OrthographicSize > 21.5f)
            {
                followCam.m_Lens.OrthographicSize = 21.5f;
            }
        }
        if (this.stage == 7)
        {
            if (followCam.m_Lens.OrthographicSize > 29.4f)
            {
                followCam.m_Lens.OrthographicSize = 29.4f;
            }
        }
        if (this.stage == 8)
        {
            if (followCam.m_Lens.OrthographicSize > 78.7f)
            {
                followCam.m_Lens.OrthographicSize = 78.7f;
            }
        }
    }

    private void LateUpdate()
    {
        if (followTrans == null)
            return;

        float clampedX = Mathf.Clamp(followTrans.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(followTrans.position.y, minBounds.y, maxBounds.y);

        Vector3 newPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = newPosition;
    }
}
