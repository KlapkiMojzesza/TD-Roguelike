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
    [SerializeField] private TMP_Text _obstacleNameText;
    [SerializeField] private TMP_Text _obstaclePriceText;
    [SerializeField] private RawImage _iconImage;

    private Controls _controls;
    private Obstacle _currentObstacle;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Info.performed += HandlePlayerMouseInfo;
    }

    private void OnDestroy()
    {
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
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
                UpdateCanvasInfo(obstacle.GetObstacleData());
                _obstacleInfoCanvas.SetActive(true);
                _currentObstacle = obstacle;
                return;
            }

        }
            _currentObstacle = null;
            _obstacleInfoCanvas.SetActive(false);
    }

    private void UpdateCanvasInfo(ObstacleScriptableObject obstacleData)
    {
        _obstacleNameText.text = obstacleData.ObstacleName;
        _obstaclePriceText.text = obstacleData.RemovePrice.ToString();
        _iconImage.texture = obstacleData.ObstacleIcon;
    }

    public void DestroyObstacle()
    {
        if (_currentObstacle != null)
        {
            _currentObstacle.Remove();
            _currentObstacle = null;
            _obstacleInfoCanvas.SetActive(false);
        }
    }
}
