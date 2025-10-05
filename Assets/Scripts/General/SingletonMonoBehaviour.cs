using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// ���̃I�u�W�F�N�g��DontDestroyOnRoad�ɂ��邩�ǂ���
    /// </summary>
    protected abstract bool dontDestroyOnLoad { get; }

    private static T instance;

    /// <summary>
    /// �C���X�^���X���擾
    /// �C���X�^���X���Ȃ��ꍇ�G���[���o��
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (!instance)
                {
                    Debug.LogError(t + " is nothing.");
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// ���łɃC���X�^���X������ꍇ�A������폜�AdontDestroyOnLoad��true�̏ꍇ�ݒ�
    /// </summary>
    protected virtual void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
