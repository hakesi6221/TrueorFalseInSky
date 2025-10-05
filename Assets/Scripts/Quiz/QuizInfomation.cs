using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateEnemyParamAsset")]
public class QuizInfomation : ScriptableObject
{
    [SerializeField, Header("–â‘è¯•Ê”Ô†")]
    private int _id = 0;
    /// <summary>
    /// –â‘è¯•Ê”Ô†
    /// </summary>
    public int ID {  get { return _id; } }

    [SerializeField, Header("–â‘è•¶"), TextArea(7, 10)]
    private string _sentence = "";
    /// <summary>
    /// –â‘è•¶
    /// </summary>
    public string Sentence { get { return _sentence; } }

    [SerializeField, Header("“š‚¦(0FZ, 1F~)")]
    private int _answer = 0;
    /// <summary>
    /// –â‘è‚Ì“š‚¦
    /// 0FZA1F~
    /// </summary>
    public int Answer { get { return _answer; } }

    [SerializeField, Header("‰ğà•¶"), TextArea(7, 10)]
    private string _explainSentence = "";
    /// <summary>
    /// ‰ğà•¶
    /// </summary>
    public string ExplainText { get { return _explainSentence; } }
}
