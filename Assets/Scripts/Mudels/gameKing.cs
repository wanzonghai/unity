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

    //��������ʱ��prefabʵ����ʱ����Ϊinactive����activeʱ���ã���ִ��һ��
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
    //���������꣬GameObjectʵ����ʱ������enabledʱ
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
    //����ȫ��������ɺ� �����¼���ʱ���� this is removed
    /*   private void OnLevelWasLoaded(int level)
    {
        
        
    }*/
    //Editor���𣬷�playmode�½ű�����ʱ���������� ����ΪĬ��ֵ
    private void Reset()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //��ʼ��

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
            //Ԥ����Ԥ������Դ����,��ʵ������
            GameObject item_poker = PoolManage.Getinstance().Getobj(GameDate.Getinstance().pokerPrefab_path);

            //ʵ����������������
            item_poker.GetComponent<item_poker>().ChangeDate(showPokerName_arr[i]);
            /*item_poker.transform.parent = layoutGroup.transform;*/
            item_poker.transform.SetParent(layoutGroup.transform);

        }
        //������ʱ��
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
    /*ÿ֡����֡�ʸߵͿ��ܶ�ε���Ҳ���ܲ����ã�ִ�к�������ʼ�������͸��£����ƶ�����ʱ����ʹ��Time.deltaTime
    ״̬������״̬������
    OnStateMachineEnter
    ������һ������ͼ���ϵ�״̬���״ν���һ��״̬ʱ�ĵ�һ֡����
    OnStateMachineExit
    �˳�״̬�����һ֡����
    ����ͼ��
    �������ж���ͼ��
    ���������¼� �ڸ�ʱ���ڵ�ǰ֡�����һ֮֡�䣬�������ж���Ƭ�εĶ����¼�
    ״̬���ڻص���OnStateEnter/OnStateUpdate/OnStateExit�� һ��״̬�������������Ծ״̬��current state, interrupted state, and next state����Ӧ�׶����ڷ���˳��ִ��*/

    /// <summary>
    /// �˳�/����
    /// </summary>
    //�����˳�ǰ�����ж���
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
    //���û��ڷǻ״̬
    void OnDisable() {
        isFinishGame = false;
        isCanStar = false;
        if (btn_start)
        {
            btn_start.onClick.RemoveListener(onBtnClick);
        }
        EventDispatcher.Getinstance().UnRegist(GameDate.Getinstance().gameover, OnGameOveHandel);
    }
    //monobehaviour������
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
