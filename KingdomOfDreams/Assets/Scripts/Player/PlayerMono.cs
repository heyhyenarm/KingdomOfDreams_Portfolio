using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMono : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;
    public System.Action OnAnimationComplete;
    public VariableJoystick joystick;
    public bool isTargeting;
    private Transform target;

    //걷는 소리
    private AudioSource audioSource;
    public AudioClip[] grassClips;
    public AudioClip[] rockClips;
    public AudioClip[] dreamlandClips;
    private int currentClipIndex = 0;
    private bool isWalking = false;
    private bool isPlaying = false;
    private string walkState;
    private bool isChangingClip = false;
    private Coroutine walkingCoroutine;

    //꾸미기
    public List<Material> playerMaterials = new List<Material>();
    private Renderer playerRenderer;

    //플레이어 기본 정보
    public Enums.ePlayerLocation location;
    public Enums.ePlayerState state;
    public Transform hand_L;
    public Transform hand_R;
    public GameObject head;
    private Rigidbody rBody;
    public Transform shovelTrans;

    //이동
    private Vector3 dir;
    private Vector3 dir1;
    public float speed = 5f;
    public float rotateSpeed = 8f;
    public bool canMove;

    //행동
    public float min = 2f;
    public GameObject actionTarget;
    public FishingPoint fishingPoint;
    private Coroutine routine;
    private Farming farming;


    //장비
    public GameObject equipement;

    public GameObject axePrefab;
    public GameObject pickaxPrefab;
    public GameObject shovelPrefab;
    public GameObject swordPrefab;
    public GameObject fishingRodPrefab;

    private string VibrationOnOff = "VibrationOnOff";

    void OnDestroy()
    {
        EventDispatcher.instance.RemoveEventHandler((int)LHMEventType.eEventType.ATTACKED_BEE, AttackedByBeeHandler);

        //Debug.Log("<color=yellow>player mono destroy</color>");
    }
    public void Init()
    {
        //Debug.Log("@@@@@@@@player Init@@@@@@@@@");
        //Debug.LogFormat("1 this.anim: {0}", this.anim); //X
        this.anim = this.GetComponent<Animator>();
        //Debug.LogFormat("2 this.anim: {0}", this.anim); //O
        var character = this.transform.GetChild(InfoManager.instance.PlayerInfo.nowCharacterId).gameObject;
        //character.SetActive(true);
        Debug.LogFormat("<color=red>{0}</color>", InfoManager.instance.PlayerInfo.nowCharacterId);
        this.ChangeCharacter(InfoManager.instance.PlayerInfo.nowCharacterId);
        this.playerRenderer = character.GetComponent<Renderer>();

        var matNum = InfoManager.instance.PlayerInfo.nowMatNum;
        this.SetMaterial(this.playerMaterials[matNum]);

        //이벤트 등록
        EventDispatcher.instance.AddEventHandler((int)LHMEventType.eEventType.ATTACKED_BEE, AttackedByBeeHandler);
        //Debug.LogFormat("5 this.anim: {0}", this.anim); //O

        if (GameObject.FindObjectOfType<Farming>() != null)
        {
            this.farming = GameObject.FindObjectOfType<Farming>();
            this.farming.Init(this);
        }
        this.rBody = this.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //EventDispatcher.instance.AddEventHandler<int[]>((int)LHMEventType.eEventType.CHARACTER_MATERIAL_CHANGE, new EventHandler<int[]>((type, arr) =>
        //{
        //    Debug.Log("CHARACTER_MATERIAL_CHANGE");
        //    this.ChangeCharacter(arr[0]);
        //    this.SetMaterial(this.playerMaterials[arr[1]]);
        //}));
    }
   
    //벌 공격 이벤트 핸들러
    private void AttackedByBeeHandler(short type)
    {
        this.routine = StartCoroutine(this.StungByBee(this.anim.GetInteger("State")));
        this.PlayAnimation(Enums.ePlayerState.Felling_Bee);
    }
    private void Update()
    {
        if (this.state == Enums.ePlayerState.Idle)
        {
            this.rBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            this.rBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        //키보드 움직임
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        dir = Vector3.Normalize(new Vector3(h, 0, v));

        //조이스틱 움직임
        var h1 = joystick.Horizontal;
        var v1 = joystick.Vertical;
        dir1 = Vector3.Normalize(new Vector3(h1, 0, v1));

        //낚시터 타겟 -> 다른 방식
        if (this.location != Enums.ePlayerLocation.Pond)
        {
            //플레이어 근처에 타겟이 있는가?
            FindTarget();
        }
        else
        {
            this.actionTarget = this.fishingPoint.fish;
            if(this.actionTarget != null)
            {
                this.isTargeting = true;
            }
            else
            {
                this.isTargeting = false;
            }
        }

        if (this.fishingPoint.gameObject.activeSelf)    //낚시 포인트 활성화?
        {
            //낚시 포인트
            this.fishingPoint.transform.localPosition = new Vector3(0, -this.transform.position.y-0.45f, 3);  //y좌표 값을 -0.45로 만듬
        }
#if UNITY_EDITOR
        Move();
#elif UNITY_ANDROID
        MoveJoystick();
#endif
        if (isChangingClip)
        {
            if (!audioSource.isPlaying)
            {
                isChangingClip = false;
            }
        }

    }

    private void FixedUpdate()
    {
        //Move();
        //MoveJoystick();
    }
    public void PlayAnimation(Enums.ePlayerState state)
    {
        this.anim.SetInteger("State", (int)state);
        this.state = state;
    }
    private void MoveJoystick()
    {
        if (this.canMove && this.state != Enums.ePlayerState.Felling
            && this.state != Enums.ePlayerState.Attacking
            && this.state != Enums.ePlayerState.Farming
            && this.state != Enums.ePlayerState.Fishing
            && this.state != Enums.ePlayerState.Mining
            &&this.state!=Enums.ePlayerState.Attacking_Stun)
        {
            if (dir1 != Vector3.zero)
            {
                //this.anim.SetInteger("State", 1);
                switch (this.anim.GetInteger("State"))
                {
                    case 0:
                        this.PlayAnimation(Enums.ePlayerState.Run);
                        break;

                    case 2:
                        this.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Run);
                        break;

                    case 4:
                        this.PlayAnimation(Enums.ePlayerState.OneHandEquipe_Run);
                        break;

                    case 6:
                        this.PlayAnimation(Enums.ePlayerState.Sword_Run);
                        break;

                }

                this.transform.Translate(dir1 * this.speed * Time.deltaTime, Space.World);

                var angle = Mathf.Atan2(dir1.x, dir1.z) * Mathf.Rad2Deg;
                //회전
                this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                if (!isWalking)
                {
                    StartWalking();
                }
            }
            else if (dir1 == Vector3.zero)
            {
                //this.anim.SetInteger("State", 0);
                switch (this.anim.GetInteger("State"))
                {
                    case 1:
                        this.PlayAnimation(Enums.ePlayerState.Idle);
                        break;

                    case 3:
                        this.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
                        break;

                    case 5:
                        this.PlayAnimation(Enums.ePlayerState.OneHandEquipe_Idle);
                        break;

                    case 7:
                        this.PlayAnimation(Enums.ePlayerState.Sword_Idle);
                        break;
                }

                if (isWalking)
                {
                    StopWalking();
                }
            }
        }
    }
    private void Move()
    {
        //Debug.LogFormat("이동 가능? :{0}", this.canMove);
        if (this.canMove && this.state != Enums.ePlayerState.Felling
            && this.state != Enums.ePlayerState.Attacking
            && this.state != Enums.ePlayerState.Farming
            && this.state != Enums.ePlayerState.Fishing
            && this.state != Enums.ePlayerState.Mining
            && this.state != Enums.ePlayerState.Attacking_Stun)
        {
            if (dir != Vector3.zero)
            {
                //현재 상태 구분
                switch (this.anim.GetInteger("State"))
                {
                    case 0:
                        this.PlayAnimation(Enums.ePlayerState.Run);

                        break;

                    case 2:
                        this.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Run);

                        break;

                    case 4:
                        this.PlayAnimation(Enums.ePlayerState.OneHandEquipe_Run);
                        break;

                    case 6:
                        this.PlayAnimation(Enums.ePlayerState.Sword_Run);
                        break;
                }

                this.transform.Translate(dir * this.speed * Time.deltaTime, Space.World);

                var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                //회전
                this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                if (!isWalking)
                {
                    StartWalking();
                }
            }
            else if (dir == Vector3.zero)
            {
                //현재 상태 구분
                switch (this.anim.GetInteger("State"))
                {
                    case 1:
                        this.PlayAnimation(Enums.ePlayerState.Idle);

                        break;

                    case 3:
                        this.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);

                        break;

                    case 5:
                        this.PlayAnimation(Enums.ePlayerState.OneHandEquipe_Idle);
                        break;

                    case 7:
                        this.PlayAnimation(Enums.ePlayerState.Sword_Idle);
                        break;
                }

                if (isWalking)
                {
                    StopWalking();
                }
            }
        }
    }

    private void StartWalking()
    {
        isWalking = true;
        walkingCoroutine = StartCoroutine(PlayWalkClips());
    }

    private void StopWalking()
    {
        isWalking = false;
        if (walkingCoroutine != null)
        {
            StopCoroutine(walkingCoroutine);
        }
        audioSource.Stop();
        isPlaying = false; // 소리 재생 상태 초기화
    }

    private IEnumerator PlayWalkClips()
    {
        while (isWalking)
        {
            if (!isPlaying && !isChangingClip)
            {
                isPlaying = true;
                AudioClip clipToPlay = GetCurrentClip();
                audioSource.clip = clipToPlay;
                audioSource.Play();

                yield return new WaitForSeconds(clipToPlay.length);

                isPlaying = false;

                currentClipIndex++;
                if (currentClipIndex >= GetCurrentClips().Length)
                {
                    currentClipIndex = 0; // 순환 재생을 위해 인덱스 초기화
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private AudioClip[] GetCurrentClips()
    {
        if (this.walkState == "Grass")
        {
            return grassClips;
        }
        else if (this.walkState == "Rock")
        {
            return rockClips;
        }
        else if(this.walkState == "DreamLand")
        {
            return dreamlandClips;
        }
        else
        {
            return null;
        }
    }

    private AudioClip GetCurrentClip()
    {
        AudioClip[] currentClips = GetCurrentClips();
        if (currentClips != null && currentClipIndex < currentClips.Length)
        {
            return currentClips[currentClipIndex];
        }
        else
        {
            return null;
        }
    }

    public void FindTarget()
    {
        var targets = this.GetComponent<FieldOfView>().visibleTargets;
        float min = 0;
        if (targets.Count == 0)
        {
            this.isTargeting = false;
            this.actionTarget = null;

        }
        else
        {
            this.isTargeting = true;
            foreach (var target in targets)
            {
                if (target.transform != null)
                {
                    float distance = Vector3.Distance(this.transform.position, target.transform.position);

                    if (min == 0)
                    {
                        min = distance;
                    }
                    if (distance <= min)
                    {
                        min = distance;
                        this.actionTarget = target.gameObject;
                        //Debug.LogFormat("타겟 확인 {0}", this.actionTarget);
                    }
                }
            }
        }
        //Debug.LogFormat("target:{0}", target);
    }


    public void ChangeCharacter(int characterID)
    {
        var nowCharacterId = InfoManager.instance.PlayerInfo.nowCharacterId;
        //Debug.LogFormat("<color=red>{0}</color>", nowCharacterId);
        var prevcharacter = this.transform.GetChild(nowCharacterId).gameObject;
        prevcharacter.SetActive(false);
        var character = this.transform.GetChild(characterID).gameObject;
        character.SetActive(true);
        this.playerRenderer = character.GetComponent<Renderer>();
        //Debug.LogFormat("Set Character {0}", character);
    }
    //public void AddCharacter(int characterID)
    //{
    //    var character = this.transform.GetChild(characterID).gameObject;
    //    character.SetActive(true);
    //    this.playerRenderer = character.GetComponent<Renderer>();

    //    //InfoManager.instance.PlayerInfo.myCharacters.Add(InfoManager.instance.PlayerInfo.nowCharacterId, character);
    //    //Debug.LogFormat("Set Character {0}", character);
    //}
    public void SetMaterial(Material mat)
    {
        playerRenderer.material = mat;
        //InfoManager.instance.PlayerInfo.nowMatNum = mat;
        //Debug.LogFormat("Set Material {0}", mat);
    }

    //===============벌목===============
    public IEnumerator FellingAnim()
    {
        var target = this.actionTarget;
        this.canMove = false;
        if (this.actionTarget != null)
        {
            this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        }

        yield return new WaitForSeconds(0.5f);    //벌목 애니메이션

        //SoundManager.StopSFX();
        //SoundManager.PlaySFX("FF_EFX_foley_chopping_wood_single_hit");
        SoundManager.PlayCappedSFX("FF_EFX_foley_chopping_wood_single_hit", "Action");
        var particle = target.GetComponentInChildren<ParticleSystem>();
        particle.Play();


        yield return new WaitForSeconds(1f);    //벌목 애니메이션

        SoundManager.PlayCappedSFX("FF_EFX_foley_chopping_wood_single_hit", "Action");
        particle.Play();
        if(PlayerPrefs.GetInt(this.VibrationOnOff)==1) Handheld.Vibrate();

        yield return new WaitForSeconds(0.5f);    //벌목 애니메이션

        this.OnAnimationComplete();

        if(this.actionTarget != null)
        {
            var tree = target.GetComponent<FellingTree>();
            //나무 재료 드랍
            tree.TreeFell();
        }

        this.canMove = true;
    }
    public IEnumerator StungByBee(int stateNum)
    {
        if (PlayerPrefs.GetInt(this.VibrationOnOff) == 1) Handheld.Vibrate();

        //벌에 쏘임
        this.canMove = false;

        //기절 시간, 머리 커짐
        if(this.head.transform.localScale == Vector3.one)
        {
            for (int i = 0; i < 20; i++)
            {
                yield return new WaitForSeconds(0.05f);

                this.head.transform.localScale += new Vector3(1f, 1f, 1f);
            }
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        //Idle
        this.canMove = true;
        if(stateNum==0 || stateNum==1)
            this.PlayAnimation(Enums.ePlayerState.Idle);
        else if(stateNum==2 || stateNum==3)
            this.PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);

        var stungTime = 6f;
        var myLevel = InfoManager.instance.MagicToolInfo.Find(x => x.id == 300).level;
        if (myLevel >= 1) //1레벨 이상이면
        {
            var data = DataManager.instance.GetMagicToolLevelDatas().Find(x => x.level == myLevel);
            stungTime = 6 * data.add_detox_property;
        }

        this.routine = null;

        yield return new WaitForSeconds(stungTime);

        if(this.routine==null)
            this.head.transform.localScale = Vector3.one;
        //this.head.transform.localScale -= new Vector3(20f, 20f, 20f);
    }
    //===================================

    //===============낚시===============
    private FishingRod fishingRod;
    public IEnumerator FishingAnim()
    {
        var target = this.actionTarget;
        this.fishingPoint.gameObject.SetActive(false);
        this.canMove = false;

        yield return new WaitForSeconds(0.55f);
        
        SoundManager.PlayCappedSFX("ESM_FG_FX_one_shot_fishing_pole_throw_whoosh_lure_into_water_splash_cast_2", "Action");

        this.fishingRod.isfishing = true;
        this.fishingRod.vectorLength = 0f;
        this.fishingRod.endPoint = target.transform.position;

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);

            this.fishingRod.vectorLength += 0.2f;
        }
        yield return new WaitForSeconds(1f);

        SoundManager.PlayCappedSFX("ESM_FG_FX_one_shot_fishing_pole_cast_line_1_throw_fishing_pole_rod_movement_ready", "Action");

        yield return new WaitForSeconds(1.233f);

        SoundManager.PlayCappedSFX("ESM_FG_FX_one_shot_fishing_water_complete_3_done_capture_splash_catch_medium_splash", "Action");

        var go = GameObject.Find("FX_Impact_Water_Ripple_01");
        go.transform.position = target.transform.position;
        var particle = go.GetComponent<ParticleSystem>();
        particle.Play();
        if (PlayerPrefs.GetInt(this.VibrationOnOff) == 1) Handheld.Vibrate();

        this.actionTarget.transform.SetParent(this.hand_L);
        this.actionTarget.transform.localPosition = new Vector3(-0.277f, 0, 0.23f);
        this.actionTarget.transform.localEulerAngles = new Vector3(8, -224, 35);    //8 - 224 35

        this.fishingRod.isfishing = false;

        yield return new WaitForSeconds(2f);

        this.OnAnimationComplete();

        SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");

        EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_INGREDIENT, 2001);

        if (target != null) Destroy(target.gameObject);
        this.actionTarget = null;

        yield return new WaitForSeconds(0.344f); //Idle전환

        this.fishingPoint.gameObject.SetActive(true);
        this.canMove = true;
    }
    //===================================

    //===============채집===============
    public IEnumerator FarmingAnim()
    {
        var target = this.actionTarget;
        this.canMove = false;
        this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));

        var go = GameObject.Find("FX_Impact_Dirt_01");
        go.transform.position = target.transform.position;

        // yield return YieldCache.WaitForSeconds(1.1f); //채집 애니메이션   

        yield return YieldCache.WaitForSeconds(0.3f); //채집 애니메이션   

        SoundManager.PlayCappedSFX("ESM_FG_FX_one_shot_farming_dig_shovel_impact_ground_old_square_point_grass_metal_pressure_creaking_2", "Action");
        
        var particle = go.GetComponent<ParticleSystem>();
        particle.Play();
        if (PlayerPrefs.GetInt(this.VibrationOnOff) == 1) Handheld.Vibrate();

        yield return YieldCache.WaitForSeconds(0.3f); //채집 애니메이션   

        //재료 드랍
        var cabbage = target.GetComponent<FarmingCabbage>();
        if (cabbage.type == Enums.eCabbageType.Poison)
        {
            cabbage.FarmingPoison();
            //EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.GET_POISON);
            yield return YieldCache.WaitForSeconds(1.333f);
        }
        else
        {
            cabbage.FarmingNormal();
        }

        this.OnAnimationComplete();

        //var transNum = cabbage.transNum;
        //var farmRow = cabbage.transform.parent.parent.GetComponent<FarmRow>();

        yield return YieldCache.WaitForSeconds(0.25f);


        this.canMove = true;
        yield break;
    }

    //===================================

    //===============광질===============
    public IEnumerator MiningAnim()
    {
        var target = this.actionTarget;
        this.canMove = false;
        this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));

        yield return new WaitForSeconds(0.5f);    //광질 애니메이션

        SoundManager.PlayCappedSFX("ESM_GB_fx_foley_one_shot_metlimpt_mining_metal_heavy_impact_02_destroy_hit_material", "Action");
        var particle = target.GetComponentInChildren<ParticleSystem>();
        particle.Play();
        if (PlayerPrefs.GetInt(this.VibrationOnOff) == 1) Handheld.Vibrate();

        yield return new WaitForSeconds(0.5f);    //광질 애니메이션

        this.OnAnimationComplete();

        var iron = target.GetComponent<MiningIron>();
        iron.Iron();

        yield return new WaitForSeconds(0.25f); //Idle전환

        this.canMove = true;
    }

    //===================================

    //===============광질 in 꿈광산===============
    public IEnumerator DreamMiningAnim()
    {
        var target = this.actionTarget;
        this.canMove = false;
        this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));

        yield return new WaitForSeconds(0.5f);    //광질 애니메이션

        SoundManager.PlayCappedSFX("ESM_GB_fx_foley_one_shot_metlimpt_mining_metal_heavy_impact_02_destroy_hit_material", "Action");
        var particle = target.GetComponentInChildren<ParticleSystem>();
        particle.Play();
        if (PlayerPrefs.GetInt(this.VibrationOnOff) == 1) Handheld.Vibrate();

        yield return new WaitForSeconds(0.5f);    //광질 애니메이션
        

        this.OnAnimationComplete();

        var dream = target.GetComponent<MiningDream>();
        dream.Dream();

        yield return new WaitForSeconds(0.25f); //Idle전환

        this.canMove = true;
    }

    //===================================

    //===============사냥===============
    public IEnumerator AttackingAnim()
    {
        var target = this.actionTarget;
        this.canMove = false;
        //this.transform.LookAt(new Vector3(this.actionTarget.transform.position.x, this.transform.position.y, this.actionTarget.transform.position.z));
        var monster = target.GetComponent<AttackingMonster>();

        var go = GameObject.Find("FX_Slash_Large_01");
        go.transform.position = this.transform.position;
        go.transform.LookAt(target.transform);
        go.transform.position += new Vector3(0f, 0.8f, 0f);
        go.transform.eulerAngles += new Vector3(0f, -60f, 180f);

        yield return YieldCache.WaitForSeconds(0.555f); //공격 애니메이션   

        SoundManager.PlayCappedSFX("ESM_Game_Saber_Sword_Slice_Impact_Stab_Knife_Spear_Arrow_Sword_Spill_Drip_Weapon_Wet_Juicy_Horror", "Action");

        yield return YieldCache.WaitForSeconds(0.2f); //공격 애니메이션
        
        var particle = go.GetComponent<ParticleSystem>();
        particle.Play();
        if (PlayerPrefs.GetInt(this.VibrationOnOff) == 1) Handheld.Vibrate();

        yield return YieldCache.WaitForSeconds(0.3f); //공격 애니메이션

        //재료 드랍
        if (monster.gameObject.activeSelf)
            StartCoroutine(monster.CoHit());

        if (this.state != Enums.ePlayerState.Attacking_Stun)
        {
            this.OnAnimationComplete();
            yield return YieldCache.WaitForSeconds(0.25f);

            if (this.state != Enums.ePlayerState.Attacking_Stun)
                this.canMove = true;
        }
    }
    public IEnumerator StunAnim()
    {
        this.canMove = false;
        this.state = Enums.ePlayerState.Attacking_Stun;
        yield return YieldCache.WaitForSeconds(3f);
        //씬 전환

        //this.canMove = true;
    }

    //===================================

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            //지역 구분
            case "Plain":   //장비 장착x
                //Debug.Log("Plain");
                this.location = Enums.ePlayerLocation.Plain;
                Destroy(this.equipement);
                //Debug.LogFormat("eq: {0}", this.equipement);
                this.equipement = null;
                PlayAnimation(Enums.ePlayerState.Idle);
                this.fishingPoint.gameObject.SetActive(false);
                this.rBody.isKinematic = false;
                break;

            case "Forest":  //도끼 장착
                //Debug.Log("숲");
                this.location = Enums.ePlayerLocation.Forest;
                PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
                if(this.equipement==null)
                {
                    var axeGo = Instantiate(this.axePrefab, this.hand_R);
                    this.equipement = axeGo;
                }
                break;

            case "Pond":  //낚싯대 장착
                //Debug.Log("낚시터");
                this.location = Enums.ePlayerLocation.Pond;
                PlayAnimation(Enums.ePlayerState.OneHandEquipe_Idle);
                this.fishingPoint.gameObject.SetActive(true);
                if (this.equipement == null)
                {
                    var fishingRodGo = Instantiate(this.fishingRodPrefab, this.hand_R);
                    this.fishingRod = fishingRodGo.GetComponent<FishingRod>();
                    this.equipement = fishingRodGo;
                }
                break;

            case "Mine": //곡괭이 장착
                //Debug.Log("광산");
                this.location = Enums.ePlayerLocation.Mine;
                PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
                if (this.equipement == null)
                {
                    var pickaxGo = Instantiate(this.pickaxPrefab, this.hand_R);
                    this.equipement = pickaxGo;
                }
                break;

            case "DreamMine": //곡괭이 장착
                //Debug.Log("꿈 광산");
                this.location = Enums.ePlayerLocation.DreamMine;
                PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
                if (this.equipement == null)
                {
                    var pickaxGo = Instantiate(this.pickaxPrefab, this.hand_R);
                    this.equipement = pickaxGo;
                }
                break;

            case "Farm":
                //Debug.Log("농장");
                this.location = Enums.ePlayerLocation.Farm;
                PlayAnimation(Enums.ePlayerState.TwoHandEquipe_Idle);
                if (this.equipement == null)
                {
                    var shovelGo = Instantiate(this.shovelPrefab, this.shovelTrans);
                    shovelGo.transform.localPosition = new Vector3(0, 0, -1);
                    this.equipement = shovelGo;
                }
                break;

            case "Dungeon":
                //Debug.Log("던전");
                if (this.location != Enums.ePlayerLocation.Dungeon)
                {
                    this.location = Enums.ePlayerLocation.Dungeon;
                    //this.rBody.isKinematic = true;
                    EventDispatcher.instance.SendEvent((int)LHMEventType.eEventType.IN_DUNGEON);
                }

                if (this.state!=Enums.ePlayerState.Attacking_Stun)
                    PlayAnimation(Enums.ePlayerState.Sword_Idle);

                if (this.equipement == null)
                {
                    var swordGo = Instantiate(this.swordPrefab, this.hand_R);
                    this.equipement = swordGo;
                }

                break;

            case "Grass":
                //Debug.Log("Grass");
                this.walkState = "Grass";
                break;

            case "Rock":
                //Debug.Log("Rock");
                this.walkState = "Rock";
                break;

            case "DreamLand":
                //Debug.Log("DreamLand");
                this.location = Enums.ePlayerLocation.Dreamland;
                this.walkState = "DreamLand";
                break;
        }
    }



    public int cnt;
    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            //드랍 아이템 구분
            case "Wood":
                //Debug.Log("나무");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_INGREDIENT, 2000);
                Destroy(collision.gameObject);
                break;

            //case "Fish":
            //    Debug.Log("물고기");
            //    EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_INGREDIENT, 2001);
            //    Destroy(collision.gameObject);
            //    break;

            case "Cabbage":
                //Debug.Log("양배추");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_INGREDIENT, 2002);
                Destroy(collision.gameObject);
                break;

            case "IronPiece":
                //Debug.Log("철");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_INGREDIENT, 2003);
                Destroy(collision.gameObject);
                break;

            case "Meat":
                //Debug.Log("고기");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_INGREDIENT, 2004);
                Destroy(collision.gameObject);
                break;

            case "DreamPiece":
                //Debug.Log("꿈 조각");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                cnt++;
                //Debug.LogFormat("<color=cyan>{0}</color>", cnt);
                Destroy(collision.gameObject);
                EventManager.instance.dreamCount(cnt);
                //Debug.LogFormat("<color=yellow>{0}</color>", EventManager.instance.dreamCount);

                break;

            case "MagicPiece":
                //Debug.Log("플레이어가 |마법의 꿈 조각| 획득");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 600);
                Destroy(collision.gameObject);
                break;

            case "WisdomPiece":
                //Debug.Log("플레이어가 |지혜의 꿈 조각| 획득");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 603);
                Destroy(collision.gameObject);
                break;

            case "DetoxPiece":
                //Debug.Log("플레이어가 |정화의 꿈 조각| 획득");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 602);
                Destroy(collision.gameObject);
                break;

            case "SpeedPiece":
                //Debug.Log("플레이어가 |신속의 꿈 조각| 획득");
                SoundManager.PlayCappedSFX("SFX_UI_Click_Designed_Pop_Generic_1", "Action");
                EventDispatcher.instance.SendEvent<int>((int)LHMEventType.eEventType.GET_DREAM_PIECE, 601);
                Destroy(collision.gameObject);
                break;

        }
    }
}
