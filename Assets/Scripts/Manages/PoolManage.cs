using System.Collections.Generic;
using UnityEngine;

public class PoolManage : BaseManager<PoolManage>
{
    public Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
   
    public GameObject Getobj(string name)
    {
        GameObject obj = null;
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            obj = poolDic[name][0];
            poolDic[name].RemoveAt(0);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name;//�ܹؼ����Ѷ������ĳɳ�������һ����ֹ��clone���µĴ���

        }
        obj.SetActive(true);
        return obj;
    }

    public void PushObj(string name, GameObject obj)
    {
        obj.SetActive(false);
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].Add(obj);
        }
        else
        {
            poolDic.Add(name, new List<GameObject>() { obj });
        }
    }

}
