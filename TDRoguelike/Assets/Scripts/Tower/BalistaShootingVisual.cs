using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalistaShootingVisual : MonoBehaviour
{
    [SerializeField] private GameObject _projectileVisual;

    public void ShowProjectile()
    {
        _projectileVisual.SetActive(true);
    }

    public void HideProjectile()
    {
        _projectileVisual.SetActive(false);
    }
}
