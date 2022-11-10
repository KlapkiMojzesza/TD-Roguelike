using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageUIManager : MonoBehaviour
{
    public void ReturnToLevelButton()
    { 
        SceneManager.LoadScene(1);
    }
}
