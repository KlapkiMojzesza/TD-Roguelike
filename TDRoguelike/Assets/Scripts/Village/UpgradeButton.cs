using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] RawImage upgradeIcon;
    [SerializeField] UpgradeScriptableObject upgradeData;
    public Vector2Int buttonPosition = Vector2Int.zero;

    public static event Action<UpgradeScriptableObject> OnUpgradeChoose;

    private void Start()
    {
        upgradeIcon.texture = upgradeData.upgradeIcon;
    }

    public void Upgrade()
    {
        OnUpgradeChoose?.Invoke(upgradeData);
    }
}
