using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.Input��
/// 2.�¼�����ģ��
/// 3.����Monoģ���ʹ��
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{

    private bool isStart = false;
    /// <summary>
    /// ���캯���� ���Updata����
    /// </summary>
    public InputMgr()
    {
        MonoMgr.Getinstance().AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// �Ƿ�����ر� �ҵ�������
    /// </summary>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    /// <summary>
    /// ������ⰴ��̧���� �ַ��¼���
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        //�¼�����ģ�� �ַ�����̧���¼�
        if (Input.GetKeyDown(key))
            EventCenter.Getinstance().EventTrigger("ĳ������", key);
        //�¼�����ģ�� �ַ�����̧���¼�
        if (Input.GetKeyUp(key))
            EventCenter.Getinstance().EventTrigger("ĳ��̧��", key);
    }

    private void MyUpdate()
    {
        //û�п��������� �Ͳ�ȥ��� ֱ��return
        if (!isStart)
            return;
        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.D);
    }

}
