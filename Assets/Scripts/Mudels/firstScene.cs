using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using StuctsCom;
using UnityEngine.EventSystems;

public class firstScene : MonoBehaviour
{
    private Transform UI_Layer_Mid = null;

    private Text text_score = null;
    private Text text_time = null;
    private GameObject region_hall = null;
    private Transform thisTransForm = null;
    private Button btn_start = null;
    private Image btnStartSprite = null;

    private Button btn_dog = null;
    private Button btn_frog = null;
    private Button btn_tiger = null;
    private Button btn_pig = null;


    private GameMgr gameMgr = null;
    private GameDate gamedata = null;
    private PoolMgr poolManage = null;
    private ResMgr resManage = null;
    private Tools tools = null;

    TimerMgr timer_game;

    SItemData itemData;

    private int gameTimer = 0;

    private int TimerID_game;

    private bool btnStatue = false;
    private bool isApplyItem = true;



    //If the value is inactive during scenario creation or prefab instantiation,
    //this parameter is invoked when it is active and is executed only once
    private void Awake()
    {

        this.gameMgr = GameMgr.Getinstance();
        this.gamedata = GameDate.Getinstance();
        this.poolManage = PoolMgr.Getinstance();
        this.resManage = ResMgr.Getinstance();
        this.tools = Tools.Getinstance();
        this.gameMgr?.init();
        this.gameMgr?.initPoolDic();

        this.InintView();
    }

    private void InintView()
    {
        this.thisTransForm = gameObject.transform;
        this.UI_Layer_Mid = this.thisTransForm.Find("Mid");

        this.text_score = this.UI_Layer_Mid.Find("region_score/text").GetComponent<Text>();
        this.text_time = this.UI_Layer_Mid.Find("region_time/text").GetComponent<Text>();
        this.btn_start = this.UI_Layer_Mid.Find("btn_start").GetComponent<Button>();
        this.btnStartSprite = this.btn_start.GetComponent<Image>();
        this.btn_dog = this.UI_Layer_Mid.Find("btn_dog").GetComponent<Button>();
        this.btn_frog = this.UI_Layer_Mid.Find("btn_frog").GetComponent<Button>();
        this.btn_tiger = this.UI_Layer_Mid.Find("btn_tiger").GetComponent<Button>();
        this.btn_pig = this.UI_Layer_Mid.Find("btn_pig").GetComponent<Button>();

        this.region_hall = this.UI_Layer_Mid.Find("region_hall").gameObject;

    }
    //When the scene is loaded, the GameObject is instantiated,
    //and the object is enabled
    private void OnEnable()
    {
        this.InitEvent();
        this.gameTimer = this.gamedata.gameTimer;
        this.UI_Layer_Mid.gameObject.SetActive(false);
        UIManager.Getinstance().ShowPanel<panel_start>("panel_start", E_UI_Layer.Top, (panel_start panel_Start) =>{});
        

    }
    private void InitEvent()
    {

        this.btn_start?.onClick.AddListener(this.OnBtnClick);
        this.btn_dog?.onClick.AddListener(this.OnBtnClick);
        this.btn_frog?.onClick.AddListener(this.OnBtnClick);
        this.btn_tiger?.onClick.AddListener(this.OnBtnClick);
        this.btn_pig?.onClick.AddListener(this.OnBtnClick);

        EventDispatcher.Getinstance().Regist(GameDate.Getinstance().gameover, OnGameOveHandel);
        EventDispatcher.Getinstance().Regist(GameDate.Getinstance().getReward, UpdateView);
        EventDispatcher.Getinstance().Regist(GameDate.Getinstance().StartGame, UpdateView);
    }
    //Called when a new level is loaded after all scenarios have been loaded
    /*  private void OnLevelWasLoaded(int level)
      {

      }*/
    //Editor level, non play_mode script mount or active call reset to default
    private void Reset()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //initialize
        timer_game = TimerMgr.Getinstance();
        itemData = new SItemData();
        timer_game.Init();
    }

    // Update is called once per frame
    void Update()
    {
        timer_game.Update();

    }

    private void UpdateView()
    {
        if (!this.UI_Layer_Mid.gameObject.activeSelf)
        {
            this.UI_Layer_Mid.gameObject.SetActive(true);
        }
        string timeText = this.tools.ToTimeFormat(this.gameTimer, 2);
        this.text_score.text = this.gameMgr?.playerInfo.playerScore.ToString();
        this.text_time.text = "Time:" + timeText;

    }

    private void OnBtnClick()
    {
        GameObject btnObj = EventSystem.current.currentSelectedGameObject;
        string btnName = btnObj.name;
        if (btnName != "btn_start")
        {
            BtnStatusPet(false);
            OnClickDrawFun();
        }
        switch (btnName)
        {
            case "btn_start":
                this.OnClickHandel();
                break;
            case "btn_dog":
                gameMgr.selectIndex = 1;

                break;
            case "btn_frog":
                gameMgr.selectIndex = 2;

                break;
            case "btn_tiger":
                gameMgr.selectIndex = 4;

                break;
            case "btn_pig":
                gameMgr.selectIndex = 3;

                break;
            default:
                break;
        }

    }

    private void OnClickHandel()
    {
        this.BtnStatus();
        PushPool();
        this.timer_game.Unschedule(this.TimerID_game);
        this.gameTimer = this.gamedata.gameTimer;
        if (!this.btnStatue)
        {
            this.isApplyItem = true;
            this.UpdateView();
            return;
        }
        this.gameMgr.ApplyItemInObj(this.region_hall, itemData);
        this.BtnStatusPet(true);
        StartCoroutine(OnStartCoroutine());
    }

    private IEnumerator OnStartCoroutine()
    {

        yield return new WaitForSeconds(0.3f);
        OnGameReset();
        this.StartSchedul();
    }

    private void StartSchedul()
    {
        //启动定时器
        TimerID_game = timer_game.Schedule(() =>
        {
            this.gameTimer--;
            string timeText = this.tools.ToTimeFormat(this.gameTimer, 2);
            this.text_time.text = "Time:" + timeText;
            if (this.gameTimer <= 0)
            {
                OnGameReset();
            }

        }, 0, 1, 0);
    }
    private void OnGameReset()
    {
        this.timer_game.Unschedule(this.TimerID_game);
        this.gameTimer = this.gamedata.gameTimer;
        this.isApplyItem = false;
        this.UpdateView();
    }

    private void PushPool()
    {

        Transform[] transArr = region_hall.transform.GetComponentsInChildren<Transform>(true);

        foreach (Transform item in transArr)
        {
            if (item.name == this.gamedata.preName)
            {
                this.poolManage.PushObj(item.name, item.gameObject);

            }
        }
    }

    private void BtnStatus()
    {
        this.btnStatue = !this.btnStatue;
        string btnResName = this.btnStatue ? "btn05" : "btn06";
        this.btnStartSprite.sprite = this.gameMgr.GetRes<Sprite>(btnResName, this.gameMgr.obj_sprite);
        //         this.btn_start.interactable = _isShow;
        //         this.btn_start.gameObject.SetActive(_isShow);

    }
    private void BtnStatusPet(bool statueInter)
    {
        this.btn_dog.interactable = statueInter;
        this.btn_frog.interactable = statueInter;
        this.btn_tiger.interactable = statueInter;
        this.btn_pig.interactable = statueInter;

    }
    private void OnGameOveHandel()
    {
        this.BtnStatusPet(true);
        this.isApplyItem = false;

    }

    private void OnClickDrawFun()
    {
        // 随机抽中ID
        gameMgr.rewardIndex = UnityEngine.Random.Range(0, gamedata.proCount);
        
        EventDispatcher.Getinstance().DispatchEvent(GameDate.Getinstance().drawReward);
    }

    ///fixedUpdate
    /// <summary>
    /// 退出/销毁
    /// </summary>
    //Sends all objects before the program exits
    void OnApplicationQuit() { 
    }
    //Disabled or inactive
    void OnDisable() {
        this.RemoveEvent();
       
    }
    //monobe_haviour is destroyed
    void OnDestroy() {
        this.RemoveEvent();
    }

    private void RemoveEvent()
    {
        this.timer_game?.Unschedule(this.TimerID_game);
        this.btn_start?.onClick.RemoveListener(this.OnBtnClick);
        this.btn_dog?.onClick.RemoveListener(this.OnBtnClick);
        this.btn_frog?.onClick.RemoveListener(this.OnBtnClick);
        this.btn_tiger?.onClick.RemoveListener(this.OnBtnClick);
        this.btn_pig?.onClick.RemoveListener(this.OnBtnClick);
        EventDispatcher.Getinstance().UnRegist(this.gamedata.gameover, OnGameOveHandel);
        EventDispatcher.Getinstance().UnRegist(this.gamedata.getReward, UpdateView);
        EventDispatcher.Getinstance().UnRegist(this.gamedata.StartGame, UpdateView);
    }
}
