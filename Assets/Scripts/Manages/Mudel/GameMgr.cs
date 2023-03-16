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
   public void init()
    {
        ResMgr resMgr = ResMgr.Getinstance();
        GameDate gameDate = GameDate.Getinstance();

        resMgr.ResourcesLoad();
        resMgr.LoadAll<Sprite>(gameDate.atlasUrl);

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
   
}
