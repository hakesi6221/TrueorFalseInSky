using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SlideUI : SingletonMonoBehaviour<SlideUI>
{
    protected override bool dontDestroyOnLoad => true;
    // �ړ�����
    private bool _isSlide = false;

    /// <summary>
    /// �C�ӂ�UI�I�u�W�F�N�g���A�w��̍��W����w��̍��W�ɃX���C�h�C��
    /// </summary>
    /// <param name="moveTime"></param>
    /// <param name="token">cancellationtoken</param>
    public async UniTask OnSlideUI(RectTransform uiObj, Vector2 startPos, Vector2 endPos, float moveTime, CancellationToken token)
    {
        if (uiObj == null)
        {
            Debug.Log("�I�u�W�F�N�g���A�^�b�`����Ă��܂���");
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
        Debug.Log("�ړ�����");
        _isSlide = false;
    }
}
