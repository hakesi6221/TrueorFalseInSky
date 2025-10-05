using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMoveWithConstAccel : MonoBehaviour
{
    [SerializeField, Header("UI�ł̔z�u�̎��́A�����̔{��")]
    private float _distanceMagnificationToUI = 100f;

    [SerializeField, Header("�A�j���[�V��������")]
    private Vector2 _animDirection = Vector2.zero;

    [SerializeField, Header("�A�j���[�V�����̈ړ�����")]
    private float _animMoveDistance = 0.5f;

    [SerializeField, Header("�A�j���[�V����1�����̎��ԁF�b")]
    private float _animDuration = 1f;

    // �A�j���[�V�����̉����x
    private float _animAcceleration = 1f;

    // �����x
    private float _animFirstSpeed = 0f;

    // ���݂̑��x
    private float _animSpeed = 0f;

    // �w�肳�ꂽ�A�j���[�V���������̒P�ʃx�N�g��
    private Vector2 _animNormalDir = Vector2.zero;

    // �����̑��x
    private Vector3 _animVelocity = Vector2.zero;

    // RectTransform
    private RectTransform _rectTransform;

    /// <summary>
    /// �ō��_�ƁA�ō��_�܂ł̕b���ŉ����x�����߂�
    /// </summary>
    /// <param name="height">�ō��_</param>
    /// <param name="duration">�ō��_�܂ł̕b��</param>
    /// <returns></returns>
    public float CalcAcceleration(float height, float duration)
    {
        // ���l���s���̏ꍇ�A-1�ŕԂ�
        if (height <= 0 || duration <= 0)
        {
            Debug.Log("���l���s���ł�");
            return -1; 
        }

        // a = 8h / T^2�@�Ȃ̂ŁA

        float acceleration = (8 * height) / (duration * duration);

        return acceleration;
    }

    /// <summary>
    /// ��Ԃ̏�����
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
