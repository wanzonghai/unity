using UnityEngine;
using System.Collections;

/// <summary>
/// ��Ϊ�ű������Ļ���
/// </summary>
public class singtonBase<T> : MonoBehaviour where T : singtonBase<T>
{
 
     private static T instance;
     public static T Instance
     {
         get
         {
             if (null==instance) {
                 //�����������ű�������,˵��û�й���
                 //����Ҫ��ʹ��,�����ж�����س�ʼ��
                 //���������Լ�����һ���ն���,�������ǵĵ����ű�
                 //��֤������ʹ�õ�ʱ�����Ѿ���ʵ����
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