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
            this.minBounds = new Vector2(-20f, -10f); // �������� 6�� �ּ� ���� ����
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

        if (Input.touchCount == 2) //�հ��� 2���� ������ ��
        {
            bool uiTouched = false;

            // ��ġ�� ����Ʈ���� Ȯ���ϸ� UI�� ��ġ�Ǿ����� Ȯ���մϴ�.
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                // ��ġ�� ����Ʈ�� UI�� ��ġ���� Ȯ���մϴ�.
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    uiTouched = true;
                    break;
                }
            }

            // UI�� ��ġ���� �ʾ��� ���� ���ϴ� ������ �����մϴ�.
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
        Touch touch0 = Input.GetTouch(0); //ù��° �հ��� ��ġ�� ����
        Touch touch1 = Input.GetTouch(1); //�ι�° �հ��� ��ġ�� ����

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
