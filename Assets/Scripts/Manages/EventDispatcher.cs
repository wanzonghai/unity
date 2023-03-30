using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void Act();
public delegate void Act<in T>(T t1);
public delegate void Act<in T1, in T2>(T1 t1, T2 t2);
public delegate void Act<in T1, in T2, in T3>(T1 t1, T2 t2, T3 t3);
public delegate void Act<in T1, in T2, in T3, in T4>(T1 t1, T2 t2, T3 t3, T4 t4);

public delegate T Fn<out T>();
public delegate T Fn<out T, in T1>(T1 t1);
public delegate T Fn<out T, in T1, in T2>(T1 t1, T2 t2);
public delegate T Fn<out T, in T1, in T2, in T3>(T1 t1, T2 t2, T3 t3);
public delegate T Fn<out T, in T1, in T2, in T3, in T4>(T1 t1, T2 t2, T3 t3, T4 t4);

public class EventDispatcher:BaseManager<EventDispatcher>
{
    /// <summary>
    /// 字典存放
    /// </summary>
    private Dictionary<int, Delegate> listeners = new Dictionary<int, Delegate>();
    public static EventDispatcher GameWorldEventDispatcher = new EventDispatcher();
   /* /// <summary>
    /// 单例模式
    /// </summary>
    public static EventDispatcher GameEvent
    {
        get
        {
            return GameWorldEventDispatcher;
        }
    }*/
    /// <summary>
    /// 构造函数
    /// </summary>
    public EventDispatcher()
    {
    }
    /// <summary>
    /// 移除字典中所有的键值（解绑所有事件）
    /// </summary>
    public void ClearEventListener()
    {
        listeners.Clear();
    }
    /// <summary>
    /// 添加事件注册
    /// </summary>
    /// <param name="evt">索引</param>
    /// <param name="listener"></param>
    private void AddDelegate(int evt, Delegate listener)
    {
        Delegate value;
        if (!listeners.TryGetValue(evt, out value))
        {
            //key不存在就添加
            listeners.Add(evt, listener);
        }
        else
        {
            //key存在判断value是否为空，为空就替换，不为空就多播
            value = (value != null) ? Delegate.Combine(value, listener) : listener;
            listeners[evt] = value;
        }
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="listener"></param>
    private void RemoveDelegate(int evt, Delegate listener)
    {
        Delegate func;
        if (listeners.TryGetValue(evt, out func))
        {
            if (func != null)
            {
                func = Delegate.Remove(func, listener);
                listeners[evt] = func;
            }
            else
            {
                Debug.LogError("Key" + evt + "不存在！");
            }
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="listener"></param>
    public void Regist(int evt, Act listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }
    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="listener"></param>
    public void UnRegist(int evt, Act listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="evt"></param>
    public void DispatchEvent(int evt)
    {
        Delegate func;

        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var act = (Act)func;
                act();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }
    }

    public void Regist<T>(int evt, Act<T> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegist<T>(int evt, Act<T> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public void DispatchEvent<T1>(int evt, T1 args)
    {
        Delegate func;

        if (listeners.TryGetValue(evt, out func) && func != null)
        {
            var act = (Act<T1>)func;
            act(args);
        }
    }

    public void Regist<T1, T2>(int evt, Act<T1, T2> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegist<T1, T2>(int evt, Act<T1, T2> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public void DispatchEvent<T1, T2>(int evt, T1 arg1, T2 arg2)
    {
        Delegate func;
        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var tmp = (Act<T1, T2>)func;
                tmp(arg1, arg2);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }
    }

    public void Regist<T1, T2, T3>(int evt, Act<T1, T2, T3> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegist<T1, T2, T3>(int evt, Act<T1, T2, T3> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public void DispatchEvent<T1, T2, T3>(int evt, T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate func;

        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var tmp = (Act<T1, T2, T3>)func;
                tmp(arg1, arg2, arg3);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }
    }

    public void Regist<T1, T2, T3, T4>(int evt, Act<T1, T2, T3, T4> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegist<T1, T2, T3, T4>(int evt, Act<T1, T2, T3, T4> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public void DispatchEvent<T1, T2, T3, T4>(int evt, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate func;

        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var tmp = (Act<T1, T2, T3, T4>)func;
                tmp(arg1, arg2, arg3, arg4);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }
    }

    public void RegistFn<T>(int evt, Fn<T> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegistFn<T>(int evt, Fn<T> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public T DispatchEventFn<T>(int evt)
    {
        Delegate func;
        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var fn = (Fn<T>)func;
                return fn();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }

        return default(T);
    }

    public void RegistFn<T, T1>(int evt, Fn<T, T1> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegistFn<T, T1>(int evt, Fn<T, T1> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public T DispatchEventFn<T, T1>(int evt, T1 args)
    {
        Delegate func;

        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var tmp = (Fn<T, T1>)func;
                return tmp(args);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }

        return default(T);
    }

    public void RegistFn<T, T1, T2>(int evt, Fn<T, T1, T2> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegistFn<T, T1, T2>(int evt, Fn<T, T1, T2> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public T DispatchEventFn<T, T1, T2>(int evt, T1 arg1, T2 arg2)
    {
        Delegate func;

        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var fn = (Fn<T, T1, T2>)func;
                return fn(arg1, arg2);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }

        return default(T);
    }

    public void RegistFn<T, T1, T2, T3>(int evt, Fn<T, T1, T2, T3> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegistFn<T, T1, T2, T3>(int evt, Fn<T, T1, T2, T3> listener)
    {
        if (listener == null)
        {
            return;
        }

        RemoveDelegate(evt, listener);
    }

    public T DispatchEventFn<T, T1, T2, T3>(int evt, T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate func;

        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var fn = (Fn<T, T1, T2, T3>)func;
                return fn(arg1, arg2, arg3);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);
        }

        return default(T);
    }

    public void RegistFn<T, T1, T2, T3, T4>(int evt, Fn<T, T1, T2, T3, T4> listener)
    {
        if (listener == null)
        {
            return;
        }

        AddDelegate(evt, listener);
    }

    public void UnRegistFn<T, T1, T2, T3, T4>(int evt, Fn<T, T1, T2, T3, T4> listener)
    {
        if (listener == null)
        {
            return;
        }
        RemoveDelegate(evt, listener);
    }

    public T DispatchEventFn<T, T1, T2, T3, T4>(int evt, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate func;
        try
        {
            if (listeners.TryGetValue(evt, out func) && func != null)
            {
                var fn = (Fn<T, T1, T2, T3, T4>)func;
                return fn(arg1, arg2, arg3, arg4);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            UnityEngine.Debug.LogError(e.StackTrace);

        }

        return default(T);
    }


}


