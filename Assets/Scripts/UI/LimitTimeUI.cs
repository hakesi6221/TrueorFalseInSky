using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimitTimeUI : MonoBehaviour
{
    [SerializeField, Header("�ォ�炩�Ԃ��Ă�Image")]
    private Image _maskImage;

    [SerializeField, Header("�񓚎�")]
    private QuizRespondent _quizRespondent;

    [SerializeField, Header("�i�s��")]
    private QuizFacilitator _facilitator;

    // ���ݎ��Ԃ̕�����
    private float _timeFramePerSec = 0f;

    // �J�E���g����
    private bool _isCount = false;

    /// <summary>
    /// �J�E���g���J�n
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
