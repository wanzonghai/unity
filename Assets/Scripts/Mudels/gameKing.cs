using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class gameKing : MonoBehaviour
{
    private GameObject region_pair = null;
    private GameObject region_single = null;
    private GameObject layoutGroup = null;
    private Text text_time = null;
    private Image Image_head = null;
    private TMP_Text text_name = null;
    private TMP_Text text_score = null;
    private Button btn_start = null;


    TimerMgr timer_game;

    int TimerID_game;

    private List<string> pokerName_arr = null;
    private List<string> showPokerName_arr = null;

    private bool isCanStar=false;

    private bool isFinishGame = false;

    //场景创建时或prefab实例化时，若为inactive则在active时调用，仅执行一次
    private void Awake()
    {
        GameMgr.Getinstance().init();
        inintView();
        if (pokerName_arr==null)
        {
            pokerName_arr = new List<string>();
        }
        if (showPokerName_arr == null)
        {
            showPokerName_arr = new List<string>();
        }
    }

    private void inintView()
    {
        region_pair = GameObject.Find("region_pair");
     
        region_single = GameObject.Find("region_single");
        layoutGroup = GameObject.Find("layoutGroup");
        text_time = GameObject.Find("time_bg/text_time").GetComponent<Text>();
        Image_head = GameObject.Find("player_bg/Image_head").GetComponent<Image>();
        text_name = GameObject.Find("player_bg/text_name").GetComponent<TMP_Text>();
        text_score = GameObject.Find("player_bg/text_score").GetComponent<TMP_Text>();
        btn_start = GameObject.Find("btn_start").GetComponent<Button>();
        
    }
    //场景加载完，GameObject实例化时，对象enabled时
    private void OnEnable()
    {
        isCanStar = false;
        if (btn_start)
        {
            btn_start.onClick.AddListener(onBtnClick);
        }
        EventDispatcher.Getinstance().Regist(GameDate.Getinstance().gameover,OnGameOveHandel);

        UpdateView();

    }
    //场景全部加载完成后 加载新级别时调用 this is removed
    /*   private void OnLevelWasLoaded(int level)
    {
        
        
    }*/
    //Editor级别，非playmode下脚本挂载时或主动调用 重置为默认值
    private void Reset()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //初始化

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
        
        text_name.text = GameDate.Getinstance().playerName;    
        text_score.text="MARK:"+ GameDate.Getinstance().playerScore;

        string poker_path = GameDate.Getinstance().poker_path;

        if (TextureMgr.Getinstance().m_pAtlasDic.ContainsKey(poker_path))
        {
            Dictionary<string, UnityEngine.Object[]> m_pAtlasDic = TextureMgr.Getinstance().m_pAtlasDic;
            UnityEngine.Object[] _atlas = m_pAtlasDic[poker_path];
            if (showPokerName_arr == null || showPokerName_arr == null) return;
            do
            {
                for (int i = 0; i < _atlas.Length; i++)
                {
                    pokerName_arr.Add(_atlas[i].name);
                }
                string _randnum = GetRandom(pokerName_arr);
                /*Debug.Log(_randnum);*/
                showPokerName_arr.Add(_randnum);
            } while (showPokerName_arr.Count < 4);

            isCanStar = true;
        }

    }

    private void onBtnClick()
    {
        if (!btn_start.interactable)  {Debug.LogWarning("res is undefinde"); return;}
         Debug.Log("Game Star");
        btn_start.interactable = false;
        
        
        for (int i = 0; i < showPokerName_arr.Count; i++)
        {
            //预设体预设体资源加载,并实例化：
            GameObject item_poker = PoolManage.Getinstance().Getobj(GameDate.Getinstance().pokerPrefab_path);

            //实例化对象属性设置
            item_poker.GetComponent<item_poker>().ChangeDate(showPokerName_arr[i]);
            /*item_poker.transform.parent = layoutGroup.transform;*/
            item_poker.transform.SetParent(layoutGroup.transform);

        }
        //启动定时器
        TimerID_game = timer_game.Schedule(() => {
            GameDate.Getinstance().gameTimer--;
            text_time.text = "TIME:" + GameDate.Getinstance().gameTimer + "S";
            if (GameDate.Getinstance().gameTimer <= 0)
            {
                OnGameReset();
                if (!isFinishGame)
                {
                    OnTimeOverHandel();
                }
            }

        }, 0, 1, 0);
    }

    ///fixedupdate
    /*每帧根据帧率高低可能多次调用也可能不调用，执行后立即开始物理计算和更新，做移动计算时无需使用Time.deltaTime
    状态机周期状态机更新
    OnStateMachineEnter
    挂载在一个动画图形上的状态机首次进入一个状态时的第一帧调用
    OnStateMachineExit
    退出状态的最后一帧调用
    处理图形
    评估所有动画图形
    触发动画事件 在该时间内当前帧和最后一帧之间，触发所有动画片段的动画事件
    状态周期回调（OnStateEnter/OnStateUpdate/OnStateExit） 一个状态层有最多三个活跃状态，current state, interrupted state, and next state，对应阶段周期方法顺序执行*/

    /// <summary>
    /// 退出/销毁
    /// </summary>
    //程序退出前发所有对象
    void OnApplicationQuit() { 
    }
    private void OnGameReset()
    {
        timer_game.Unschedule(TimerID_game);
        GameDate.Getinstance().gameTimer = 120;
        btn_start.interactable = true;
        isCanStar = true;
        text_time.text = "TIME:" + GameDate.Getinstance().gameTimer + "S";
    }
   private void OnGameOveHandel()
    {
        OnGameReset();
        isFinishGame = true;
        GameDate.Getinstance().playerScore += GameDate.Getinstance().addScore;
        text_score.text = "MARK:" + GameDate.Getinstance().playerScore;
    }
    private void OnTimeOverHandel()
    {
        btn_start.interactable = true;
    }
    //禁用或处于非活动状态
    void OnDisable() {
        isFinishGame = false;
        isCanStar = false;
        if (btn_start)
        {
            btn_start.onClick.RemoveListener(onBtnClick);
        }
        EventDispatcher.Getinstance().UnRegist(GameDate.Getinstance().gameover, OnGameOveHandel);
    }
    //monobehaviour被销毁
    void OnDestroy() {
        isFinishGame = false;
        isCanStar = false;
        if (btn_start)
        {
            btn_start.onClick.RemoveListener(onBtnClick);
        }
        EventDispatcher.Getinstance().UnRegist(GameDate.Getinstance().gameover, OnGameOveHandel);
    }

    private string GetRandom(List<string> arr)
    {
        Random ran = new Random();
        int n = ran.Next(arr.Count - 1);
        return arr[n];
    }

}
