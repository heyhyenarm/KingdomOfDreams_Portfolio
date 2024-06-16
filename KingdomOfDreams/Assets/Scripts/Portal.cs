using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    //public enum ePortalType
    //{
    //    //������, ����, ����
    //    None = -1, Stage06, Stage07, Stage08, Pond, Mine, Dungeon
    //}
    public Enums.ePortalType typeToChange;
    private bool isContact;
    //public Enums.eSceneType type;

    private PlayerMono player;
    
    private void OnCollisionEnter(Collision collision)
    {

        //var mono = collision.gameObject.GetComponent<PlayerMono>();
        //if (mono != null)
        //{
        //    //�̺�Ʈ �߻�, �� ��ȯ, �÷��̾� 
        //    EventManager.instance.onPortalEnter(this.type);
        //}

        if (collision.gameObject.tag =="Player")
        {
            this.player = collision.gameObject.GetComponent<PlayerMono>();
            this.player.canMove = false;
            this.player.PlayAnimation(Enums.ePlayerState.Idle);

            if (GameObject.FindObjectOfType<Dungeon>() != null)
            {
                var dungeon = GameObject.FindObjectOfType<Dungeon>();
                for (int i = 0; i < dungeon.uiLife.lifes.Count; i++)
                {
                    dungeon.uiLife.lifes[i].gameObject.SetActive(false);


                    //if (dungeon.uiLife.lifes[i] != null)
                    //{
                    //    Debug.LogFormat("<color=yellow>dungeon out portal activeSelf �˻� �� i:{0}</color>", i);

                    //    if (dungeon.uiLife.lifes[i].gameObject.activeSelf)
                    //    {
                    //        Debug.LogFormat("<color=yellow>dungeon out portal activeSelf �˻� �� i:{0}</color>", i);
                    //        dungeon.uiLife.lifes[i].gameObject.SetActive(false);

                    //    }
                    //}
                }



                dungeon.uiLife.GetComponent<Image>().color = dungeon.defaultColor;
                dungeon.uiLife.txt.SetActive(false);
            }

            if(!this.isContact)
            {
                this.isContact = true;
                EventDispatcher.instance.SendEvent<Enums.ePortalType>((int)LHMEventType.eEventType.ENTER_PORTAL, this.typeToChange);
                
            }
        }

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    var player = other.gameObject.GetComponent<PlayerMono>();
    //    if(player != null)
    //    {
    //        //�̺�Ʈ �߻�, �� ��ȯ, �÷��̾� 
    //        this.onPortalEnter(this.type);
    //    }
    //}
}
