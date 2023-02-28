using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class GameMgr : BaseManager<GameMgr>
{
    public string playerInfo;
    public void init()
    {
        TextureMgr textureMgr = TextureMgr.Getinstance();
        GameDate gameDate = GameDate.Getinstance();
        textureMgr.init();
        if (textureMgr.m_pAtlasDic != null)
        {
            textureMgr.LoadResAtlas(gameDate.poker_path);
        }
    }
   
}
