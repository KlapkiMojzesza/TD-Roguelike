using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCursor : MonoBehaviour
{   
    [Header("Settings")]
    [SerializeField] private LayerMask _obstacleLayer;

    [Header("To Attach")]
    [SerializeField] private Texture2D _textureOverUI;
    [SerializeField] private Texture2D _textureOverTower;
    [SerializeField] private Texture2D _textureOverObstacle;
    [SerializeField] private Texture2D _textureOverOverGround;

    private Vector2 _uiCursorHotspot;
    private Vector2 _towerCursorHotspot;
    private Vector2 _obstacleCursorHotspot;
    private Vector2 _groundCursorHotspot;

    private RaycastHit _obstacleHit;

    private void Start()
    {
        _groundCursorHotspot = new Vector2(_textureOverOverGround.width / 2, _textureOverOverGround.height / 2);
        Cursor.SetCursor(_textureOverOverGround, _groundCursorHotspot, CursorMode.Auto);

        _uiCursorHotspot = new Vector2(5f, 5f);
        _towerCursorHotspot = new Vector2(5f, 5f);
        _obstacleCursorHotspot = new Vector2(5f, 5f);
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.SetCursor(_textureOverUI, _uiCursorHotspot, CursorMode.Auto);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _obstacleHit, Mathf.Infinity, _obstacleLayer))
        {
            if (_obstacleHit.collider.CompareTag("Tower"))
            {
                Cursor.SetCursor(_textureOverTower, _towerCursorHotspot, CursorMode.Auto);
                return;
            }

            Cursor.SetCursor(_textureOverObstacle, _obstacleCursorHotspot, CursorMode.Auto);
            return;
        }

        Cursor.SetCursor(_textureOverOverGround, _groundCursorHotspot, CursorMode.Auto);

    }
}
