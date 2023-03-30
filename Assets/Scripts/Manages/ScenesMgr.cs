using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�ģ��
/// ֪ʶ��
/// 1.�����첽����
/// 2.Э��
/// 3.ί��
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// �л����� ͬ��
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name, UnityAction fun)
    {
        //����ͬ������
        SceneManager.LoadScene(name);
        //������ɹ��� �Ż�ȥִ��fun
        fun();
    }

    /// <summary>
    /// �ṩ���ⲿ�� �첽���صĽӿڷ���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsyn(string name, UnityAction fun)
    {
        MonoMgr.Getinstance().StartCoroutine(ReallyLoadSceneAsyn(name, fun));
    }

    /// <summary>
    /// Э���첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //���Եõ��������ص�һ������
        while (!ao.isDone)
        {
            //�¼����� ����ַ� �������  �������þ���
            EventCenter.Getinstance().EventTrigger("����������", ao.progress);
            //������ȥ���½�����
            yield return ao.progress;
        }
        //������ɹ��� �Ż�ȥִ��fun
        fun();
    }
}
