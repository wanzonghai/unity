using StuctsCom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class obj_bucket : MonoBehaviour,IPointerClickHandler
{
    private Image Image_head = null;
    private GameObject obj_tag = null;
    private Image Image_tag = null;
    private GameMgr gameMgr = null;
    private GameDate gamedata = null;
    private ResMgr resManage = null;
    private PoolMgr poolMgr = null;
    private Tools tools = null;
    private EventDispatcher eventDispatcher = null;

    private GameObject game_object = null;

    public SItemData infoData;

    private Sprite[] obj_sprite = null;

    private bool isClick = false;


    //If the value is inactive during scenario creation or prefab instantiation,
    //this parameter is invoked when it is active and is executed only once
    private void Awake()
    {
        this.gameMgr = GameMgr.Getinstance();
        this.gamedata = GameDate.Getinstance();
        this.resManage = ResMgr.Getinstance();
        this.eventDispatcher = EventDispatcher.Getinstance();
        this.poolMgr = PoolMgr.Getinstance();
        this.tools = Tools.Getinstance();
        this.game_object = gameObject;
        this.InintView();
    }

    private void InintView()
    {
        this.Image_head = gameObject.transform.Find("Image_head").GetComponent<Image>();
        this.obj_tag = gameObject.transform.Find("obj_tag").gameObject;
        this.Image_tag = this.obj_tag.transform.Find("Image_tag").GetComponent<Image>();


    }

    public void ChangeData(SItemData itemData)
    {
        
        infoData = itemData;
        this.obj_sprite = this.gameMgr.obj_sprite;
        this.isClick = itemData.isClick;
        this.InitEvent();
        this.UpdateView();
       /* this.obj_tag.SetActive((this.infoData.index - 1) == haloIndex);*/
    }

    //When the scene is loaded, the GameObject is instantiated,
    //and the object is enabled
    private void OnEnable()
    {
     
        this.obj_tag.SetActive(false);
    }
    private void InitEvent()
    {
        EventDispatcher.Getinstance().Regist(GameDate.Getinstance().drawReward, OnListenerHandel);
    }

    //Called when a new level is loaded after all scenarios have been loaded
    /*   private void OnLevelWasLoaded(int level)
       {

       }*/
    //Editor level, non-play mode script mount or active call reset to default
    private void Reset()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        //initialize
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateView()
    {
        //this.Image_tag
        string bg_name = "png_Jiutong";
        this.Image_head.sprite = this.gameMgr.GetRes<Sprite>(bg_name, this.obj_sprite);
    }

    private void OnListenerHandel()
    {
    }

    ///fixed update
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
        EventDispatcher.Getinstance().UnRegist(GameDate.Getinstance().drawReward, OnListenerHandel);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClick) {return;}
        isClick = true;
        /*int randRom_dif = (int)Math.Floor((double)this.tools.GetRandomInt(1, 5));*/
        string tag_name = "";
        if (this.infoData.index == (gameMgr.rewardIndex + 1))
        {
            tag_name = "png_qiu" + 1;
            this.gameMgr.curBallNum++;
        }
        else if (this.infoData.index == (gameMgr.rewardIndex2 + 1))
        {
            tag_name = "png_qiu" + 2;
            this.gameMgr.curBall2Num++;
        }
        this.obj_tag.SetActive(tag_name.Length > 0);
        if (tag_name.Length > 0)
        {
            this.Image_tag.sprite = this.gameMgr.GetRes<Sprite>(tag_name, this.obj_sprite);
            EventDispatcher.Getinstance().DispatchEvent(GameDate.Getinstance().getReward);
        }

        /*throw new NotImplementedException();*/
    }
}
