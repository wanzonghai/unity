using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// ��Դ����ģ��
/// 1.�첽����
/// 2.ί�к� lambda���ʽ
/// 3.Э��
/// 4.����
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    
    //ͬ��������Դ
    public T Load<T>(string pathName) where T : Object
    {
        T res = Resources.Load<T>(pathName);
        //���������һ��GameObject���͵� �Ұ���ʵ������ �ٷ��س�ȥ �ⲿ ֱ��ʹ�ü���
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset AudioClip
            return res;
    }
    //�첽������Դ
    public void LoadAsync<T>(string pathName, UnityAction<T> callback) where T : Object
    {
        //�����첽���ص�Э��
        MonoMgr.Getinstance().StartCoroutine(ReallyLoadAsync(pathName, callback));
    }

    //������Эͬ������  ���� �����첽���ض�Ӧ����Դ
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;
        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }


}






