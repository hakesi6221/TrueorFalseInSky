using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateEnemyParamAsset")]
public class QuizInfomation : ScriptableObject
{
    [SerializeField, Header("��莯�ʔԍ�")]
    private int _id = 0;
    /// <summary>
    /// ��莯�ʔԍ�
    /// </summary>
    public int ID {  get { return _id; } }

    [SerializeField, Header("��蕶"), TextArea(7, 10)]
    private string _sentence = "";
    /// <summary>
    /// ��蕶
    /// </summary>
    public string Sentence { get { return _sentence; } }

    [SerializeField, Header("����(0�F�Z, 1�F�~)")]
    private int _answer = 0;
    /// <summary>
    /// ���̓���
    /// 0�F�Z�A1�F�~
    /// </summary>
    public int Answer { get { return _answer; } }

    [SerializeField, Header("�����"), TextArea(7, 10)]
    private string _explainSentence = "";
    /// <summary>
    /// �����
    /// </summary>
    public string ExplainText { get { return _explainSentence; } }
}
