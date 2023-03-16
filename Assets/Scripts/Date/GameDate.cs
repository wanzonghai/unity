using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameDate:BaseManager<GameDate>
{
    /// <summary>
    /// player name
    /// </summary>
    public string playerName = "MAXMIT";
    /// <summary>
    /// player score
    /// </summary>
    public int playerScore = 0;
    public int addScore = 10;
    /// <summary>
    /// game time
    /// </summary>
    public int gameTimer=90;
    /// <summary>
    /// Atlas url path
    /// </summary>
    public string atlasUrl = "Texture/gsSprite";
    /// <summary>
    /// pre url path
    /// </summary>
    public string preUrl = "Prefab/obj_cxk";
    public string preName = "obj_cxk";
    /// <summary>
    /// pro num
    /// </summary>
    public int proCount = 4;

    public int gameover = 100;
   
}
