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
    /// 一文字ずつテキストを表示する
    /// </summary>
    /// <param name="centence">表示したい文</param>
    /// <param name="tmp">表示する対象のTextMeshPro</param>
    /// <param name="waitTime">文字を表示する待機時間：秒</param>
    /// <returns></returns>
    public async UniTask DisplayTextOneCharacter(string centence, TextMeshProUGUI tmp, float waitTime, CancellationToken token)
    {

        // テキストの表示文字数を0に
        tmp.maxVisibleCharacters = 0;
        // テキストの内容を与えられた文章に決定
        tmp.text = centence;
        tmp.gameObject.SetActive(true);

        while  (centence.Length > tmp.maxVisibleCharacters)
        {
            // 表示テキストを増やす
            tmp.maxVisibleCharacters++;

            // 指定秒待って
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("キャンセル");
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
