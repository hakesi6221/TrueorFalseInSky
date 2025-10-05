using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using System.Threading;



public class QuizFacilitator : MonoBehaviour
{
    [SerializeField, Header("���o�萔")]
    private int _quizAmount = 10;

    [SerializeField, Header("���̐�������")]
    private float _quizLimitTime = 15f;

    [SerializeField, Header("�N�C�Y�̏��i�[���X�g")]
    private List<QuizInfomation> _quizList = new List<QuizInfomation>();

    [SerializeField, Header("�����I�����F0���Z, 1���~")]
    private int _firstSelectIndex = 0;

    // ���݂̖�萔
    private int _quizIndex = 0;

    // ������
    private int _currentAnswerAmount = 0;

    // ���ݍU�����̃N�C�Y
    private QuizInfomation _currentQuiz = null;

    // �����܂ł̍U�������N�C�Y
    private List<QuizInfomation> _solvedQuizes = new List<QuizInfomation>();

    // �N�C�Y���U������
    private bool _isQuiz = false;

    private CancellationToken _destroyToken;

    /// <summary>
    /// ���̐�������
    /// </summary>
    public float QuizLimitTime { get { return _quizLimitTime; } }

    /// <summary>
    ///  �U������N�C�Y������
    /// </summary>
    /// <returns></returns>
    private QuizInfomation DecisionQuiz()
    {
        QuizInfomation quiz = null;

        while (quiz == null || _solvedQuizes.Contains(quiz))
        {
            quiz = _quizList[Random.Range(0, _quizList.Count)];
        }

        return quiz;
    }

    private async void OnStartQuiz()
    {
        // ���łɃN�C�Y�U�����Ȃ�L�����Z��
        if (_isQuiz) return;
        _isQuiz = true;
        if (_quizIndex >= _quizAmount)
        {
            GameInfomation.Instance.SetCurrentAnswerPer(((float)_currentAnswerAmount / (float)_quizAmount) * 100f);
            QuizDirections.Instance.FinishAllQuiz();
            return; 
        }

        // �U������N�C�Y������
        _currentQuiz = DecisionQuiz();
        _solvedQuizes.Add(_currentQuiz);

        await QuizDirections.Instance.StartQuizDirection(_quizIndex++, _destroyToken);

        await QuizDirections.Instance.DisplayQuizSentence(_currentQuiz.Sentence, _firstSelectIndex, _destroyToken);

    }

    /// <summary>
    /// �N�C�Y�ɉ�
    /// </summary>
    /// <param name="index"></param>
    public void AnswerQuiz(int index)
    {
        QuizDirections.Instance.DecisionOptionsDirection(_currentQuiz.Answer == index, _destroyToken);
    }

    /// <summary>
    /// �N�C�Y�̔�����s���A���o���I��莟������\��
    /// </summary>
    /// <param name="success"></param>
    public async void JudgeQuiz(bool success)
    {
        if (success)
        {
            _currentAnswerAmount++;
            await QuizDirections.Instance.SuccessDirection(_destroyToken);
        }
        else
        {
            await QuizDirections.Instance.FailedDirection(_destroyToken);
        }

        QuizDirections.Instance.DisplayExplain(_currentQuiz.ExplainText);
    }

    /// <summary>
    /// ���݂̃N�C�Y���I��
    /// </summary>
    public async void FinishCurrentQuiz()
    {
        await QuizDirections.Instance.FinishCurrentQuizDirection(_destroyToken);

        _isQuiz = false;
    }

    // Start is called before the first frame update
    async void Start()
    {
        _destroyToken = this.GetCancellationTokenOnDestroy();

        await UniTask.WaitUntil(() => !SlideCloudsManager.Instance.IsSlide, cancellationToken: _destroyToken);
        OnStartQuiz();
    }

    // Update is called once per frame
    void Update()
    {
        if (_quizIndex == 0) return;
        OnStartQuiz();
    }
}
