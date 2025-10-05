using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMoveWithConstAccel : MonoBehaviour
{
    [SerializeField, Header("UIでの配置の時の、距離の倍率")]
    private float _distanceMagnificationToUI = 100f;

    [SerializeField, Header("アニメーション方向")]
    private Vector2 _animDirection = Vector2.zero;

    [SerializeField, Header("アニメーションの移動距離")]
    private float _animMoveDistance = 0.5f;

    [SerializeField, Header("アニメーション1周期の時間：秒")]
    private float _animDuration = 1f;

    // アニメーションの加速度
    private float _animAcceleration = 1f;

    // 初速度
    private float _animFirstSpeed = 0f;

    // 現在の速度
    private float _animSpeed = 0f;

    // 指定されたアニメーション方向の単位ベクトル
    private Vector2 _animNormalDir = Vector2.zero;

    // 動きの速度
    private Vector3 _animVelocity = Vector2.zero;

    // RectTransform
    private RectTransform _rectTransform;

    /// <summary>
    /// 最高点と、最高点までの秒数で加速度を求める
    /// </summary>
    /// <param name="height">最高点</param>
    /// <param name="duration">最高点までの秒数</param>
    /// <returns></returns>
    public float CalcAcceleration(float height, float duration)
    {
        // 数値が不正の場合、-1で返す
        if (height <= 0 || duration <= 0)
        {
            Debug.Log("数値が不正です");
            return -1; 
        }

        // a = 8h / T^2　なので、

        float acceleration = (8 * height) / (duration * duration);

        return acceleration;
    }

    /// <summary>
    /// 状態の初期化
    /// </summary>
    public void Initialize()
    {
        if (_rectTransform == null)
        {
            transform.position = Vector2.zero;
        }
        else
        {
            _rectTransform.anchoredPosition = Vector2.zero;
        }
        _animSpeed = _animFirstSpeed;
    }


    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animNormalDir = _animDirection.normalized;
        _animAcceleration = CalcAcceleration(_animMoveDistance, _animDuration);
        _animFirstSpeed = _animAcceleration * (_animDuration / 2);
        _animSpeed = _animFirstSpeed;
        _animVelocity = _animNormalDir * _animSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rectTransform == null)
        {
            transform.position += _animVelocity * Time.deltaTime;
        }
        else
        {
            _rectTransform.anchoredPosition += (Vector2)(_animVelocity * Time.deltaTime) * _distanceMagnificationToUI;
        }
        _animVelocity = UpdateVelocity(_animFirstSpeed, _animAcceleration, _animNormalDir);
    }

    private Vector3 UpdateVelocity(float animFirstSpeed, float animAcceleration, Vector2 normalDir)
    {
        if (_animSpeed <= animFirstSpeed * -1)
        {
            if (_rectTransform == null)
            {
                transform.position = Vector2.zero;
            }
            else
            {
                _rectTransform.anchoredPosition = Vector2.zero;
            }
            _animSpeed = animFirstSpeed;
        }
        else
        {
            _animSpeed -= animAcceleration * Time.deltaTime;
        }

        return normalDir * _animSpeed;
    }
}
