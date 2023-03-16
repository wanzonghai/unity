using UnityEngine;
using System.Collections;

/// <summary>
/// 作为脚本单例的基类
/// </summary>
public class singtonBase<T> : MonoBehaviour where T : singtonBase<T>
{
 
     private static T instance;
     public static T Instance
     {
         get
         {
             if (null==instance) {
                 //如果这个单例脚本不存在,说明没有挂载
                 //但是要想使用,必须有对象挂载初始化
                 //所有我们自己创建一个空对象,挂载我们的单例脚本
                 //保证我们在使用的时候它已经被实例化
                 GameObject obj = new GameObject();
                 instance=obj.AddComponent<T>();
             }
             return instance;
      }
   }

  protected virtual void Awake()
   {
        instance = this as T;
    }

}