using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMapArea : MonoBehaviour
{
    private LevelLoaderManager _levelLoader;

    private void Start()
    {
        _levelLoader = (LevelLoaderManager)FindObjectOfType(typeof(LevelLoaderManager));
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            _levelLoader.LoadNextScene();
        }
    }
}
