using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField, Header("床のタグ"), Tag]
    private string _floorTag = string.Empty;

    [SerializeField, Header("とげのタグ"), Tag]
    private string _spikeTag = string.Empty;

    [SerializeField, Header("クイズ進行役")]
    private QuizFacilitator _quizFacilitator;

    // プレイヤーが演出として落下中か
    private bool _playerIsFalling = false;
    /// <summary>
    /// プレイヤーが演出として落下中か
    /// </summary>
    public bool PlayerIsFalling { get { return _playerIsFalling; } }
    /// <summary>
    /// プレイヤーが落下を開始
    /// </summary>
    public void PlayerFall() { _playerIsFalling = true; }
    /// <summary>
    /// プレイヤーが落下を終了
    /// </summary>
    public void PlayerFinishFall() { _playerIsFalling = false; }

    // プレイヤーが演出として回転中か
    private bool _playerRolling = false;

    // 一秒に何度回転するか
    private float _rotSpeed = 0f;

    /// <summary>
    /// プレイヤーが回転を開始
    /// </summary>
    public void PlayerRoll(float rotSpeed)
    {
        _rotSpeed = rotSpeed;
        _playerRolling = true;
    }
    /// <summary>
    /// プレイヤーが回転を終了
    /// </summary>
    public void PlayerFinishRoll()
    {
        _playerRolling = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary>
    ///  プレイヤーが回転
    /// </summary>
    private void PlayerRolling()
    {
        if (!_playerRolling) return;

        Debug.Log("回転中");
        transform.rotation *= Quaternion.AngleAxis(_rotSpeed * Time.deltaTime, new Vector3(0, 0, 1));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 落下中に床のオブジェクトに当たった場合
        if (collision.gameObject.CompareTag(_floorTag) && _playerIsFalling)
        {
            // 落下を終了して、成功演出を入れる
            PlayerFinishFall();

            // 成功演出
            Debug.Log("せいかい");
            _quizFacilitator.JudgeQuiz(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 落下中にとげにあったった場合
        if (collision.gameObject.CompareTag(_spikeTag) && _playerIsFalling)
        {
            // 落下を終了して、失敗演出を入れる
            PlayerFinishFall();

            // 失敗演出
            Debug.Log("ふせいかい");
            _quizFacilitator.JudgeQuiz(false);
        }
    }

    private void Update()
    {
        PlayerRolling();
    }
}
