using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [SerializeField, Header("どの方向へ移動して行くか")]
    private Vector3 _moveDirection = Vector2.up;

    [SerializeField, Header("移動所要時間")]
    private float _moveTimeSec = 5f;

    [SerializeField, Header("移動速度：秒速")]
    private float _moveSpeedPerSec = 1f;

    [SerializeField, Header("移動を終了する割合")]
    private float _moveFinishPer = 0.998f;

    [SerializeField, Header("とげのオブジェクト")]
    private GameObject _spikes = null;

    // 初期位置
    private Vector3 _firstPos = Vector3.zero;

    // 所要時間と速度をもとに計算した移動開始地点
    private Vector3 _moveStartPos = Vector3.zero;

    // 移動中かどうか
    private bool _isMoving = false;

    /// <summary>
    /// 移動開始地点に座標を移動
    /// </summary>
    /// <param name="success">正解したかどうか</param>
    public void SetPositionOnMoveStartPos(bool success)
    {
        transform.position = _moveStartPos;
        _spikes.SetActive(!success);
    }

    /// <summary>
    /// 移動を開始
    /// </summary>
    public void StartMove()
    {
        _isMoving = true;
    }

    /// <summary>
    /// とげを非表示に
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

        // 移動量の割合が一定値を超えたら、移動を終了して初期位置に戻す
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
