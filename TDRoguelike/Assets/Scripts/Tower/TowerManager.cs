using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int startMoneyAmount = 100;
    [SerializeField] private LayerMask groundLayer;

    [Header("To Attach")]
    [SerializeField] TMP_Text moneyAmountText;
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] GameObject towersCanvas;

    //remove serialize
    [SerializeField] List<GameObject> towersPlaced = new List<GameObject>();

    public static event Action OnTowerSelect;
    public static event Action OnTowerDeselect;
    public static event Action OnMouseButtonEnter;
    public static event Action OnMouseButtonExit;
    public static event Action OnNextWaveButtonClicked;

    private int currentMoneyAmount;
    private GameObject currentTowerPrefab;
    private Tower currentTower;
    private bool mouseOverButton = true;

    private void Start()
    {
        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString();

        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    private void OnDestroy()
    {
        PlayerBase.OnBaseDestroyed -= HandleBaseDestruction;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
    }

    void Update()
    {
        if (currentTowerPrefab == null) return;

        MoveTowerPrefab();

        if (Input.GetMouseButtonUp(0) && currentTower.CanBePlaced() && !mouseOverButton)
        {
            PlaceTower();
        }

    }

    private void MoveTowerPrefab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit groundHit;
        if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, groundLayer))
        {
            currentTowerPrefab.transform.position = groundHit.point;
        }
        currentTower.SetTowerColor();
    }

    private void PlaceTower()
    {
        currentMoneyAmount -= currentTower.GetTowerPrize();
        moneyAmountText.text = currentMoneyAmount.ToString();

        currentTower.SetOrginalColor();

        currentTower.PlaceTower();
        towersPlaced.Add(currentTowerPrefab);
        currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
    }

    public void SwitchTowers(int towerIndex)
    {
        if (towerPrefabs[towerIndex].GetComponent<Tower>().GetTowerPrize() > currentMoneyAmount) return;

        if (currentTowerPrefab != towerPrefabs[towerIndex])
        {
            Destroy(currentTowerPrefab);
            currentTowerPrefab = Instantiate(towerPrefabs[towerIndex]);
            currentTower = currentTowerPrefab.GetComponent<Tower>();
            OnTowerSelect?.Invoke();
        }
        else
        {
            Destroy(currentTowerPrefab);
            OnTowerDeselect?.Invoke();
        }
    }

    public void HandleWaveEnd(int amount)
    {
        currentMoneyAmount += amount;
        moneyAmountText.text = currentMoneyAmount.ToString();
    }

    private void HandleBaseDestruction()
    {
        foreach (GameObject tower in towersPlaced)
        {
            Destroy(tower);
        }

        EndLevel();
    }

    private void EndLevel()
    {
        towersPlaced.Clear();
        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString();
    }

    public void SpawnNextWave()
    {
        towersCanvas.SetActive(false);

        Destroy(currentTowerPrefab);
        OnTowerDeselect?.Invoke();

        mouseOverButton = false;
        OnMouseButtonExit?.Invoke();
        OnNextWaveButtonClicked?.Invoke();
    }

    public void mousceOverButtonEnter()
    {
        mouseOverButton = true;
        OnMouseButtonEnter?.Invoke();
    }

    public void mousceOverButtonExit()
    {
        mouseOverButton = false;
        OnMouseButtonExit?.Invoke();
    }

    public void VillageButton()
    {
        towersCanvas.SetActive(true);
    }

    public void ExitButton()
    {
        //todo
        //tp between vilage nad level
        //tower target choise
    }

}
