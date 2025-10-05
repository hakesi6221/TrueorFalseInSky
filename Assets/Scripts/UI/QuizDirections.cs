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

    [SerializeField, Header("�v���C���[�I�u�W�F�N�g"), Foldout("Objects")]
    private Rigidbody2D _player;

    [SerializeField, Header("�v���C���[�X�v���C�g"), Foldout("Objects")]
    private Animator _playerSprite;

    [SerializeField, Header("�񓚎�"), Foldout("Objects")]
    private QuizRespondent _respondent;

    [SerializeField, Header("��`��e�L�X�g"), Foldout("Objects")]
    private TextMeshProUGUI _numberText;

    [SerializeField, Header("��蕶�e�L�X�g"), Foldout("Objects")]
    private TextMeshProUGUI _sentenceText;

    [SerializeField, Header("�������e�L�X�g"), Foldout("Objects")]
    private TextMeshProUGUI _explainText;

    [SerializeField, Header("�I���e�L�X�g"), Foldout("Objects")]
    private TextMeshProUGUI _allFinishText;

    [SerializeField, Header("�񓚒�UI"), Foldout("Objects")]
    private RectTransform _solvingUI;

    [SerializeField, Header("�������ԕ\��UI"), Foldout("Objects")]
    private LimitTimeUI _timeLimitUI;

    [SerializeField, Header("�ZUI"), Foldout("Objects")]
    private OptionsUI _circleUI;

    [SerializeField, Header("�~UI"), Foldout("Objects")]
    private OptionsUI _crossUI;

    [SerializeField, Header("�����e�L�X�g"), Foldout("Objects")]
    private TextMeshProUGUI _successText;

    [SerializeField, Header("�s�����e�L�X�g"), Foldout("Objects")]
    private TextMeshProUGUI _failedText;

    [SerializeField, Header("�����p�[�e�B�N��"), Foldout("Objects")]
    private List<ParticleSystem> _successParticle = new List<ParticleSystem>();

    [SerializeField, Header("��"), Foldout("Objects")]
    private MoveFloor _floor;

    [SerializeField, Header("��`��e�L�X�g�\�����v���ԁF�b"), Foldout("StartQuiz")]
    private float _startQuizNumberSpeed = 1f;

    [SerializeField, Header("��`��e�L�X�g�\���I������X���C�h�A�E�g�܂ł̎��ԁF�b"), Foldout("StartQuiz")]
    private float _startQuizNumberSlideOutDelay = 1f;

    [SerializeField, Header("��`��X���C�h�A�E�g�\�����v���ԁF�b"), Foldout("StartQuiz")]
    private float _startQuizNumberSlideOutSpeed = 0.5f;

    [SerializeField, Header("��`��X���C�h�A�E�g�I����̃f�B���C�F�b"), Foldout("StartQuiz")]
    private float _startQuizStartDelay = 0.5f;

    [SerializeField, Header("��蕶�̕\�����x�F�b"), Foldout("DisplaySentence")]
    private float _displaySentenceSpeed = 0.05f;

    [SerializeField, Header("��蕶�\����̃f�B���C�F�b"), Foldout("DisplaySentence")]
    private float _displaySentenceAfterDelay = 0.5f;

    [SerializeField, Header("�񓚒���UI�̃X���C�h�C�����x�F�b"), Foldout("DisplaySentence")]
    private float _solvingUISlideInTime = 0.8f;

    [SerializeField, Header("�񓚎���UI�X���C�h�A�E�g���ԁF�b"), Foldout("Answered")]
    private float _answeredSlideOutTime = 0.8f;

    [SerializeField, Header("�񓚎���UI�X���C�h�A�E�g��̃f�B���C�F�b"), Foldout("Answered")]
    private float _answeredAfterDelay = 0.8f;

    [SerializeField, Header("�v���C���[�W�����v���̉��ɑ΂���c�̑��ΓI�ȋ���"), Foldout("FallPlayer")]
    private float _playerJumpForce = 3f;

    [SerializeField, Header("�v���C���[�ړ����̗͂̋���"), Foldout("FallPlayer")]
    private float _playerAddForcePower = 6f;

    [SerializeField, Header("�v���C���[�ړ�����x�ő�ړ���"), Foldout("FallPlayer")]
    private float _playerMoveLimitXOnAddForce = 5f;

    [SerializeField, Header("�v���C���[�ړ�����y�̍ō����x"), Foldout("FallPlayer")]
    private float _playerMoveMaxSpeedY = 10f;

    [SerializeField, Header("���o��ł̉_�̃X���C�h�C���̏��v���ԁF�b"), Foldout("FallPlayer")]
    private float _cloudsInTimeSec = 1f;

    [SerializeField, Header("���o��ł̉_�̑ҋ@�̏��v���ԁF�b"), Foldout("FallPlayer")]
    private float _cloudsWaitTimeSec = 1f;

    [SerializeField, Header("���o��ł̉_�̃X���C�h�A�E�g�̏��v���ԁF�b"), Foldout("FallPlayer")]
    private float _cloudsOutTimeSec = 1f;

    [SerializeField, Header("�����Ă���Ƃ��̉��o�p�[�e�B�N��"), Foldout("FallPlayer")]
    private ParticleSystem _fallingParticle;

    [SerializeField, Header("�������o���̃e�L�X�g�Đ��܂ł̎��ԁF�b"), Foldout("SuccessDirection")]
    private float _successTextDelay = 0.4f;

    [SerializeField, Header("�������o���̃e�L�X�g��"), Foldout("SuccessDirection")]
    private string _successSentence = "����";

    [SerializeField, Header("�������o���̃e�L�X�g���\���X�V���ԁF�b"), Foldout("SuccessDirection")]
    private float _successDisplaySentenceTimeSec = 0.8f;

    [SerializeField, Header("�������o���̃e�L�X�g���\����p�[�e�B�N���Đ��܂ł̃f�B���C"), Foldout("SuccessDirection")]
    private float _successsParticleDelay = 0.8f;

    [SerializeField, Header("�������o���̃p�[�e�B�N���Đ��㉉�o�I���܂ł̃f�B���C"), Foldout("SuccessDirection")]
    private float _successFinishDirectionDelay = 0.8f;

    [SerializeField, Header("�s�������o���̃v���C���[���΂�����"), Foldout("FailedDirection")]
    private float _failedPlayerAddForce = 10f;

    [SerializeField, Header("�s�������o���̃v���C���[���΂�����"), Foldout("FailedDirection")]
    private Vector2 _failedPlayerAddForceDirection = new Vector2(1, 2);

    [SerializeField, Header("�s�������o���̃v���C���[�̉�]���x"), Foldout("FailedDirection")]
    private float _failedPlayerRotSpeed = 60f;

    [SerializeField, Header("�s�������o���̃v���C���[���΂��Ă���e�L�X�g���\�������܂ł̎��ԁF�b"), Foldout("FailedDirection")]
    private float _failedTextDelay = 0.5f;

    [SerializeField, Header("�s�������o���̕\�������e�L�X�g"), Foldout("FailedDirection")]
    private string _failedSentence = "�s����";

    [SerializeField, Header("�s�������o���̃e�L�X�g���\���X�V���ԁF�b"), Foldout("FailedDirection")]
    private float _failedDisplaySentenceTimeSec = 0.8f;

    [SerializeField, Header("�s�������o���̃e�L�X�g���\���I����_�̃J�b�g�C��������܂ł̃f�B���C"), Foldout("FailedDirection")]
    private float _failedCloudDelay = 0.8f;

    [SerializeField, Header("�s�������o���̉_�X���C�h�C�����ԁF�b"), Foldout("FailedDirection")]
    private float _failedCloudInSec = 1f;

    [SerializeField, Header("�s�������o���̉_�ҋ@���ԁF�b"), Foldout("FailedDirection")]
    private float _failedCloudWaitSec = 1f;

    [SerializeField, Header("�s�������o���̉_�X���C�h�A�E�g���ԁF�b"), Foldout("FailedDirection")]
    private float _failedCloudOutSec = 1f;

    [SerializeField, Header("�s�������o���̉_�X���C�h�A�E�g�㉉�o�I���܂ł̃f�B���C"), Foldout("FailedDirection")]
    private float _failedFinishDirectionDelay = 0.2f;

    [SerializeField, Header("����e�L�X�g�̕\�����x�F�b"), Foldout("Explain")]
    private float _explainDisplaySpeed = 0.1f;

    [SerializeField, Header("����e�L�X�g�̃X���C�h�A�E�g���x�F�b"), Foldout("Explain")]
    private float _explainSlideOutSpeed = 0.8f;

    [SerializeField, Header("����e�L�X�g�̃X���C�h�A�E�g��̃f�B���C�F�b"), Foldout("Explain")]
    private float _explainFinishDelay = 0.8f;

    [SerializeField, Header("���ׂẴN�C�Y���I��������ɏo���e�L�X�g"), Foldout("AllFinish")]
    private string _allFinishSentence = "�I��";

    [SerializeField, Header("���ׂẴN�C�Y���I��������ɏo���e�L�X�g�\�����x�F�b"), Foldout("AllFinish")]
    private float _allFinishTextSpeed = 0.8f;

    [SerializeField, Header("���ׂẴN�C�Y�I���e�L�X�g�\���I����̃f�B���C"), Foldout("AllFinish")]
    private float _allFinishDelay = 0.8f;

    [SerializeField, Header("���U���g�J�ڎ��̉_�X���C�h�C�����x�F�b"), Foldout("AllFinish")]
    private float _allFinishCloudInSpeed = 1f;

    [SerializeField, Header("���U���g�J�ڎ��̉_�X���C�h�A�E�g���x�F�b"), Foldout("AllFinish")]
    private float _allFinishCloudOutSpeed = 1f;

    [SerializeField, Header("���U���g�V�[���̖��O"), Foldout("AllFinish")]
    private string _resultSceneName;

    // �v���C���[�̊Ǘ��X�N���v�g
    private PlayerManager _playerManager;

    // �v���C���[�̈ړ��O��X���W
    private float _playerDefPlayerPosX = 0f;

    // �񓚂��ꂽ���̂��������ǂ���
    private bool _isLight = false;

    // �v���C���[�����ۂɈړ�����
    private bool _playerIsMoving = false;

    /// <summary>
    /// �N�C�Y�o��O�̉��o
    /// </summary>
    /// <param name="number">�o�肷����̔ԍ�</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask StartQuizDirection(int number, CancellationToken token)
    {
        string displayText = $"��{number + 1}��";
        _numberText.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        await DisplayTextOneChar.Instance.DisplayTextOneCharacter(displayText, _numberText, _startQuizNumberSpeed, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_startQuizNumberSlideOutDelay), cancellationToken: token);

        await SlideUI.Instance.OnSlideUI(_numberText.gameObject.GetComponent<RectTransform>(), Vector2.zero, new Vector2(0, 1000f), _startQuizNumberSlideOutSpeed, token);
        _numberText.gameObject.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(_startQuizStartDelay), cancellationToken: token);

        Debug.Log("�N�C�Y�J�n���o�I��");
    }

    /// <summary>
    /// �N�C�Y�o��̉��o
    /// </summary>
    /// <param name="sentence">�\�������蕶</param>
    /// <param name="firstSelect">�ŏ��ɑI�����Ă���I�����̔ԍ�</param>
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

        Debug.Log("�N�C�Y�o�艉�o�I��");
    }

    /// <summary>
    /// �N�C�Y�񓚎��̉��o
    /// ���o����������Ō���
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
    /// �v���C���[���w��̕����փW�����v������
    /// </summary>
    /// <param name="direction">�W�����v���������</param>
    public void PlayerAddForce()
    {
        _player.gravityScale = 1;
        _player.AddForce(_playerAddForcePower * new Vector2(_player.transform.right.x, _playerJumpForce).normalized, ForceMode2D.Impulse);
        _playerManager.PlayerFall();
        _playerIsMoving = true;
    }

    /// <summary>
    /// �v���C���[��X���W�̌��E�l��K�p
    /// </summary>
    /// <param name="positionX"></param>
    /// <returns></returns>
    private float PlayerMoveLimitX(float positionX)
    {
        return Mathf.Clamp(positionX, _playerDefPlayerPosX - _playerMoveLimitXOnAddForce, _playerDefPlayerPosX +  _playerMoveLimitXOnAddForce);
    }

    /// <summary>
    /// �v���C���[��Y���x�̌��E�l��K�p
    /// </summary>
    /// <param name="velocityY"></param>
    /// <returns></returns>
    private float PlayerVelocityLimitY(float velocityY)
    {
        return Mathf.Clamp(velocityY, _playerMoveMaxSpeedY * -1, _playerMoveMaxSpeedY);
    }

    /// <summary>
    /// �v���C���[�̈ړ����Ǘ�
    /// </summary>
    private void PlayerMoveManage()
    {
        if (!_playerIsMoving) return;
        _player.transform.position = new Vector3(PlayerMoveLimitX(_player.transform.position.x), _player.transform.position.y, _player.transform.position.z);
        _player.velocity = new Vector2(_player.velocity.x, PlayerVelocityLimitY(_player.velocity.y));
    }

    
    /// <summary>
    /// �v���C���[�̑��x�A���W�����Z�b�g
    /// </summary>
    public void PlayerReset()
    {
        _player.velocity = Vector2.zero;
        _player.transform.position = Vector3.zero;
    }

    /// <summary>
    /// �v���C���[���J��������O��āA�ړ����̏ꍇ�A�ړ����I���A�_�̃J�b�g�����ė������̉��o������
    /// ���ꂪ�I��莟��A���̈ړ��̊J�n
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
    /// �������̉��o���Đ�
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

        Debug.Log("�������o�I��");
    }

    /// <summary>
    /// �s�������̉��o���Đ�
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

        Debug.Log("�s�������o�I��");
    }

    /// <summary>
    /// ��������\�����āA�I��莟�掟�̖��ւ̑ҋ@��Ԃֈڍs
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
    /// ���݂̃N�C�Y�̉񓚂��I�����āA���̃N�C�Y�̊J�n
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async UniTask FinishCurrentQuizDirection(CancellationToken token)
    {
        await SlideUI.Instance.OnSlideUI(_explainText.GetComponent<RectTransform>(), Vector2.zero, new Vector2(0, 1000f), _explainSlideOutSpeed, token);

        await UniTask.Delay(TimeSpan.FromSeconds(_explainFinishDelay), cancellationToken: token);

    }

    /// <summary>
    /// ���ׂẲ񓚂��I�����A���ʂ̃V�[���ֈړ�
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
