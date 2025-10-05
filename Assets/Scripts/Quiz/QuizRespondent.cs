using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuizRespondent : MonoBehaviour
{
    [SerializeField, Header("�v���C���[")]
    private Transform _player;

    [SerializeField, Header("�����I�����F0���Z, 1���~")]
    private int _firstSelectIndex = 0;

    [SerializeField, Header("�ZUI")]
    private OptionsUI _circleUI;

    [SerializeField, Header("�~UI")]
    private OptionsUI _crossUI;

    [SerializeField, Header("���֍s���ۂ̃|�C���^�[")]
    private PointerMoveWithConstAccel _gonextPointer;

    [SerializeField, Header("�I�����Z�̔ԍ��F0, 1"), Range(0, 1)]
    private int _circleIndex = 0;

    [SerializeField, Header("�I�����~�̔ԍ��F0, 1"), Range(0, 1)]
    private int _crossIndex = 1;

    [SerializeField, Header("�N�C�Y�i�s��")]
    private QuizFacilitator _quizFacilitator;

    // ���ݑI�𒆂̑I����
    private int _currentOption = 0;
    /// <summary>
    /// ���ݑI�𒆂̑I����
    /// </summary>
    public int CurrentOption { get {  return _currentOption; } }

    // �񓚒���
    private bool _isSelecting = false;

    // ����s�������o����
    private bool _isAnswerd = false;

    /// <summary>
    /// �I����Ԃ̏������A�I��������J�n
    /// </summary>
    public void Initialize()
    {
        _isAnswerd = false;
        _currentOption = _firstSelectIndex;
        _circleUI.ChangeMoveModeWithColor(_circleIndex == _currentOption);
        _crossUI.ChangeMoveModeWithColor (_crossIndex == _currentOption);
        if (_currentOption == 0)
        {
            _player.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _player.rotation = Quaternion.Euler(0, 180f, 0);
        }
            _isSelecting = true;
    }

    /// <summary>
    /// �񓚐���s�������o���I��
    /// </summary>
    public void Answerd()
    {
        _gonextPointer.gameObject.SetActive(true);
        //_gonextPointer.Initialize();
        _isAnswerd = true;
    }

    /// <summary>
    /// �I�����̌���
    /// </summary>
    public void DecisionOption()
    {
        _isSelecting = false;
        _circleUI.ChangeMoveModeWithoutColor(false);
        _crossUI.ChangeMoveModeWithoutColor(false);
        _quizFacilitator.AnswerQuiz(_currentOption);
    }

    public void OnSelectOptions(InputAction.CallbackContext context)
    {
        if (!_isSelecting) return;

        if (context.performed || context.canceled) return;

        // ���݂̃|�C���^�[�f�o�C�X���擾
        Pointer pointer = Pointer.current;
        if (pointer == null) return;


        // �^�b�v�y�уN���b�N���ꂽ���W���擾
        Vector3 tapPos = pointer.position.ReadValue();
        Debug.Log($"Press Position:{tapPos}");

        if (tapPos.x < 1920f / 2f && _currentOption == 0)
        {
            _currentOption++;
            _circleUI.ChangeMoveModeWithColor(_circleIndex == _currentOption);
            _crossUI.ChangeMoveModeWithColor(_crossIndex == _currentOption);
            _player.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (tapPos.x > 1920f / 2f && _currentOption == 1)
        {
            _currentOption--;
            _circleUI.ChangeMoveModeWithColor(_circleIndex == _currentOption);
            _crossUI.ChangeMoveModeWithColor(_crossIndex == _currentOption);
            _player.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void OnGoNext(InputAction.CallbackContext context)
    {
        if (!_isAnswerd) return;

        if (context.performed || context.canceled) return;

        _isAnswerd = false;

        _gonextPointer.gameObject.SetActive(false);
        _quizFacilitator.FinishCurrentQuiz();
    }

}
