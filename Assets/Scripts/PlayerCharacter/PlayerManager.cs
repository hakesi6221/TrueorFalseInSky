using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField, Header("���̃^�O"), Tag]
    private string _floorTag = string.Empty;

    [SerializeField, Header("�Ƃ��̃^�O"), Tag]
    private string _spikeTag = string.Empty;

    [SerializeField, Header("�N�C�Y�i�s��")]
    private QuizFacilitator _quizFacilitator;

    // �v���C���[�����o�Ƃ��ė�������
    private bool _playerIsFalling = false;
    /// <summary>
    /// �v���C���[�����o�Ƃ��ė�������
    /// </summary>
    public bool PlayerIsFalling { get { return _playerIsFalling; } }
    /// <summary>
    /// �v���C���[���������J�n
    /// </summary>
    public void PlayerFall() { _playerIsFalling = true; }
    /// <summary>
    /// �v���C���[���������I��
    /// </summary>
    public void PlayerFinishFall() { _playerIsFalling = false; }

    // �v���C���[�����o�Ƃ��ĉ�]����
    private bool _playerRolling = false;

    // ��b�ɉ��x��]���邩
    private float _rotSpeed = 0f;

    /// <summary>
    /// �v���C���[����]���J�n
    /// </summary>
    public void PlayerRoll(float rotSpeed)
    {
        _rotSpeed = rotSpeed;
        _playerRolling = true;
    }
    /// <summary>
    /// �v���C���[����]���I��
    /// </summary>
    public void PlayerFinishRoll()
    {
        _playerRolling = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary>
    ///  �v���C���[����]
    /// </summary>
    private void PlayerRolling()
    {
        if (!_playerRolling) return;

        Debug.Log("��]��");
        transform.rotation *= Quaternion.AngleAxis(_rotSpeed * Time.deltaTime, new Vector3(0, 0, 1));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �������ɏ��̃I�u�W�F�N�g�ɓ��������ꍇ
        if (collision.gameObject.CompareTag(_floorTag) && _playerIsFalling)
        {
            // �������I�����āA�������o������
            PlayerFinishFall();

            // �������o
            Debug.Log("��������");
            _quizFacilitator.JudgeQuiz(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �������ɂƂ��ɂ����������ꍇ
        if (collision.gameObject.CompareTag(_spikeTag) && _playerIsFalling)
        {
            // �������I�����āA���s���o������
            PlayerFinishFall();

            // ���s���o
            Debug.Log("�ӂ�������");
            _quizFacilitator.JudgeQuiz(false);
        }
    }

    private void Update()
    {
        PlayerRolling();
    }
}
