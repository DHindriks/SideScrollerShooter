using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSpawner : MonoBehaviour
{

    [SerializeField] GameObject PrefabToSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("GameManager") == null)
        {
            Instantiate(PrefabToSpawn);
        }
    }
}
