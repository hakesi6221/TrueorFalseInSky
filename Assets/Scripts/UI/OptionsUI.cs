using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField, Header("���䂷��UI")]
    private RectTransform _rectTransform;

    [SerializeField, Header("�F����X�N���v�g")]
    private SelectableTextColor _selectedTextColor;

    [SerializeField, Header("�g�̑���")]
    private float _waveSpeed = 5f;

    [SerializeField, Header("�ړ����镝")]
    private float _maxDistance = 300f;

    [SerializeField, Header("�ړ��������")]
    private Vector2 _waveDrection = Vector2.up;

    // �����ʒu
    private Vector2 _firstPos = Vector2.zero;

    // �ړ����Ă��邩
    private bool _isMoving = false;

    // ���ݎ��Ԃ̕�����
    private float _timeFramePerSec = 0f;

    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        _isMoving = false;
        _timeFramePerSec = 0f;
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// �ړ���ԂƐF�̕ύX
    /// </summary>
    /// <param name="selected">�I������Ă��邩</param>
    public void ChangeMoveModeWithColor(bool selected)
    {
        _isMoving = selected;
        _selectedTextColor.ChangeColor(selected);
    }

    /// <summary>
    /// �ړ���Ԃ̕ύX
    /// </summary>
    /// <param name="selected">�I������Ă��邩</param>
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
