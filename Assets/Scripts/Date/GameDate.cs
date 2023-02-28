using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameDate:BaseManager<GameDate>
{
    /// <summary>
    /// 玩家名字
    /// </summary>
    public string playerName = "MAXMIT";
    /// <summary>
    /// 玩家分值
    /// </summary>
    public int playerScore = 0;
    public int addScore = 10;
    /// <summary>
    /// 游戏时长
    /// </summary>
    public int gameTimer=120;
    /// <summary>
    /// poker资源地址
    /// </summary>
    public string poker_path = "Texture/poker20029";
    /// <summary>
    /// poker 预设位置
    /// </summary>
    public string pokerPrefab_path = "Prefab/item_poker";
    /// <summary>
    /// 桌面poker数量
    /// </summary>
    public int pokerCount = 4;

    public int gameover = 100;
   
}
