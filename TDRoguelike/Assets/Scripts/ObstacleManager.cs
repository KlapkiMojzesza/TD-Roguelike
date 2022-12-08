using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObstacleManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _obstacleLayer;

    [Header("To Attach")]
    [SerializeField] private GameObject _obstacleInfoCanvas;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private TMP_Text _obstacleNameText;
    [SerializeField] private TMP_Text[] _obstaclePriceTexts;
    [SerializeField] private RawImage _iconImage;

    private TowerManager _towerManager;
    private Controls _controls;
    private Obstacle _currentObstacle;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();

        _towerManager = GameObject.FindGameObjectWithTag("TowerManager").GetComponent<TowerManager>();

        _controls.Player.Info.performed += HandlePlayerMouseInfo;
        TowerManager.OnTowerPlaced += HideUI;
    }

    private void OnDestroy()
    {
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
        TowerManager.OnTowerPlaced -= HideUI;
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit obstacleHit;

        if (Physics.Raycast(ray, out obstacleHit, Mathf.Infinity, _obstacleLayer))
        {
            GameObject obstacleHitGameObject = obstacleHit.transform.gameObject;
            Obstacle obstacle = obstacleHitGameObject.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                UpdateCanvasInfo(obstacle);

                if (obstacle.GetRemovePrice() <= _towerManager.GetCurrentMoneyAmount()) _buyButton.SetActive(true);
                else _buyButton.SetActive(false);

                _obstacleInfoCanvas.SetActive(true);
                _currentObstacle = obstacle;
                return;
            }

        }
            _currentObstacle = null;
            _obstacleInfoCanvas.SetActive(false);
    }

    private void UpdateCanvasInfo(Obstacle obstacle)
    {
        _obstacleNameText.text = obstacle.GetObstacleData().ObstacleName;
        _iconImage.texture = obstacle.GetObstacleData().ObstacleIcon;

        foreach (TMP_Text priceText in _obstaclePriceTexts)
        {
            priceText.text = obstacle.GetRemovePrice().ToString();
        }
    }

    private void HideUI()
    {
        _obstacleInfoCanvas.SetActive(false);
    }

    public void DestroyObstacle()
    {
        if (_currentObstacle != null)
        {
            _towerManager.RemoveObstacle(_currentObstacle.GetRemovePrice());
            _currentObstacle.Remove();
            _currentObstacle = null;
            _obstacleInfoCanvas.SetActive(false);
        }
    }
}
