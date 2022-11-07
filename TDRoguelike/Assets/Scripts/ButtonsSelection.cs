using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSelection : MonoBehaviour
{
    [SerializeField] GameObject[] icons;
    [SerializeField] GameObject[] iconsSelection;

    public void SwitchTower(int iconIndex)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] == icons[iconIndex])
            {
                iconsSelection[i].SetActive(true);
                icons[i].SetActive(false);
            }
            else
            {
                iconsSelection[i].SetActive(false);
                icons[i].SetActive(true);
            }
        }
    }
}
