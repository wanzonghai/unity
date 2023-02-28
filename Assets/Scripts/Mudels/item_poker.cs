using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//IPointerDownHandler
public class item_poker : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //item ����
    [HideInInspector]
    public string item_info = "1_1";

    private Image item_sprite = null;

    [Header("��ʾ���Ƶ�����")]
    public RectTransform LimitContainer;
    [Header("������Canvas")]
    public Canvas canvas;

    private GameObject layoutGroup;
    private GameObject region_pair;
    private GameObject region_single;

    private RectTransform rt;
    // λ��ƫ����
    private Vector3 offset = Vector3.zero;
    // ��С�����X��Y����
    private float minX, maxX, minY, maxY;
    //���һ�εļ�¼��
    private Vector3 starPoint;
    //���һ�εļ�¼��
    private Vector3 lastPoint;

    private Transform orignaParent;

    [HideInInspector]
    private Vector2 offsetPos;  //��ʱ��¼�������UI�����λ��
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
        //Input.multiTouchEnabled = true;//������㴥��
        // mouse��������Ч�������к����еĲ�����0����������1������Ҽ���2������м�
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
    /// ��ʼ��ק
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
        {
            orignaParent = rt.parent;//ȷ����ʼ���ڵ�
            transform.SetParent(orignaParent.parent);//�ı丸�ڵ�
            // ����ƫ����
            offset = rt.position - globalMousePos;
            // ������ק��Χ
            SetDragRange();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position - offsetPos;
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        // ����Ļ�ռ��ϵĵ�ת��Ϊλ�ڸ���RectTransformƽ���ϵ�����ռ��е�λ��
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
    // ���������С����
    void SetDragRange()
    {
        // ��Сx���� = ������ǰx���� - �������ľ�����߽�ľ��� + UI���ľ�����߽�ľ���
        minX = LimitContainer.position.x
            - LimitContainer.pivot.x * LimitContainer.rect.width * canvas.scaleFactor
            + rt.rect.width * canvas.scaleFactor * rt.pivot.x;
        // ���x���� = ������ǰx���� + �������ľ����ұ߽�ľ��� - UI���ľ����ұ߽�ľ���
        maxX = LimitContainer.position.x
            + (1 - LimitContainer.pivot.x) * LimitContainer.rect.width * canvas.scaleFactor
            - rt.rect.width * canvas.scaleFactor * (1 - rt.pivot.x);

        // ��Сy���� = ������ǰy���� - �������ľ���ױߵľ��� + UI���ľ���ױߵľ���
        minY = LimitContainer.position.y
            - LimitContainer.pivot.y * LimitContainer.rect.height * canvas.scaleFactor
            + rt.rect.height * canvas.scaleFactor * rt.pivot.y;

        // ���y���� = ������ǰx���� + �������ľ��붥�ߵľ��� - UI���ľ��붥�ߵľ���
        maxY = LimitContainer.position.y
            + (1 - LimitContainer.pivot.y) * LimitContainer.rect.height * canvas.scaleFactor
            - rt.rect.height * canvas.scaleFactor * (1 - rt.pivot.y);
    }
    // �������귶Χ
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
            transform.SetParent(orignaParent);//�ָ����ڵ�
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
