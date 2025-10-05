using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using TMPro;
using System;
using System.Threading;

public class DisplayTextOneChar : SingletonMonoBehaviour<DisplayTextOneChar>
{
    protected override bool dontDestroyOnLoad => false;

    /// <summary>
    /// �ꕶ�����e�L�X�g��\������
    /// </summary>
    /// <param name="centence">�\����������</param>
    /// <param name="tmp">�\������Ώۂ�TextMeshPro</param>
    /// <param name="waitTime">������\������ҋ@���ԁF�b</param>
    /// <returns></returns>
    public async UniTask DisplayTextOneCharacter(string centence, TextMeshProUGUI tmp, float waitTime, CancellationToken token)
    {

        // �e�L�X�g�̕\����������0��
        tmp.maxVisibleCharacters = 0;
        // �e�L�X�g�̓��e��^����ꂽ���͂Ɍ���
        tmp.text = centence;
        tmp.gameObject.SetActive(true);

        while  (centence.Length > tmp.maxVisibleCharacters)
        {
            // �\���e�L�X�g�𑝂₷
            tmp.maxVisibleCharacters++;

            // �w��b�҂���
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("�L�����Z��");
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
