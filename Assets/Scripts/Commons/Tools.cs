using System;
using System.Collections;
using System.Collections.Generic;

public class Tools:BaseManager<Tools>
{
    public string GetRandom(string[] arr)
    {
        Random ran = new Random();
        int n = ran.Next(arr.Length - 1);
        return arr[n];
    }

    public int GetRandomInt(int minNum, int maxNum )
    {
        Random ran = new Random();
        return ran.Next(minNum, maxNum);
    }

    //
    public IList<IList<int>> MergeSimilarItems(int[][] items1, int[][] items2)
    {
        IDictionary<int, int> dictionary = new Dictionary<int, int>();
        foreach (int[] v in items1)
        {
            dictionary.TryAdd(v[0], 0);
            dictionary[v[0]] += v[1];
        }
        foreach (int[] v in items2)
        {
            dictionary.TryAdd(v[0], 0);
            dictionary[v[0]] += v[1];
        }

        IList<IList<int>> res = new List<IList<int>>();
        foreach (KeyValuePair<int, int> kv in dictionary)
        {
            int k = kv.Key, v = kv.Value;
            res.Add(new List<int> { k, v });
        }
        ((List<IList<int>>)res).Sort((a, b) => a[0] - b[0]);
        return res;
    }

    /// <summary>
    /// 将秒数转化为00:00:00格式
    /// </summary>
    /// <param name="time">秒数</param>
    /// <returns>00:00:00</returns>
    public string ToTimeFormat(float time, int typeTime)
    {
        //秒数取整
        int seconds = (int)time;
        //一小时为3600秒 秒数对3600取整即为小时
        int hour = seconds / 3600;
        //一分钟为60秒 秒数对3600取余再对60取整即为分钟
        int minute = seconds % 3600 / 60;
        //对3600取余再对60取余即为秒数
        seconds = seconds % 3600 % 60;
        //返回00:00:00时间格式
        string type_h = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
        string type_m = string.Format("{0:D2}:{1:D2}", minute, seconds);
        string stringType = typeTime == 1 ? type_h : type_m;
        return stringType;
    }
}