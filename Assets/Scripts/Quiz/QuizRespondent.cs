using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuizRespondent : MonoBehaviour
{
    [SerializeField, Header("プレイヤー")]
    private Transform _player;

    [SerializeField, Header("初期選択肢：0→〇, 1→×")]
    private int _firstSelectIndex = 0;

    [SerializeField, Header("〇UI")]
    private OptionsUI _circleUI;

    [SerializeField, Header("×UI")]
    private OptionsUI _crossUI;

    [SerializeField, Header("次へ行く際のポインター")]
    private PointerMoveWithConstAccel _gonextPointer;

    [SerializeField, Header("選択肢〇の番号：0, 1"), Range(0, 1)]
    private int _circleIndex = 0;

    [SerializeField, Header("選択肢×の番号：0, 1"), Range(0, 1)]
    private int _crossIndex = 1;

    [SerializeField, Header("クイズ進行者")]
    private QuizFacilitator _quizFacilitator;

    // 現在選択中の選択肢
    private int _currentOption = 0;
    /// <summary>
    /// 現在選択中の選択肢
    /// </summary>
    public int CurrentOption { get {  return _currentOption; } }

    // 回答中か
    private bool _isSelecting = false;

    // 正解不正解が出たか
    private bool _isAnswerd = false;

    /// <summary>
    /// 選択状態の初期化、選択肢操作開始
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
    /// 回答正解不正解演出が終了
    /// </summary>
    public void Answerd()
    {
        _gonextPointer.gameObject.SetActive(true);
        //_gonextPointer.Initialize();
        _isAnswerd = true;
    }

    /// <summary>
    /// 選択肢の決定
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

        // 現在のポインターデバイスを取得
        Pointer pointer = Pointer.current;
        if (pointer == null) return;


        // タップ及びクリックされた座標を取得
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
