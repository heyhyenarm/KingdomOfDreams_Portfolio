using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Farming : MonoBehaviour
{
    private PlayerMono player;

    private bool isPoisoned;
    private float poisonDuration = 6f;
    private float elapsedTime = 0f;

    private float originalSpeed;
    private float originalRotateSpeed;

    private Renderer rend;
    private Color originalColor;

    public void Init(PlayerMono player)
    {
        this.player = player;
        for (int i = 0; i < 8; i++)
        {
            if (this.player.transform.GetChild(i).gameObject.activeSelf)
                this.rend = this.player.transform.GetChild(i).GetComponent<Renderer>();
        }

        this.GetOriginalValues();


        EventManager.instance.onPoison = () =>
        {
            //Debug.LogFormat("ispoisoned:{0}", this.isPoisoned);
            if (this.isPoisoned)
            {
                elapsedTime = 0;
            }
            else
            {
                this.isPoisoned = true;
                elapsedTime = 0;
            }
            this.PoisonReaction();

        };
    }

    private void Update()
    {
        //Debug.LogFormat("ispoisoned:{0}", this.isPoisoned);

        if (isPoisoned)
        {
            //Debug.LogFormat("elapsedTime:{0}", elapsedTime);
            elapsedTime += Time.deltaTime;

            if (elapsedTime < poisonDuration)
            {
                // �ߵ� ���� ����
                ApplyPoisonEffect();
            }
            else if (elapsedTime >= poisonDuration)
            {
                // �ߵ� ���� ����
                isPoisoned = false;
                elapsedTime = 6f;
                Restore();
            }
        }
    }
    public void SetPoisonDuration(float detoxProperty)
    {
        this.poisonDuration = 6 * detoxProperty; //���� ���ӽð� * �ص� �ɷ�
    }

    public void GetOriginalValues()
    {
        this.originalSpeed = this.player.speed;
        this.originalRotateSpeed = this.player.rotateSpeed;
        this.originalColor = this.rend.material.color;
    }

    public void PoisonReaction()
    {
        //Debug.LogFormat("poison cabbage reaction");
        this.player.PlayAnimation(Enums.ePlayerState.Farming_Poison);
    }

    private void ApplyPoisonEffect()
    {
        Debug.LogFormat("<color=#82f802>�ߵ� �ð�:{0}</color>", this.poisonDuration);

        // �̵� �ӵ��� ���ҽ�Ŵ
        //Debug.Log("�ӵ� ����, ����� ����");
        float poisonSpeedMultiplier = 0.5f; // �̵� �ӵ� ���� ����
        float newSpeed = originalSpeed * poisonSpeedMultiplier;
        float newRotateSpeed = originalRotateSpeed * poisonSpeedMultiplier;
        SetSpeed(newSpeed, newRotateSpeed);
        this.ChangeColor("Purple");
    }

    private void Restore()
    {
        // �̵� �ӵ��� ���� �������� ����
        //Debug.Log("�������");
        SetSpeed(originalSpeed, originalRotateSpeed);
        this.ChangeColor("Original");
    }

    private void SetSpeed(float speed, float rotateSpeed)
    {
        // �̵� �ӵ� ���� (������ ���ӿ� �°� �����ؾ� ��)
        // ����: Rigidbody ������Ʈ�� �ӵ� ����
        this.player.speed = speed;
        this.player.rotateSpeed = rotateSpeed;
    }

    private void ChangeColor(string color)
    {
        if (color == "Purple")
            this.rend.material.color = new Color(0.58f, 0.13f, 0.75f);
        if (color == "Original")
            this.rend.material.color = originalColor;
    }
}
