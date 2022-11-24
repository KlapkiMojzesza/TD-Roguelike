using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _icons;
    [SerializeField] private GameObject[] _iconsSelection;

    public void SwitchTower(int iconIndex)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            if (_icons[i] == _icons[iconIndex])
            {
                _iconsSelection[i].SetActive(true);
                _icons[i].SetActive(false);
            }
            else
            {
                _iconsSelection[i].SetActive(false);
                _icons[i].SetActive(true);
            }
        }
    }
}
