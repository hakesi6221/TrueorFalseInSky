using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using System.Threading;



public class QuizFacilitator : MonoBehaviour
{
    [SerializeField, Header("問題出題数")]
    private int _quizAmount = 10;

    [SerializeField, Header("問題の制限時間")]
    private float _quizLimitTime = 15f;

    [SerializeField, Header("クイズの情報格納リスト")]
    private List<QuizInfomation> _quizList = new List<QuizInfomation>();

    [SerializeField, Header("初期選択肢：0→〇, 1→×")]
    private int _firstSelectIndex = 0;

    // 現在の問題数
    private int _quizIndex = 0;

    // 正答数
    private int _currentAnswerAmount = 0;

    // 現在攻略中のクイズ
    private QuizInfomation _currentQuiz = null;

    // ここまでの攻略したクイズ
    private List<QuizInfomation> _solvedQuizes = new List<QuizInfomation>();

    // クイズを攻略中か
    private bool _isQuiz = false;

    private CancellationToken _destroyToken;

    /// <summary>
    /// 問題の制限時間
    /// </summary>
    public float QuizLimitTime { get { return _quizLimitTime; } }

    /// <summary>
    ///  攻略するクイズを決定
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
        // すでにクイズ攻略中ならキャンセル
        if (_isQuiz) return;
        _isQuiz = true;
        if (_quizIndex >= _quizAmount)
        {
            GameInfomation.Instance.SetCurrentAnswerPer(((float)_currentAnswerAmount / (float)_quizAmount) * 100f);
            QuizDirections.Instance.FinishAllQuiz();
            return; 
        }

        // 攻略するクイズを決定
        _currentQuiz = DecisionQuiz();
        _solvedQuizes.Add(_currentQuiz);

        await QuizDirections.Instance.StartQuizDirection(_quizIndex++, _destroyToken);

        await QuizDirections.Instance.DisplayQuizSentence(_currentQuiz.Sentence, _firstSelectIndex, _destroyToken);

    }

    /// <summary>
    /// クイズに回答
    /// </summary>
    /// <param name="index"></param>
    public void AnswerQuiz(int index)
    {
        QuizDirections.Instance.DecisionOptionsDirection(_currentQuiz.Answer == index, _destroyToken);
    }

    /// <summary>
    /// クイズの判定を行い、演出が終わり次第解説を表示
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
    /// 現在のクイズが終了
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
