using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfomation : SingletonMonoBehaviour<GameInfomation>
{
    protected override bool dontDestroyOnLoad => true;

    private float _currentAnswerPer = 0f;
    public float CurrentAnswerPer {  get { return _currentAnswerPer; } }
    public void SetCurrentAnswerPer(float currentAnswerPer) {  this._currentAnswerPer = currentAnswerPer; }

    public void Initialize()
    {
        _currentAnswerPer = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
