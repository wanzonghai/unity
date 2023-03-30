using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using StuctsCom;

public class GameMgr : BaseManager<GameMgr>
{
    /// <summary>
    /// player data;
    /// </summary>
    public SPlayerData playerInfo;
    public Sprite[] obj_sprite = null;
    GameObject _obj = null;
    //random id
    public int rewardIndex = 0;
    //random2 id
    public int rewardIndex2 = 0;
    //select id
    public int selectIndex = 0;

    public int curBallNum=0;
    public int curBall2Num = 0;

    public bool drawReward = false;

    public void init()
    {
        ResMgr resMgr = ResMgr.Getinstance();
        GameDate gameDate = GameDate.Getinstance();

        resMgr.ResourcesLoad();
        this.obj_sprite= resMgr.LoadAll<Sprite>(gameDate.resMainUrl);

        playerInfo.name = gameDate.playerName;
        playerInfo.playerScore = gameDate.playerScore; 
    }

    public  void initPoolDic()
    {
        PoolMgr poolMgr = PoolMgr.Getinstance();
        GameDate gameDate = GameDate.Getinstance();
        for (int i = 0; i < 3; i++)
        {
            poolMgr.GetObj(gameDate.preName, gameDate.preUrl, (gameObject) =>{
                poolMgr.PushObj(gameDate.preName, gameObject);
            });
        }

    }

    public T GetRes<T>(string resName,T[] resArr ) where T : Object
    {
        T _res = default(T);
        for (int i = 0; i < resArr.Length; i++)
        {
            if(resArr[i].name== resName)
            {
                _res = resArr[i];
            }
        }
        return _res;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemCount"></param>
    /// <param name="parentObj"></param>
    /// <param name="itemData"></param>
    public void ApplyItemInObj(GameObject parentObj,SItemData itemData)
    {
        PoolMgr poolMgr = PoolMgr.Getinstance();
        ResMgr resMgr = ResMgr.Getinstance();
        GameDate gameData = GameDate.Getinstance();
     
        for (int i = 0; i < gameData.proCount; i++)
        {
            int indexI = i + 1;
            
            poolMgr.GetObj(gameData.preName, gameData.preUrl, (obj) =>
            {
                if (!obj) return;
                _obj = obj;
                itemData.index = indexI;
                _obj.transform.GetComponent<obj_bucket>().ChangeData(itemData);
                _obj.transform.SetParent(parentObj.transform);
                
                _obj.transform.SetLocalPositionAndRotation(locationItem(indexI - 1, parentObj, _obj), new Quaternion(0, 0, 0, 0));
            });
        }
    }

    public Vector3 locationItem(int i,GameObject parentObj,GameObject _obj)
    {
        Vector3 _vv3 = new Vector3();
        RectTransform parentRect= parentObj.GetComponent<RectTransform>();
        RectTransform _objRect = _obj.GetComponent<RectTransform>();
        float x1 = (parentObj.transform.localPosition.x- parentRect.rect.width/2) + parentRect.rect.width / 4 * (i+1) - _objRect.rect.width*0.85f;
        float y1 = (parentObj.transform.localPosition.y - parentRect.rect.height / 2)+ _objRect.rect.height / 2;
        _vv3.x = x1;
        _vv3.y = y1;
        //         float x1 = parentObj.transform.localPosition.x- parentRect.rect.width / 2+ _objRect.rect.width/2;
        //         float x2 = x1 + 3* parentRect.rect.width/4;
        //         float y1 = parentObj.transform.localPosition.y + parentRect.rect.height / 2 - _objRect.rect.height / 2;
        //         float y2 = y1-3* parentRect.rect.height/4;
        //        
        //         _vv3.x = i < 4 ? (x1 + _objRect.rect.width * i) : i < 7 ? x2 : i < 10 ? (x2 - _objRect.rect.width * (i - 6)) : x1;
        //         _vv3.y = i < 4 ? y1 : i < 7 ? (y1 - parentRect.rect.height/4 * (i - 3)) : i < 10 ? y2 : (y2 + parentRect.rect.height / 4 * (i - 9));
        return _vv3;
    }
}
