 using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour
{
    public float perspectiveZoomSpeed;
    public float orthoZoomSpeed;
    public Camera mainCamera;
    public CinemachineVirtualCamera followCam;
    private int stage;

    private float deltaMagnitudeDiff;

    public Transform center;
    private Transform followTrans;

    //private bool uiTouched;

    private void Start()
    {
        this.perspectiveZoomSpeed = 0.1f;
        this.orthoZoomSpeed = 0.1f;
        //this.mainCamera = Camera.main;
        //this.followCam = this.gameObject.GetComponent<CinemachineVirtualCamera>();

        if (GameObject.FindObjectOfType<PondMain>() != null)
        {
            this.stage = 0;
        }
        if (GameObject.FindObjectOfType<Stage06Main>() != null)
        {
            this.stage = 6;
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
        if (this.followCam.Follow != null && this.followTrans==null)
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

        //deltaPositoin: 이전 프레임의 터치 위치와 현 프레임의 터치 위치 차이 나타냄
        //touch0.deltaPosion=현재 터치 위치-이전 터치 위치
        //=====>현재 터치 위치 알면 이전 터치 위치 알 수 있다
        Vector2 preTouchPos0 = touch0.position - touch0.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
        Vector2 preTouchPos1 = touch1.position - touch1.deltaPosition;

        // 각 프레임에서 터치 사이의 벡터 거리 구함
        float prevTouchDeltaMag = (preTouchPos0 - preTouchPos1).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
        float touchDeltaMag = (touch0.position - touch1.position).magnitude;

        // 거리 차이 구함
        //음수이면 이전 값이 더 작다는 뜻이므로 줌인
        //양수이면 이전 값이 더 크다는 뜻이므로 줌아웃
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
}