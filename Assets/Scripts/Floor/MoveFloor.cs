using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [SerializeField, Header("�ǂ̕����ֈړ����čs����")]
    private Vector3 _moveDirection = Vector2.up;

    [SerializeField, Header("�ړ����v����")]
    private float _moveTimeSec = 5f;

    [SerializeField, Header("�ړ����x�F�b��")]
    private float _moveSpeedPerSec = 1f;

    [SerializeField, Header("�ړ����I�����銄��")]
    private float _moveFinishPer = 0.998f;

    [SerializeField, Header("�Ƃ��̃I�u�W�F�N�g")]
    private GameObject _spikes = null;

    // �����ʒu
    private Vector3 _firstPos = Vector3.zero;

    // ���v���ԂƑ��x�����ƂɌv�Z�����ړ��J�n�n�_
    private Vector3 _moveStartPos = Vector3.zero;

    // �ړ������ǂ���
    private bool _isMoving = false;

    /// <summary>
    /// �ړ��J�n�n�_�ɍ��W���ړ�
    /// </summary>
    /// <param name="success">�����������ǂ���</param>
    public void SetPositionOnMoveStartPos(bool success)
    {
        transform.position = _moveStartPos;
        _spikes.SetActive(!success);
    }

    /// <summary>
    /// �ړ����J�n
    /// </summary>
    public void StartMove()
    {
        _isMoving = true;
    }

    /// <summary>
    /// �Ƃ����\����
    /// </summary>
    public void InactiveSpikes()
    {
        if (_spikes == null) return;

        _spikes.SetActive(false);
    }

    private void Moving(Vector3 lastPosition)
    {
        if (!_isMoving) return;

        transform.position = lastPosition + (_moveDirection.normalized * _moveSpeedPerSec * Time.deltaTime);

        // �ړ��ʂ̊��������l�𒴂�����A�ړ����I�����ď����ʒu�ɖ߂�
        if ((Vector3.Distance(_moveStartPos, _firstPos) - Vector3.Distance(transform.position, _firstPos)) / Vector3.Distance(_moveStartPos, _firstPos) >= _moveFinishPer)
        {
            _isMoving = false;
            transform.position = _firstPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _isMoving = false;
        _firstPos = transform.position;
        _moveStartPos = _firstPos + ((_moveDirection.normalized * -1) * _moveTimeSec * _moveSpeedPerSec);
    }

    // Update is called once per frame
    void Update()
    {
        Moving(transform.position);
    }
}
