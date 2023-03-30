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
    public string resMainUrl = "Texture/main";
    public string resResultUrl = "Texture/result";
    public string resStartUrl = "Texture/start";
    /// <summary>
    /// pre url path
    /// </summary>
    public string preUrl = "Prefab/obj_pet";
    public string preName = "obj_pet";
    /// <summary>
    /// pro num
    /// </summary>
    public int proCount = 12;

    public int gameover = 100;
    public int drawReward = 101;
    public int getReward = 102;
    public int StartGame = 103;

}
