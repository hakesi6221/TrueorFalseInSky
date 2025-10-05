using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SlideUI : SingletonMonoBehaviour<SlideUI>
{
    protected override bool dontDestroyOnLoad => true;
    // 移動中か
    private bool _isSlide = false;

    /// <summary>
    /// 任意のUIオブジェクトを、指定の座標から指定の座標にスライドイン
    /// </summary>
    /// <param name="moveTime"></param>
    /// <param name="token">cancellationtoken</param>
    public async UniTask OnSlideUI(RectTransform uiObj, Vector2 startPos, Vector2 endPos, float moveTime, CancellationToken token)
    {
        if (uiObj == null)
        {
            Debug.Log("オブジェクトがアタッチされていません");
            return;
        }

        if (_isSlide)
        {
            await UniTask.WaitUntil(() => !_isSlide, cancellationToken: token);
        }

        _isSlide = true;
        uiObj.anchoredPosition = startPos;
        await uiObj.DOAnchorPos(endPos, moveTime)
            .SetEase(Ease.OutQuad)
            .ToUniTask(cancellationToken: token);
        Debug.Log("移動完了");
        _isSlide = false;
    }
}
