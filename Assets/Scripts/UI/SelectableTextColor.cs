using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectableTextColor : MonoBehaviour
{
    [SerializeField, Header("���䂷��e�L�X�g")]
    private TextMeshProUGUI _tmPro;

    [SerializeField, Header("��I�����̐F")]
    private Color _unselectedColor;

    [SerializeField, Header("�I�����̐F")]
    private Color _selectedColor;

    /// <summary>
    /// �I�𒆂��ɂ���ĐF��ύX
    /// </summary>
    /// <param name="selected">�I�𒆂�</param>
    public void ChangeColor(bool selected)
    {
        if (selected)
            _tmPro.color = _selectedColor;
        else
            _tmPro.color = _unselectedColor;
    }
}
