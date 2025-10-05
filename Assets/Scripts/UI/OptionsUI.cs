using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField, Header("制御するUI")]
    private RectTransform _rectTransform;

    [SerializeField, Header("色制御スクリプト")]
    private SelectableTextColor _selectedTextColor;

    [SerializeField, Header("波の速さ")]
    private float _waveSpeed = 5f;

    [SerializeField, Header("移動する幅")]
    private float _maxDistance = 300f;

    [SerializeField, Header("移動する方向")]
    private Vector2 _waveDrection = Vector2.up;

    // 初期位置
    private Vector2 _firstPos = Vector2.zero;

    // 移動しているか
    private bool _isMoving = false;

    // 現在時間の物差し
    private float _timeFramePerSec = 0f;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _isMoving = false;
        _timeFramePerSec = 0f;
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// 移動状態と色の変更
    /// </summary>
    /// <param name="selected">選択されているか</param>
    public void ChangeMoveModeWithColor(bool selected)
    {
        _isMoving = selected;
        _selectedTextColor.ChangeColor(selected);
    }

    /// <summary>
    /// 移動状態の変更
    /// </summary>
    /// <param name="selected">選択されているか</param>
    public void ChangeMoveModeWithoutColor(bool selected)
    {
        _isMoving = selected;
    }

    // Start is called before the first frame update
    void Start()
    {
        _firstPos = _rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMoving) return;

        Vector2 performedPos = (Mathf.Sin(_timeFramePerSec * _waveSpeed) * _maxDistance) * _waveDrection;
        _rectTransform.anchoredPosition = _firstPos + performedPos;
        _timeFramePerSec += Time.deltaTime;
    }
}
