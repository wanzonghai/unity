using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using StuctsCom;

public class firstScene : MonoBehaviour
{
    private Text text_score = null;
    private Text text_time = null;
    private GameObject region_hall = null;
    private Transform thisTransForm = null;
    private Button btn_start = null;

    private GameMgr gameMgr = null;
    private GameDate gamedata = null;
    private PoolMgr poolManage = null;
    private ResMgr resManage = null;
    private Tools tools = null;

    TimerMgr timer_game;

    private int gameTimer = 0;

    private int TimerID_game;

    private SCXKData cxkData ;

    private bool isFinishGame = false;

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
        this.text_score = this.thisTransForm.Find("region_score/text").GetComponent<Text>();
        this.text_time = this.thisTransForm.Find("region_time/text").GetComponent<Text>();
        this.btn_start = this.thisTransForm.Find("btn_start").GetComponent<Button>();
        this.region_hall = this.thisTransForm.Find("region_hall").gameObject;

    }
    //When the scene is loaded, the GameObject is instantiated,
    //and the object is enabled
    private void OnEnable()
    {
        this.InitEvent();
        this.gameTimer = this.gamedata.gameTimer;
        this.UpdateView();

    }
    private void InitEvent()
    {
        this.btn_start?.onClick.AddListener(this.OnBtnClick);
        EventDispatcher.Getinstance().Regist(GameDate.Getinstance().gameover, OnGameOveHandel);
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

        timer_game.Init();
    }

    // Update is called once per frame
    void Update()
    {
        timer_game.Update();
    }

    private void UpdateView()
    {
        this.text_score.text = this.gameMgr?.playerInfo.playerScore.ToString();
        this.text_time.text = "Time:" + this.gameTimer + "S";
    }

    private void OnBtnClick()
    {
        this.PushPool();
        this.BtnStatus(false);
        int randRom_dif = (int)Math.Floor((double)this.tools.GetRandomInt(1, 3));
        int randRom = 0;
        while (randRom_dif== randRom|| randRom==0)
        {
            randRom = (int)Math.Floor((double)this.tools.GetRandomInt(1, 3));
        }
        this.cxkData.randDif = randRom_dif;
        this.cxkData.rand = randRom;
        for (int i = 0; i < 3; i++)
        {
            int indexI = i + 1;
            GameObject _obj = null;
            this.poolManage.GetObj(this.gamedata.preName,this.gamedata.preUrl,(obj)=> {
                _obj = obj;
                _obj.transform.SetParent(this.region_hall.transform);
                this.cxkData.index = indexI;
                _obj.transform.GetComponent<obj_cxk>().ChangeData(this.cxkData);
                int _x = (indexI - 2) * 600;
                int _y = (indexI-1) == 1 ? 0 : 60;
                _obj.transform.SetLocalPositionAndRotation(new Vector3(_x, _y, 0), new Quaternion(0, 0, 0, 0));
            });
            

        }
        this.StartSchedul();

    }

    private void StartSchedul()
    {
        //启动定时器
        TimerID_game= timer_game.Schedule(() =>
        {
            this.gameTimer--;
            this.text_time.text = "Time:" + this.gameTimer + "S";
            if (this.gameTimer <= 0)
            {
                OnGameReset();
            }

        }, 0, 1, 0);
    }
    private void OnGameReset()
    {
        this.timer_game.Unschedule(this.TimerID_game);
        this.BtnStatus(true);
        this.gameTimer = this.gamedata.gameTimer;
        this.UpdateView();
      
    }

    private void PushPool()
    {
        int len = this.region_hall.transform.childCount;
        for (int i = 0; i < len; i++)
        {
            if (this.region_hall.transform.GetChild(i) != null)
            {
                GameObject _childObj = this.region_hall.transform.GetChild(i).gameObject;
                this.poolManage.PushObj(_childObj.name, _childObj);
                Destroy(this.region_hall.transform.GetChild(i).gameObject);
            }
        }

    }

    private void BtnStatus(bool _isShow)
    {
        this.btn_start.interactable = _isShow;
        this.btn_start.gameObject.SetActive(_isShow);

    }
    private void OnGameOveHandel()
    {
        this.gameMgr.playerInfo.playerScore += this.gamedata.addScore;
        this.OnGameReset();
    }
    ///fixedupdate
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
        EventDispatcher.Getinstance().UnRegist(this.gamedata.gameover, OnGameOveHandel);
    }
}
