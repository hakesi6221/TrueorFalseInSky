using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;
using System;

public class SlideCloudsManager : SingletonMonoBehaviour<SlideCloudsManager>
{
    protected override bool dontDestroyOnLoad => true;

    [SerializeField, Header("移動させたいUI")]
    private RectTransform _clouds = null;

    // 動作中にデストロイされたらキャンセル処理
    private CancellationToken tokenOnDestroy;

    private void Start()
    {
        tokenOnDestroy = this.GetCancellationTokenOnDestroy();
    }

    // 移動中か
    private bool _isSlide = false;
    /// <summary>
    /// 移動中か
    /// </summary>
    public bool IsSlide {  get { return _isSlide; } }

    /// <summary>
    /// 雲のオブジェクトを、画面中央にスライドイン
    /// </summary>
    /// <param name="moveTime"></param>
    /// <param name="token">cancellationtoken</param>
    public async UniTask OnSlideInUI(float moveTime, CancellationToken token)
    {
        if (_isSlide)
        {
            await UniTask.WaitUntil(() => !_isSlide, cancellationToken: token);
        }

        _isSlide = true;
        _clouds.anchoredPosition = new Vector2(0, -2000f);
        await _clouds.DOAnchorPos(new Vector2(0, 0), moveTime)
            .SetEase(Ease.OutQuad)
            .ToUniTask(cancellationToken: token);
        Debug.Log("移動完了");
        _isSlide = false;
    }

    /// <summary>
    /// 雲のオブジェクトを、画面中央にスライドイン
    /// </summary>
    /// <param name="moveTime"></param>
    public async UniTask OnSlideInUI(float moveTime)
    {
        if (_isSlide)
        {
            await UniTask.WaitUntil(() => !_isSlide, cancellationToken: tokenOnDestroy);
        }

        _isSlide = true;
        _clouds.anchoredPosition = new Vector2(0, -2000f);
        await _clouds.DOAnchorPos(new Vector2(0, 0), moveTime)
            .SetEase(Ease.OutQuad)
            .ToUniTask(cancellationToken: tokenOnDestroy);
        Debug.Log("移動完了");
        _isSlide = false;
    }

    /// <summary>
    /// 雲のオブジェクトを、画面上部へスライドアウト
    /// </summary>
    /// <param name="moveTime"></param>
    /// <param name="token">cancellationtoken</param>
    public async UniTask OnSlideOutUI(float moveTime, CancellationToken token)
    {
        if (_isSlide)
        {
            await UniTask.WaitUntil(() => !_isSlide, cancellationToken: token);
        }

        _isSlide = true;
        _clouds.anchoredPosition = new Vector2(0, 0f);
        await _clouds.DOAnchorPos(new Vector2(0, 2000), moveTime)
            .SetEase(Ease.OutQuad)
            .ToUniTask(cancellationToken:  token);
        Debug.Log("移動完了");
        _isSlide = false;
    }

    /// <summary>
    /// 雲のオブジェクトを、画面上部へスライドアウト
    /// </summary>
    /// <param name="moveTime"></param>
    public async UniTask OnSlideOutUI(float moveTime)
    {
        if (_isSlide)
        {
            await UniTask.WaitUntil(() => !_isSlide, cancellationToken: tokenOnDestroy);
        }

        _isSlide = true;
        _clouds.anchoredPosition = new Vector2(0, 0f);
        await _clouds.DOAnchorPos(new Vector2(0, 2000), moveTime)
            .SetEase(Ease.OutQuad)
            .ToUniTask(cancellationToken: tokenOnDestroy);
        Debug.Log("移動完了");
        _isSlide = false;
    }

    public async void OnCallSceneWithSlideUI(string sceneName, float slideInSpeed, float slideOutSpeed, float waitTimeOnMoveScene)
    {
        await OnSlideInUI(slideInSpeed, tokenOnDestroy);
        SceneManager.LoadScene(sceneName);
        await UniTask.Delay(TimeSpan.FromSeconds(waitTimeOnMoveScene), cancellationToken: tokenOnDestroy);
        await OnSlideOutUI(slideOutSpeed, tokenOnDestroy);
    }
}
