using StuctsCom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class obj_cxk: MonoBehaviour,IPointerClickHandler
{
    private Image Image_head = null;
    private Image Image_tag = null;
    private GameMgr gameMgr = null;
    private GameDate gamedata = null;
    private ResMgr resManage = null;
    private PoolMgr poolMgr = null;

    private EventDispatcher eventDispatcher = null;

    private GameObject game_object = null;

    private SCXKData infoData;

    private Sprite[] obj_sprite = null;

    //If the value is inactive during scenario creation or prefab instantiation,
    //this parameter is invoked when it is active and is executed only once
    private void Awake()
    {
        this.gameMgr = GameMgr.Getinstance();
        this.gamedata = GameDate.Getinstance();
        this.resManage = ResMgr.Getinstance();
        this.eventDispatcher = EventDispatcher.Getinstance();
        this.poolMgr = PoolMgr.Getinstance();
        this.game_object = gameObject;
        this.InintView();
    }

    private void InintView()
    {
        this.Image_head = gameObject.transform.Find("Image_head").GetComponent<Image>();
        this.Image_tag= gameObject.transform.Find("Image_bg/Image_tag").GetComponent<Image>();

       
    }

    public void ChangeData(SCXKData cxkData)
    {
        infoData = cxkData;
        this.InitEvent();
        this.UpdateView();
    }
    //When the scene is loaded, the GameObject is instantiated,
    //and the object is enabled
    private void OnEnable()
    {

    }
    private void InitEvent()
    {
      
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
        this.obj_sprite = this.resManage.LoadAll<Sprite>(this.gamedata.atlasUrl);
        this.Image_head.sprite = this.gameMgr.GetRes<Sprite>("png_niao" + this.infoData.index, this.obj_sprite);
      
        string tag_name = "png_tuxing" + (this.infoData.index == this.infoData.randDif ? this.infoData.randDif : this.infoData.rand);
        this.Image_tag.sprite = this.gameMgr.GetRes<Sprite>( tag_name, this.obj_sprite);
      
    }

    private void OnBtnClick()
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
       
        /*  this.poolMgr.PushObj(this.game_object.name,this.game_object);*/
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.infoData.index != this.infoData.randDif) return;
        this.Image_tag.sprite = this.gameMgr.GetRes<Sprite>("png_tuxing" + this.infoData.rand, this.obj_sprite);
        this.eventDispatcher.DispatchEvent(this.gamedata.gameover);
    }
}
