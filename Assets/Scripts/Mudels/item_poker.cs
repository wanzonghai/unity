using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//IPointerDownHandler
public class item_poker : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //item 数据
    [HideInInspector]
    public string item_info = "1_1";

    private Image item_sprite = null;

    [Header("表示限制的区域")]
    public RectTransform LimitContainer;
    [Header("场景中Canvas")]
    public Canvas canvas;

    private GameObject layoutGroup;
    private GameObject region_pair;
    private GameObject region_single;

    private RectTransform rt;
    // 位置偏移量
    private Vector3 offset = Vector3.zero;
    // 最小、最大X、Y坐标
    private float minX, maxX, minY, maxY;
    //最初一次的记录点
    private Vector3 starPoint;
    //最后一次的记录点
    private Vector3 lastPoint;

    private Transform orignaParent;

    [HideInInspector]
    private Vector2 offsetPos;  //临时记录点击点与UI的相对位置
    private void Awake()
    {

        layoutGroup = GameObject.Find("Canvas/layoutGroup");
        region_pair = GameObject.Find("Canvas/region_pair");
        region_single = GameObject.Find("Canvas/region_single");
        GameObject _canvas = GameObject.Find("Canvas").gameObject;
        canvas = _canvas.GetComponent<Canvas>();
        LimitContainer = _canvas.GetComponent<RectTransform>();
        item_sprite = gameObject.GetComponent<Image>();

       
    }
    private void OnEnable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //Input.multiTouchEnabled = true;//开启多点触碰
        // mouse相关鼠标点击效果，下列函数中的参数，0是鼠标左键，1是鼠标右键，2是鼠标中键
        rt = GetComponent<RectTransform>();

        starPoint= lastPoint =Camera.main.ScreenToWorldPoint( rt.position);

        UpdateView();
    }
    public void ChangeDate(string _data)
    {
        item_info = (_data.Length > 0) ? _data : "1-1";

       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void UpdateView()
    {
        string poker_path = GameDate.Getinstance().poker_path;
        Sprite sprite = TextureMgr.Getinstance().LoadResAtlasSprite(poker_path, item_info);
        item_sprite.sprite = sprite;
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
        {
            orignaParent = rt.parent;//确定初始父节点
            transform.SetParent(orignaParent.parent);//改变父节点
            // 计算偏移量
            offset = rt.position - globalMousePos;
            // 设置拖拽范围
            SetDragRange();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position - offsetPos;
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        // 将屏幕空间上的点转换为位于给定RectTransform平面上的世界空间中的位置
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
        {
            rt.position = DragRangeLimit(globalMousePos + offset);
            lastPoint = rt.position;
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offsetPos = eventData.position - (Vector2)transform.position;
    }
    // 设置最大、最小坐标
    void SetDragRange()
    {
        // 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
        minX = LimitContainer.position.x
            - LimitContainer.pivot.x * LimitContainer.rect.width * canvas.scaleFactor
            + rt.rect.width * canvas.scaleFactor * rt.pivot.x;
        // 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
        maxX = LimitContainer.position.x
            + (1 - LimitContainer.pivot.x) * LimitContainer.rect.width * canvas.scaleFactor
            - rt.rect.width * canvas.scaleFactor * (1 - rt.pivot.x);

        // 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
        minY = LimitContainer.position.y
            - LimitContainer.pivot.y * LimitContainer.rect.height * canvas.scaleFactor
            + rt.rect.height * canvas.scaleFactor * rt.pivot.y;

        // 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
        maxY = LimitContainer.position.y
            + (1 - LimitContainer.pivot.y) * LimitContainer.rect.height * canvas.scaleFactor
            - rt.rect.height * canvas.scaleFactor * (1 - rt.pivot.y);
    }
    // 限制坐标范围
    Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Bounds bounds_layout = layoutGroup.GetComponent<Collider2D>().bounds;
        Bounds bounds_pair = region_pair.GetComponent<Collider2D>().bounds;
        Bounds bounds_single = region_single.GetComponent<Collider2D>().bounds;
        bool inPair = bounds_pair.Contains(lastPoint);
        bool inSingle = bounds_single.Contains(lastPoint);
        int pokerInfo = item_info.Length>0?  int.Parse(item_info.Split("-")[0]):1;
        if (inPair&& pokerInfo % 2 == 0|| inSingle && pokerInfo % 2 != 0)
        {
            pushPool();
        }
        else
        {
            transform.SetParent(orignaParent);//恢复父节点
        }
        if (orignaParent.childCount <= 0)
        {
            EventDispatcher.Getinstance().DispatchEvent(GameDate.Getinstance().gameover);
           // EventDispatcher.Getinstance().DispatchEvent<string>(GameDate.Getinstance().gameover, "st");
        }
       
    }

    private void pushPool()
    {
        PoolManage.Getinstance().PushObj(GameDate.Getinstance().poker_path, rt.gameObject);
        
    }
}
