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
    private Text text_ball = null;
    private Text text_ball2 = null;
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

    SItemData itemData;

    private int gameTimer = 0;

    private int maxProNum = 0;


    private int TimerID_game;

    private bool isFinish = false;


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
        this.text_ball=this.UI_Layer_Mid.Find("region_ball/text").GetComponent<Text>();
        this.text_ball2 =this.UI_Layer_Mid.Find("region_ball2/text").GetComponent<Text>();

        this.text_time = this.UI_Layer_Mid.Find("region_time/text").GetComponent<Text>();
        this.btn_start = this.UI_Layer_Mid.Find("btn_start").GetComponent<Button>();
     
        

        this.region_hall = this.UI_Layer_Mid.Find("region_hall").gameObject;

    }
    //When the scene is loaded, the GameObject is instantiated,
    //and the object is enabled
    private void OnEnable()
    {
        this.InitEvent();
        this.gameTimer = this.gamedata.gameTimer;
        this.maxProNum = this.gamedata.maxProNum;


        this.UpdateView();
        StartCoroutine(OnUpdateBallHandel(0f));

    }
    private void InitEvent()
    {

        this.btn_start?.onClick.AddListener(this.OnBtnClick);


         EventDispatcher.Getinstance().Regist(GameDate.Getinstance().getReward, UpdateBallNum);

    }
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
        //if (!this.UI_Layer_Mid.gameObject.activeSelf)
        //{
        //    this.UI_Layer_Mid.gameObject.SetActive(true);
        //}
        //string timeText = this.tools.ToTimeFormat(this.gameTimer, 2);
        this.text_ball.text = this.gameMgr.curBallNum + "/" + this.maxProNum;
        this.text_ball2.text = this.gameMgr.curBall2Num + "/" + this.maxProNum;
        this.text_score.text = this.gameMgr?.playerInfo.playerScore.ToString();
        this.text_time.text = "time:" + this.gameTimer + "s";

    }

    private void UpdateBallNum()
    {
        isFinish = this.gameMgr.curBallNum >= this.maxProNum && this.gameMgr.curBall2Num >= this.maxProNum;
       
        if (isFinish)
        {
            this.gameMgr.curBallNum = this.gameMgr.curBall2Num = this.gamedata.curProNum;

            this.gameMgr.playerInfo.playerScore+= this.gamedata.addScore;

            StartCoroutine(OnUpdateBallHandel(1.0f));
        }
        this.UpdateView();
    }

    private IEnumerator OnUpdateBallHandel(float _time)
    {
        yield return new WaitForSeconds(_time);
        btn_start.gameObject.SetActive(true);
        PushPool();
        itemData.isClick = true;
        this.gameMgr.ApplyItemInObj(this.region_hall, itemData);
    }


    private void OnBtnClick()
    {
        GameObject btnObj = EventSystem.current.currentSelectedGameObject;
        string btnName = btnObj.name;
   
        switch (btnName)
        {
            case "btn_start":
                btn_start.gameObject.SetActive(false);
                StartCoroutine(OnActiveHandel());
                break;
            default:
                break;
        }

    }
    private void OnActiveOfBucket()
    {
       
        int[] point =new int[4] { 1,3,0,2};
        for (int i = 0; i < this.gamedata.proCount; i++)
        {
            Transform item= region_hall.transform.GetChild(i);
            SItemData infoData = item.GetComponent<obj_bucket>().infoData;
           
            Vector3 start = item.position;
            Vector3 end = this.gameMgr.locationItem(point[i], this.region_hall, item.gameObject);
          
            float step = 0.5f * Time.deltaTime;

            StartCoroutine(MoveStartEnd(0.5f, item, start, end));
              /*item.localPosition = Vector3.MoveTowards(start, end, step);*/
            /*item.localPosition = new Vector3(Mathf.Lerp(start.x, end.x, step),Mathf.Lerp(start.y, end.y, step), Mathf.Lerp(start.z, end.z, step));*/
            /* item.Translate(Vector3.Normalize(end - start) *(Vector3.Distance(start, end) / (1f / Time.deltaTime)));*/

        }
    }
    private IEnumerator MoveStartEnd(float moveTime,Transform item, Vector3 start, Vector3 end)
    {
        // Loop for how ever long moveTime is.
        for (float x = 0f; x < 1.0f; x += Time.deltaTime / moveTime)
        {
            // Move to the EndLocation in a SmoothStep fashion.
            item.localPosition = new Vector3(
                Mathf.SmoothStep(start.x, end.x, x),
                Mathf.SmoothStep(start.y, end.y, x),
                Mathf.SmoothStep(start.z, end.z, x));
            yield return null;

        }
    }
    private void MoveToStart(Transform item, Vector3 start)
    {
        // Move back to the start.
        item.position = start;
    }

    private IEnumerator OnActiveHandel()
    {
        OnActiveOfBucket();
        yield return new WaitForSeconds(0.3f);
        OnUpdateHandel();
    }

    private void OnUpdateHandel()
    {
        PushPool();
        this.timer_game.Unschedule(this.TimerID_game);
        this.gameTimer = this.gamedata.gameTimer;
        itemData.isClick = false;
        this.gameMgr.ApplyItemInObj(this.region_hall, itemData);
        OnDrawFun();
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
            this.text_time.text =  "time:" + this.gameTimer + "s";
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
        btn_start.gameObject.SetActive(true);
        this.UpdateView();
    }

    private void PushPool()
    {

        Transform[] transArr = region_hall.transform.GetComponentsInChildren<Transform>();

        foreach (Transform item in transArr)
        {
            if (item.name == this.gamedata.preName)
            {
                this.poolManage.PushObj(item.name, item.gameObject);

            }
        }
    }
    private void OnGameOveHandel()
    {
  

    }

    private void OnDrawFun()
    {
        // 随机抽中ID
        gameMgr.rewardIndex = UnityEngine.Random.Range(0, gamedata.proCount);
        gameMgr.rewardIndex2 = UnityEngine.Random.Range(0, gamedata.proCount);
        while (gameMgr.rewardIndex == gameMgr.rewardIndex2)
        {
            gameMgr.rewardIndex2 = UnityEngine.Random.Range(0, gamedata.proCount);
        }
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

         EventDispatcher.Getinstance().UnRegist(this.gamedata.getReward, UpdateBallNum);
    }
}
