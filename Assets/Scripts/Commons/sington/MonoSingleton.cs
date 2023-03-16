
using UnityEngine;


/*
Editor:
Version:
The last time for modification:
Time for Creation:
*/
namespace Demo_XXManager
{
    /// <summary>
    /// ����Ϊ�ű������ࡣ���е������ű���Ҫ����XXManager����Ҫʹ��XXManager�̳��ڸ����Instance����
    /// ���м̳д����Manager�඼��Ҫ�ڼ̳�ʱ�����Լ�������
    /// ���������������ڳ�����Ψһ�ҳ��õ���
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        //����ʵ���ֶΣ�˽���ֶ�
        private static T instance;
        //����ʵ�����ԣ����з���Ҫ����XXManager��Ҫͨ��XXManager�Ĵ˷��������ʣ�ͬʱΪ�˷�ֹ�нű���Awake�е��ô�Manager�࣬���ǲ��ð�����صķ�ʽ�����С�
        //���нű������˴����ԣ����ǲŻ�ȥ��ʼ�������ԡ�
        public static T Instance
        {
            get
            {
                //�����ǰû��ʵ������
                if (instance == null)
                {
                    //�ڳ�����Ѱ��ʵ������
                    instance = FindObjectOfType<T>();
                    //���������û��Manager��ʵ��
                    if (instance == null)
                    {
                        //�½�һ����Ϸ���󲢸�����XXManager������ű���ʱ�����XXManager��Awake������Ĭ�ϵ����˸����Awake��
                        new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                    }
                }
                else
                {
                    //�����Զ����ʼ������
                    instance.Init();
                }
                //�����ʵ��������ֱ�ӷ��ء�
                return instance;
            }
        }
        //Ϊ�˷�ֹ�е���ϲ��ֱ�ӽ��ű����أ�����Ҳ�ṩ��Awake���������г�ʼ��Instance���ԡ�
        protected void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                Init();
            }
        }
        /// <summary>
        /// �Զ����ʼ������,����Awake�������ڳ�ʼ���ˣ�����������һ���Զ��庯��������ʼ������
        /// </summary>
        public virtual void Init()
        {

        }
        /// <summary>
        /// ���Է���
        /// </summary>
        public virtual void Add()
        {
            Debug.Log("�ɹ�");
        }
    }
}
