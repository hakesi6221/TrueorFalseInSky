using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class QuizDirections : SingletonMonoBehaviour<QuizDirections>
{
    protected override bool dontDestroyOnLoad => false;

    [SerializeField, Header("プレイヤーオブジェクト"), Foldout("Objects")]
    private Rigidbody2D _player;

    [SerializeField, Header("プレイヤースプライト"), Foldout("Objects")]
    private Animator _playerSprite;

    [SerializeField, Header("回答者"), Foldout("Objects")]
    private QuizRespondent _respondent;

    [SerializeField, Header("第～問テキスト"), Foldout("Objects")]
    private TextMeshProUGUI _numberText;

    [SerializeField, Header("問題文テキスト"), Foldout("Objects")]
    private TextMeshProUGUI _sentenceText;

    [SerializeField, Header("説明文テキスト"), Foldout("Objects")]
    private TextMeshProUGUI _explainText;

    [SerializeField, Header("終了テキスト"), Foldout("Objects")]
    private TextMeshProUGUI _allFinishText;

    [SerializeField, Header("回答中UI"), Foldout("Objects")]
    private RectTransform _solvingUI;

    [SerializeField, Header("制限時間表示UI"), Foldout("Objects")]
    private LimitTimeUI _timeLimitUI;

    [SerializeField, Header("〇UI"), Foldout("Objects")]
    private OptionsUI _circleUI;

    [SerializeField, Header("×UI"), Foldout("Objects")]
    private OptionsUI _crossUI;

    [SerializeField, Header("正解テキスト"), Foldout("Objects")]
    private TextMeshProUGUI _successText;

    [SerializeField, Header("不正解テキスト"), Foldout("Objects")]
    private TextMeshProUGUI _failedText;

    [SerializeField, Header("正解パーティクル"), Foldout("Objects")]
    private List<ParticleSystem> _successParticle = new List<ParticleSystem>();

    [SerializeField, Header("床"), Foldout("Objects")]
    private MoveFloor _floor;

    [SerializeField, Header("第～問テキスト表示所要時間：秒"), Foldout("StartQuiz")]
    private float _startQuizNumberSpeed = 1f;

    [SerializeField, Header("第～問テキスト表示終了からスライドアウトまでの時間：秒"), Foldout("StartQuiz")]
    private float _startQuizNumberSlideOutDelay = 1f;

    [SerializeField, Header("第～問スライドアウト表示所要時間：秒"), Foldout("StartQuiz")]
    private float _startQuizNumberSlideOutSpeed = 0.5f;

    [SerializeField, Header("第～問スライドアウト終了後のディレイ：秒"), Foldout("StartQuiz")]
    private float _startQuizStartDelay = 0.5f;

    [SerializeField, Header("問題文の表示速度：秒"), Foldout("DisplaySentence")]
    private float _displaySentenceSpeed = 0.05f;

    [SerializeField, Header("問題文表示後のディレイ：秒"), Foldout("DisplaySentence")]
    private float _displaySentenceAfterDelay = 0.5f;

    [SerializeField, Header("回答中のUIのスライドイン速度：秒"), Foldout("DisplaySentence")]
    private float _solvingUISlideInTime = 0.8f;

    [SerializeField, Header("回答時のUIスライドアウト時間：秒"), Foldout("Answered")]
    private float _answeredSlideOutTime = 0.8f;

    [SerializeField, Header("回答時のUIスライドアウト後のディレイ：秒"), Foldout("Answered")]
    private float _answeredAfterDelay = 0.8f;

    [SerializeField, Header("プレイヤージャンプ時の横に対する縦の相対的な強さ"), Foldout("FallPlayer")]
    private float _playerJumpForce = 3f;

    [SerializeField, Header("プレイヤー移動時の力の強さ"), Foldout("FallPlayer")]
    private float _playerAddForcePower = 6f;

    [SerializeField, Header("プレイヤー移動時のx最大移動量"), Foldout("FallPlayer")]
    private float _playerMoveLimitXOnAddForce = 5f;

    [SerializeField, Header("プレイヤー移動時のyの最高速度"), Foldout("FallPlayer")]
    private float _playerMoveMaxSpeedY = 10f;

    [SerializeField, Header("演出上での雲のスライドインの所要時間：秒"), Foldout("FallPlayer")]
    private float _cloudsInTimeSec = 1f;

    [SerializeField, Header("演出上での雲の待機の所要時間：秒"), Foldout("FallPlayer")]
    private float _cloudsWaitTimeSec = 1f;

    [SerializeField, Header("演出上での雲のスライドアウトの所要時間：秒"), Foldout("FallPlayer")]
    private float _cloudsOutTimeSec = 1f;

    [SerializeField, Header("落ちているときの演出パーティクル"), Foldout("FallPlayer")]
    private ParticleSystem _fallingParticle;

    [SerializeField, Header("成功演出時のテキスト再生までの時間：秒"), Foldout("SuccessDirection")]
    private float _successTextDelay = 0.4f;

    [SerializeField, Header("成功演出時のテキスト文"), Foldout("SuccessDirection")]
    private string _successSentence = "正解";

    [SerializeField, Header("成功演出時のテキスト文表示更新時間：秒"), Foldout("SuccessDirection")]
    private float _successDisplaySentenceTimeSec = 0.8f;

    [SerializeField, Header("成功演出時のテキスト文表示後パーティクル再生までのディレイ"), Foldout("SuccessDirection")]
    private float _successsParticleDelay = 0.8f;

    [SerializeField, Header("成功演出時のパーティクル再生後演出終了までのディレイ"), Foldout("SuccessDirection")]
    private float _successFinishDirectionDelay = 0.8f;

    [SerializeField, Header("不正解演出時のプレイヤーを飛ばす強さ"), Foldout("FailedDirection")]
    private float _failedPlayerAddForce = 10f;

    [SerializeField, Header("不正解演出時のプレイヤーを飛ばす方向"), Foldout("FailedDirection")]
    private Vector2 _failedPlayerAddForceDirection = new Vector2(1, 2);

    [SerializeField, Header("不正解演出時のプレイヤーの回転速度"), Foldout("FailedDirection")]
    private float _failedPlayerRotSpeed = 60f;

    [SerializeField, Header("不正解演出時のプレイヤーを飛ばしてからテキストが表示されるまでの時間：秒"), Foldout("FailedDirection")]
    private float _failedTextDelay = 0.5f;

    [SerializeField, Header("不正解演出時の表示されるテキスト"), Foldout("FailedDirection")]
    private string _failedSentence = "不正解";

    [SerializeField, Header("不正解演出時のテキスト文表示更新時間：秒"), Foldout("FailedDirection")]
    private float _failedDisplaySentenceTimeSec = 0.8f;

    [SerializeField, Header("不正解演出時のテキスト文表示終了後雲のカットインが入るまでのディレイ"), Foldout("FailedDirection")]
    private float _failedCloudDelay = 0.8f;

    [SerializeField, Header("不正解演出時の雲スライドイン時間：秒"), Foldout("FailedDirection")]
    private float _failedCloudInSec = 1f;

    [SerializeField, Header("不正解演出時の雲待機時間：秒"), Foldout("FailedDirection")]
    private float _failedCloudWaitSec = 1f;

    [SerializeField, Header("不正解演出時の雲スライドアウト時間：秒"), Foldout("FailedDirection")]
    private float _failedCloudOutSec = 1f;

    [SerializeField, Header("不正解演出時の雲スライドアウト後演出終了までのディレイ"), Foldout("FailedDirection")]
    private float _failedFinishDirectionDelay = 0.2f;

    [SerializeField, Header("解説テキストの表示速度：秒"), Foldout("Explain")]
    private float _explainDisplaySpeed = 0.1f;

    [SerializeField, Header("解説テキストのスライドアウト速度：秒"), Foldout("Explain")]
    private float _explainSlideOutSpeed = 0.8f;

    [SerializeField, Header("解説テキストのスライドアウト後のディレイ：秒"), Foldout("Explain")]
    private float _explainFinishDelay = 0.8f;

    [SerializeField, Header("すべてのクイズが終わった時に出すテキスト"), Foldout("AllFinish")]
    private string _allFinishSentence = "終了";

    [SerializeField, Header("すべてのクイズが終わった時に出すテキスト表示速度：秒"), Foldout("AllFinish")]
    private float _allFinishTextSpeed = 0.8f;

    [SerializeField, Header("すべてのクイズ終了テキスト表示終了後のディレイ"), Foldout("AllFinish")]
    private float _allFinishDelay = 0.8f;

    [SerializeField, Header("リザルト遷移時の雲スライドイン速度：秒"), Foldout("AllFinish")]
    private float _allFinishCloudInSpeed = 1f;

    [SerializeField, Header("リザルト遷移時の雲スライドアウト速度：秒"), Foldout("AllFinish")]
    private float _allFinishCloudOutSpeed = 1f;

    [SerializeField, Header("リザルトシーンの名前"), Foldout("AllFinish")]
    private string _resultSceneName;

    // プレイヤーの管理スクリプト
    private PlayerManager _playerManager;

    // プレイヤーの移動前のX座標
    private float _playerDefPlayerPosX = 0f;

    // 回答されたものが正解かどうか
    private bool _isLight = false;

    // プレイヤーが実際に移動中か
    private bool _playerIsMoving = false;

    /// <summary>
    /// クイズ出題前の演出
    /// </summary>
    /// <param name="number">出題する問題の番号</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask StartQuizDirection(int number, CancellationToken token)
    {
        string displayText = $"第{number + 1}問";
        _numberText.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(displayText, _numberText, _startQuizNumberSpeed, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_startQuizNumberSlideOutDelay), cancellationToken: token);

        await SlideUI.Instance.OnSlideUI(_numberText.gameObject.GetComponent<RectTransform>(), Vector2.zero, new Vector2(0, 1000f), _startQuizNumberSlideOutSpeed, token);
        _numberText.gameObject.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(_startQuizStartDelay), cancellationToken: token);

        Debug.Log("クイズ開始演出終了");
    }

    /// <summary>
    /// クイズ出題の演出
    /// </summary>
    /// <param name="sentence">表示する問題文</param>
    /// <param name="firstSelect">最初に選択している選択肢の番号</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask DisplayQuizSentence(string sentence, int firstSelect, CancellationToken token)
    {
        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(sentence, _sentenceText, _displaySentenceSpeed, token);
        await UniTask.Delay(TimeSpan.FromSeconds(_displaySentenceAfterDelay), cancellationToken: token);

        _solvingUI.gameObject.SetActive(true);
        _circleUI.Initialize();
        _crossUI.Initialize();
        _timeLimitUI.Initialize();
        await SlideUI.Instance.OnSlideUI(_solvingUI, new Vector2(0, 1000f), Vector2.zero, _solvingUISlideInTime, token);
        _timeLimitUI.StartCount();
        _respondent.Initialize();

        Debug.Log("クイズ出題演出終了");
    }

    /// <summary>
    /// クイズ回答時の演出
    /// 演出分岐もここで決定
    /// </summary>
    /// <param name="success"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async void DecisionOptionsDirection(bool success, CancellationToken token)
    {
        _isLight = success;
        await SlideUI.Instance.OnSlideUI(_solvingUI, Vector2.zero, new Vector2(0, 1000f), _answeredSlideOutTime, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_answeredAfterDelay), cancellationToken: token);
        PlayerAddForce();
    }


    /// <summary>
    /// プレイヤーを指定の方向へジャンプさせる
    /// </summary>
    /// <param name="direction">ジャンプさせる方向</param>
    public void PlayerAddForce()
    {
        _player.gravityScale = 1;
        _player.AddForce(_playerAddForcePower * new Vector2(_player.transform.right.x, _playerJumpForce).normalized, ForceMode2D.Impulse);
        _playerManager.PlayerFall();
        _playerIsMoving = true;
    }

    /// <summary>
    /// プレイヤーのX座標の限界値を適用
    /// </summary>
    /// <param name="positionX"></param>
    /// <returns></returns>
    private float PlayerMoveLimitX(float positionX)
    {
        return Mathf.Clamp(positionX, _playerDefPlayerPosX - _playerMoveLimitXOnAddForce, _playerDefPlayerPosX +  _playerMoveLimitXOnAddForce);
    }

    /// <summary>
    /// プレイヤーのY速度の限界値を適用
    /// </summary>
    /// <param name="velocityY"></param>
    /// <returns></returns>
    private float PlayerVelocityLimitY(float velocityY)
    {
        return Mathf.Clamp(velocityY, _playerMoveMaxSpeedY * -1, _playerMoveMaxSpeedY);
    }

    /// <summary>
    /// プレイヤーの移動を管理
    /// </summary>
    private void PlayerMoveManage()
    {
        if (!_playerIsMoving) return;
        _player.transform.position = new Vector3(PlayerMoveLimitX(_player.transform.position.x), _player.transform.position.y, _player.transform.position.z);
        _player.velocity = new Vector2(_player.velocity.x, PlayerVelocityLimitY(_player.velocity.y));
    }

    
    /// <summary>
    /// プレイヤーの速度、座標をリセット
    /// </summary>
    public void PlayerReset()
    {
        _player.velocity = Vector2.zero;
        _player.transform.position = Vector3.zero;
    }

    /// <summary>
    /// プレイヤーがカメラから外れて、移動中の場合、移動を終え、雲のカットを入れて落下中の演出を入れる
    /// それが終わり次第、床の移動の開始
    /// </summary>
    private async void PlayerResetManage()
    {
        if (!_playerIsMoving) return;
        if (_playerSprite.GetComponent<Renderer>().isVisible) return;

        _playerIsMoving = false;

        var token = this.GetCancellationTokenOnDestroy();

        await SlideCloudsManager.Instance.OnSlideInUI(_cloudsInTimeSec, token);

        _sentenceText.gameObject.SetActive(false);
        _player.gravityScale = 0f;
        PlayerReset();
        _fallingParticle.Play();
        _floor.SetPositionOnMoveStartPos(_isLight);
        await UniTask.Delay(TimeSpan.FromSeconds(_cloudsWaitTimeSec), cancellationToken: token);
        await SlideCloudsManager.Instance.OnSlideOutUI(_cloudsOutTimeSec, token);
        _floor.StartMove();
    }

    /// <summary>
    /// 成功時の演出を再生
    /// </summary>
    public async UniTask SuccessDirection(CancellationToken token)
    {
        _fallingParticle.Stop();
        _player.gravityScale = 1f;
        _successText.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(_successTextDelay), cancellationToken: token);

        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(_successSentence, _successText, _successDisplaySentenceTimeSec, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_successsParticleDelay), cancellationToken: token);
        
        foreach (ParticleSystem particle in _successParticle)
        {
            particle.Play();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(_successFinishDirectionDelay), cancellationToken: token);
        await SlideUI.Instance.OnSlideUI(_successText.GetComponent<RectTransform>(), Vector2.zero, new Vector2(0, 2000), 1f, token);
        _successText.gameObject.SetActive(false);

        Debug.Log("正解演出終了");
    }

    /// <summary>
    /// 不成功時の演出を再生
    /// </summary>
    public async UniTask FailedDirection(CancellationToken token)
    {
        _fallingParticle.Stop();
        _player.gravityScale = 1f;
        _player.AddForce(new Vector2(_failedPlayerAddForceDirection.x * _player.transform.right.x, _failedPlayerAddForceDirection.y).normalized * _failedPlayerAddForce, ForceMode2D.Impulse);
        _playerManager.PlayerRoll(_failedPlayerRotSpeed);

        await UniTask.Delay(TimeSpan.FromSeconds(_failedTextDelay), cancellationToken: token);

        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(_failedSentence, _failedText, _failedDisplaySentenceTimeSec, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_failedCloudDelay), cancellationToken: token);

        await SlideCloudsManager.Instance.OnSlideInUI(_failedCloudInSec, token);

        _floor.InactiveSpikes();
        _playerManager.PlayerFinishRoll();
        _player.transform.rotation = Quaternion.Euler(0, 0, 0);
        PlayerReset();
        _failedText.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(_failedCloudWaitSec), cancellationToken: token);

        await SlideCloudsManager.Instance.OnSlideOutUI(_failedCloudOutSec, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_failedFinishDirectionDelay), cancellationToken: token);

        Debug.Log("不正解演出終了");
    }

    /// <summary>
    /// 説明文を表示して、終わり次第次の問題への待機状態へ移行
    /// </summary>
    /// <param name="explain"></param>
    public async void DisplayExplain(string explain)
    {
        var token = this.GetCancellationTokenOnDestroy();

        _explainText.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(explain, _explainText, _explainDisplaySpeed, token);

        _respondent.Answerd();
    }

    /// <summary>
    /// 現在のクイズの回答を終了して、次のクイズの開始
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask FinishCurrentQuizDirection(CancellationToken token)
    {
        await SlideUI.Instance.OnSlideUI(_explainText.GetComponent<RectTransform>(), Vector2.zero, new Vector2(0, 1000f), _explainSlideOutSpeed, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_explainFinishDelay), cancellationToken: token);

    }

    /// <summary>
    /// すべての回答が終了し、結果のシーンへ移動
    /// </summary>
    public async void FinishAllQuiz()
    {
        var token = this.GetCancellationTokenOnDestroy();

        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(_allFinishSentence, _allFinishText, _allFinishTextSpeed, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_allFinishDelay), cancellationToken: token);

        SlideCloudsManager.Instance.OnCallSceneWithSlideUI(_resultSceneName, _allFinishCloudInSpeed, _allFinishCloudOutSpeed, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //_numberText.text = string.Empty;
        //_sentenceText.text = string.Empty;
        _playerManager = _player.GetComponent<PlayerManager>();
        _playerDefPlayerPosX = _player.transform.position.x;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerMoveManage();
        PlayerResetManage();
    }
}
