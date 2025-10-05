using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Properties;
using System;
using System.Threading;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField, Header("���Ȃ��̐������̓e�L�X�g")]
    private TextMeshProUGUI _currentAnswerTitle;

    [SerializeField, Header("�������e�L�X�g")]
    private TextMeshProUGUI _currentAnswerAmountText;

    [SerializeField, Header("�^�C�g���֖߂�e�L�X�g")]
    private TextMeshProUGUI _returnTitleText;

    [SerializeField, Header("���Ȃ��̐������̓e�L�X�g���o���܂ł̎��ԁF�b")]
    private float _currentAnswerTitleDelay = 1f;

    [SerializeField, Header("�������e�L�X�g���o���܂ł́A�O�i�K����̎��ԁF�b")]
    private float _currentAnswerAmountTextDelay = 1f;

    [SerializeField, Header("�������̕\�����x�F1�b�ŉ�%")]
    private float _currentAnswerAmountTextTimeSec = 30f;

    [SerializeField, Header("���ʂ��o���Ă���A�^�C�g���ւ̈ē����o��܂ł̎��ԁF�b")]
    private float _returnTitleDelay = 1f;

    [SerializeField, Header("�V�[���J�ڂ̉_�̃X���C�h�̎���")]
    private float _cloudsSlideTimeSec = 1f;

    [SerializeField, Header("�p�[�e�B�N�����o�郉�C��")]
    private float _particleLinePer = 80f;

    [SerializeField, Header("���߂łƂ��p�[�e�B�N��")]
    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    [SerializeField, Header("�^�C�g���V�[���̖��O")]
    private string _titleSceneName;

    private CancellationToken _destroyToken;

    private bool _isFinishResult = false;

    public void OnReturnTitle(InputAction.CallbackContext context)
    {
        if (!_isFinishResult) return;
        if (context.performed || context.canceled) return;

        _isFinishResult = false;

        SlideCloudsManager.Instance.OnCallSceneWithSlideUI(_titleSceneName, _cloudsSlideTimeSec, _cloudsSlideTimeSec, 0f);
    }

    // Start is called before the first frame update
    async void Start()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitUntil(() => !SlideCloudsManager.Instance.IsSlide, cancellationToken: _destroyToken);

        StartResult();
    }

    private async void StartResult()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_currentAnswerTitleDelay), cancellationToken: _destroyToken);
        _currentAnswerTitle.gameObject.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(_currentAnswerAmountTextDelay), cancellationToken: _destroyToken);
        await DisplayCurrentAnswerPer(_destroyToken);
        await UniTask.Delay(TimeSpan.FromSeconds(_returnTitleDelay), cancellationToken: _destroyToken);
        _returnTitleText.gameObject.SetActive(true);
        _isFinishResult = true;
    }

    private async UniTask DisplayCurrentAnswerPer(CancellationToken token)
    {
        float displayPer = 0f;
        _currentAnswerAmountText.text = string.Empty;
        _currentAnswerAmountText.gameObject.SetActive(true);

        while (displayPer <= GameInfomation.Instance.CurrentAnswerPer)
        {
            _currentAnswerAmountText.text = displayPer.ToString("f0");
            displayPer += Time.deltaTime * _currentAnswerAmountTextTimeSec;
            await UniTask.Yield(cancellationToken: token);
        }

        if (displayPer >= _particleLinePer)
        {
            foreach (ParticleSystem particle in _particles)
            {
                particle.Play();
            }
        }
    }
}
