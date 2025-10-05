using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectableTextColor : MonoBehaviour
{
    [SerializeField, Header("制御するテキスト")]
    private TextMeshProUGUI _tmPro;

    [SerializeField, Header("非選択時の色")]
    private Color _unselectedColor;

    [SerializeField, Header("選択時の色")]
    private Color _selectedColor;

    /// <summary>
    /// 選択中かによって色を変更
    /// </summary>
    /// <param name="selected">選択中か</param>
    public void ChangeColor(bool selected)
    {
        if (selected)
            _tmPro.color = _selectedColor;
        else
            _tmPro.color = _unselectedColor;
    }
}
