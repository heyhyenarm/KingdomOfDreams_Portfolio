using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Activation : MonoBehaviour
{
    public Player player;

    private float regenTime = 3f;
    public FarmRow farmRow;
    public Dungeon dungeon;

    private float delta;

    private void Start()
    {

        //this.player.onFarming = ((cabbage) => {
        //    var transNum = cabbage.transNum;

        //    if (cabbage.cropType == HSYEnum.eCropType.Poison)
        //    {
        //        Debug.Log("독 양배추");
        //        cabbage.poisonEffectGo.SetActive(true);
        //        cabbage.poisonEffectGo.GetComponent<ParticleSystem>().Play();
        //        Destroy(cabbage.poisonCabbageGo);
        //        StartCoroutine(WaitSeconds(cabbage.gameObject, 0.8f));
        //    }
        //    if (cabbage.cropType == HSYEnum.eCropType.Normal)
        //    {
        //        Debug.Log("일반 양배추");
        //        StartCoroutine(cabbage.MoveParabolic());
        //    }
        //    this.farmRow.crobPoses[transNum].GetComponent<HSYTargetPos>().cabbageList.Clear();
        //    StartCoroutine(this.RegenCrob(transNum));
        //});
        ////사냥
        //this.player.onDungeon = (() => {
        //    Debug.Log("onDungeon");
        //    StartCoroutine(this.player.CorTakeEquipment());
        //    this.dungeon.uiLife.Init();
        //    this.dungeon.uiLife.gameObject.SetActive(true);
        //});

        //this.player.onAttack = ((monster) => {
        //    var transNum = monster.transNum;
        //    monster.onHit();
        //});

        //this.player.onHit = () =>
        //{
        //    Debug.LogFormat("onhit 실행");
        //    Destroy(this.dungeon.uiLife.lifes[this.dungeon.uiLife.lifes.Count - 1].gameObject);
        //    this.dungeon.uiLife.lifes.RemoveAt(this.dungeon.uiLife.lifes.Count - 1);

        //    if (this.dungeon.uiLife.lifes.Count == 0)
        //        StartCoroutine(this.player.CorFaint());
        //};
        //this.player.onPlain = () => {
        //    if (this.dungeon.uiLife.gameObject.activeSelf)
        //        this.dungeon.uiLife.gameObject.SetActive(false);
        //};
    }

    private IEnumerator WaitSeconds(GameObject targetGo, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(targetGo);
        yield break;
    }
}
