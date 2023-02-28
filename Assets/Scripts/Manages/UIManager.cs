using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UI�㼶
/// </summary>
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}

/// <summary>
/// UI������
/// 1.����������ʾ�����
/// 2.�ṩ���ⲿ ��ʾ�����صȵȽӿ�
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    //��¼����UI��Canvas������ �����Ժ��ⲿ���ܻ�ʹ����
    public RectTransform canvas;

    public UIManager()
    {
        //����Canvas �����������ʱ�� �����Ƴ�
        GameObject obj = ResMgr.Getinstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        //�ҵ�����
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //����EventSystem �����������ʱ�� �����Ƴ�
        obj = ResMgr.Getinstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// ͨ���㼶ö�� �õ���Ӧ�㼶�ĸ�����
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <typeparam name="T">���ű�����</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="layer">��ʾ����һ��</param>
    /// <param name="callBack">�����Ԥ���崴���ɹ��� ����������</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            // ������崴����ɺ���߼�
            if (callBack != null)
                callBack(panelDic[panelName] as T);
            //��������ظ����� ������ڸ���� ��ֱ����ʾ ���ûص�������  ֱ��return ���ٴ��������첽�����߼�
            return;
        }

        ResMgr.Getinstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //������Ϊ Canvas���Ӷ���
            //���� Ҫ�����������λ��
            //�ҵ������� �㵽����ʾ����һ��
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            //���ø�����  �������λ�úʹ�С
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //�õ�Ԥ�������ϵ����ű�
            T panel = obj.GetComponent<T>();
            // ������崴����ɺ���߼�
            if (callBack != null)
                callBack(panel);

            panel.ShowMe();

            //����������
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// �õ�ĳһ���Ѿ���ʾ����� �����ⲿʹ��
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }

}



