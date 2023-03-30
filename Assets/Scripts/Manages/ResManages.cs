using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// 资源加载模块
/// 1.异步加载
/// 2.委托和 lambda表达式
/// 3.协程
/// 4.泛型
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    [Tooltip("哈希表")]
    private Hashtable m_res;
    #region ResourcesLoad 构造函数
    /// <summary>
    /// 构造函数
    /// </summary>
    public void ResourcesLoad()
    {
        m_res = new Hashtable();
    }
    #endregion
    //同步加载资源
    public T Load<T>(string resPath, bool cache = false) where T : Object
    {
        StringBuilder m_Builder = new StringBuilder();

        T _res = default(T);
        if (m_res.ContainsKey(resPath))
        {
            // Debug.Log(path + "该资源来自缓存");
            _res = m_res[resPath] as T;
            cache = false;
        }
        else
        {
            m_Builder.Append(resPath);
            _res = Resources.Load<T>(m_Builder.ToString());
            if (!cache)
            {
                m_res.Add(resPath, _res);
            }
        }
        if (_res == null)
        {
            Debug.Log("Load ===>" + resPath);
        }


        //如果对象是一个GameObject类型的 我把他实例化后 再返回出去 外部 直接使用即可
        if (_res is GameObject)
        {
            return GameObject.Instantiate(_res);
        }
        else//TextAsset AudioClip{
            return _res;
    }
    #region Load 动态加载方法
    /// <summary>
    /// Load 预制体动态加载方法
    /// </summary>
    /// <param name="type"> 预制体类型</param>
    /// <param name="path">预制体名称（路径）</param>
    /// <param name="cache">是否有缓存</param>
    /// <returns>预制体实体</returns>
    public T[] LoadAll<T>(string resPath, bool cache = false) where T : Object
    {
        StringBuilder m_Builder = new StringBuilder();

        T[] resArr = default(T[]);
        if (m_res.ContainsKey(resPath))
        {
            // Debug.Log(path + "该资源来自缓存");
            resArr = m_res[resPath] as T[];
            cache = false;
        }
        else
        {
            m_Builder.Append(resPath);
            resArr = Resources.LoadAll<T>(m_Builder.ToString());
            if (!cache)
            {
                m_res.Add(resPath, resArr);
            }
        }
        if (resArr == null)
        {
            Debug.Log("Load ===>" + resPath);
        }
        return resArr;
    }
    #endregion

    //异步加载资源
    public void LoadAsync<T>(string pathName, UnityAction<T> callback) where T : Object
    {
        //开启异步加载的协程
        MonoMgr.Getinstance().StartCoroutine(ReallyLoadAsync(pathName, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string pathName, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(pathName);
        yield return r;
        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }

    #region Dispose() 释放资源
    /// <summary>
    /// Dispose() 释放资源
    /// </summary>
    public void Dispose()
    {
        base.Dispose();
        m_res.Clear();
        Resources.UnloadUnusedAssets();
    }
    #endregion
}






