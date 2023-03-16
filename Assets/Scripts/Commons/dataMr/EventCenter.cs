using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
/// <summary>
/// �¼����� ����ģʽ����
/// 1.Dictionary
/// 2.ί��
/// 3.�۲������ģʽ
/// 4.����
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //key ���� �¼������֣����磺�������������������ͨ�� �ȵȣ�
    //value ���� ��Ӧ���� ��������¼� ��Ӧ��ί�к�����
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// ����¼�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">׼�����������¼� ��ί�к���</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //û�е����
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// ��������Ҫ�������ݵ��¼�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        //û�е����
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }


    /// <summary>
    /// �Ƴ���Ӧ���¼�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions -= action;
    }

    /// <summary>
    /// �Ƴ�����Ҫ�������¼�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions -= action;
    }

    /// <summary>
    /// �¼�����
    /// </summary>
    /// <param name="name">��һ�����ֵ��¼�������</param>
    public void EventTrigger<T>(string name, T info)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            //eventDic[name].Invoke(info);
        }
    }
    /// <summary>
    /// �¼�����������Ҫ�����ģ�
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name)
    {
        //��û�ж�Ӧ���¼�����
        //�е����
        if (eventDic.ContainsKey(name))
        {
            //eventDic[name]();
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
            //eventDic[name].Invoke(info);
        }
    }
    /// <summary>
    /// ����¼�����
    /// ��Ҫ���� �����л�ʱ
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
