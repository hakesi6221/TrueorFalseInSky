using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitTimeUI : MonoBehaviour
{
    [SerializeField, Header("上からかぶせてるImage")]
    private Image _maskImage;

    [SerializeField, Header("回答者")]
    private QuizRespondent _quizRespondent;

    [SerializeField, Header("進行役")]
    private QuizFacilitator _facilitator;

    // 現在時間の物差し
    private float _timeFramePerSec = 0f;

    // カウント中か
    private bool _isCount = false;

    /// <summary>
    /// カウントを開始
    /// </summary>
    public void StartCount()
    {
        _maskImage.fillAmount = 0;
        _timeFramePerSec = 0f;
        _isCount = true;
    }

    public void Initialize()
    {
        _maskImage.fillAmount = 0;
        _timeFramePerSec = 0f;
    }

    private void OnCount()
    {
        if (!_isCount) return;

        _maskImage.fillAmount = _timeFramePerSec / _facilitator.QuizLimitTime;
        _timeFramePerSec += Time.deltaTime;

        if (_timeFramePerSec >= _facilitator.QuizLimitTime)
        {
            _isCount = false;
            _quizRespondent.DecisionOption();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnCount();
    }
}
