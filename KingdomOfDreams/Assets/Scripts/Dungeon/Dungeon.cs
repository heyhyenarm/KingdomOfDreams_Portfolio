using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Dungeon : MonoBehaviour
{
    public Transform[] monsterPoses;
    public GameObject monsterPrefab;
    private List<AttackingMonster> monsterList = new List<AttackingMonster>();

    public UILife uiLife;
    private PlayerMono player;

    public GameObject messageGo;
    public Button btnDungeonOut;
    private App.eSceneType preSceneType;

    private float regenTime = 10;

    public Color defaultColor = new Color(0, 0, 0, 0);

    public void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.TAKE_STUN, TakeStun);
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.GET_HIT, GetHit);
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.IN_DUNGEON, InDungeon);
        EventDispatcher.instance.RemoveEventHandler<AttackingMonster>((int)LHMEventType.eEventType.MONSTER_DIE, this.DieMonster);

    }

    public void Init5()
    {
        this.player = GameObject.Find("Player(Clone)").GetComponent<PlayerMono>();
        for(int i = 0; i < 4; i++)
        {
            this.transform.GetChild(i).GetComponent<AttackingMonster>().Init();

        }
        EventDispatcher.instance.AddEventHandler<AttackingMonster>((int)LHMEventType.eEventType.MONSTER_DIE, this.DieMonster);

    }

    public void Init(App.eSceneType sceneType) 
    {
        this.preSceneType = sceneType;
        this.player = GameObject.Find("Player(Clone)").GetComponent<PlayerMono>();
        //Debug.LogFormat("<color=yellow>uillifeGo:{0}</color>", GameObject.Find("UILife"));
        //Debug.LogFormat("<color=yellow>uillife:{0}</color>", GameObject.Find("UILife").GetComponent<UILife>());
        this.uiLife = GameObject.Find("UILife").GetComponent<UILife>();
        //this.uiLife = GameObject.FindObjectOfType<UILife>();

        this.AddEvent();

        for (int i = 0; i < monsterPoses.Length; i++)
        {
            AttackingMonster monster = this.GenerateMonster(i);
            monster.Init();
            this.monsterList.Add(monster);
        }
    }

    private void AddEvent()
    {
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.TAKE_STUN, TakeStun);
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.GET_HIT, GetHit);
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.IN_DUNGEON, InDungeon);
        EventDispatcher.instance.AddEventHandler<AttackingMonster>((int)LHMEventType.eEventType.MONSTER_DIE, this.DieMonster);


        if (this.btnDungeonOut != null)
        {
            btnDungeonOut.onClick.AddListener(() =>
            {
                //Debug.Log("<color=yellow>나가기 확인</color>");
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.EXIT_DUNGEON);
                //this.ReturnHome(preSceneType);
                this.uiLife.GetComponent<Image>().color = defaultColor;
                this.uiLife.txt.gameObject.SetActive(false);
                this.messageGo.SetActive(false);
            });
        }
    }

    private void DieMonster(short type, AttackingMonster monster)
    {
        //Debug.Log("die monster event");
        StartCoroutine(this.SetActiveMonster(monster));
    }
    private IEnumerator SetActiveMonster(AttackingMonster monster)
    {
        monster.DropMeat();
        monster.DropPiece();
        monster.gameObject.SetActive(false);
        yield return YieldCache.WaitForSeconds(regenTime);
        monster.character.GetComponent<Renderer>().material.color = monster.baseColor;
        monster.gameObject.SetActive(true);
        monster.Init();
        yield break;
    }


    private void TakeStun(short type)
    {
        //Debug.Log("<color=red>기절 이벤트</color>");
        this.StunReaction();
    }
    private void GetHit(short type)
    {
        if (this.uiLife != null)
        {
            if(this.uiLife.lifes[this.uiLife.lifes.Count - 1].gameObject != null)
            {
                Destroy(this.uiLife.lifes[this.uiLife.lifes.Count - 1].gameObject);
                this.uiLife.lifes.RemoveAt(this.uiLife.lifes.Count - 1);

            }

            //Debug.LogFormat("<color=yellow>life count:{0}</color>", this.uiLife.lifes.Count);
            if (this.uiLife.lifes.Count == 0)
                EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.TAKE_STUN);
            else
            {
                this.player.canMove = true;
            }
        }
    }
    private void InDungeon(short type)
    {
        //Debug.Log("<color=yellow>InDungeon</color>");
        this.uiLife.Init();
        this.uiLife.gameObject.SetActive(true);
    }

    private void StunReaction()
    {
        //Debug.LogFormat("플레이어 기절");
        this.player.PlayAnimation(Enums.ePlayerState.Attacking_Stun);
        this.player.canMove = false;
        this.player.GetComponent<Rigidbody>().constraints = 
            RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        //this.player.StartCoroutine("StunAnim");
        this.messageGo.SetActive(true);

    }

    public AttackingMonster GenerateMonster(int i)
    {
        if (monsterPoses[i].GetComponent<TargetPos>().monsterList.Count == 0)
        {
            //Debug.Log("몬스터 생성");
            var monster = Instantiate(monsterPrefab, monsterPoses[i]).GetComponent<AttackingMonster>();
            monster.transNum = i;
            monsterPoses[i].GetComponent<TargetPos>().monsterList.Add(monster);
            return monster;
        }
        else return null;

    }
}