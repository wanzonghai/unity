using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    [Tooltip("��ϣ��")]
    private Hashtable m_res;
    #region ResourcesLoad ���캯��
    /// <summary>
    /// ���캯��
    /// </summary>
    public void ResourcesLoad()
    {
        m_res = new Hashtable();
    }
    #endregion
    //ͬ��������Դ
    public T Load<T>(string resPath, bool cache = false) where T : Object
    {
        StringBuilder m_Builder = new StringBuilder();

        T _res = default(T);
        if (m_res.ContainsKey(resPath))
        {
            // Debug.Log(path + "����Դ���Ի���");
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


        //���������һ��GameObject���͵� �Ұ���ʵ������ �ٷ��س�ȥ �ⲿ ֱ��ʹ�ü���
        if (_res is GameObject)
        {
            return GameObject.Instantiate(_res);
        }
        else//TextAsset AudioClip{
            return _res;
    }
    #region Load ��̬���ط���
    /// <summary>
    /// Load Ԥ���嶯̬���ط���
    /// </summary>
    /// <param name="type"> Ԥ��������</param>
    /// <param name="path">Ԥ�������ƣ�·����</param>
    /// <param name="cache">�Ƿ��л���</param>
    /// <returns>Ԥ����ʵ��</returns>
    public T[] LoadAll<T>(string resPath, bool cache = false) where T : Object
    {
        StringBuilder m_Builder = new StringBuilder();

        T[] resArr = default(T[]);
        if (m_res.ContainsKey(resPath))
        {
            // Debug.Log(path + "����Դ���Ի���");
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

    //�첽������Դ
    public void LoadAsync<T>(string pathName, UnityAction<T> callback) where T : Object
    {
        //�����첽���ص�Э��
        MonoMgr.Getinstance().StartCoroutine(ReallyLoadAsync(pathName, callback));
    }

    //������Эͬ������  ���� �����첽���ض�Ӧ����Դ
    private IEnumerator ReallyLoadAsync<T>(string pathName, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(pathName);
        yield return r;
        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }

    #region Dispose() �ͷ���Դ
    /// <summary>
    /// Dispose() �ͷ���Դ
    /// </summary>
    public void Dispose()
    {
        base.Dispose();
        m_res.Clear();
        Resources.UnloadUnusedAssets();
    }
    #endregion
}






