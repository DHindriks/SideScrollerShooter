using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Picks a random prefab from the list to spawn.
/// </summary>
public class SpawnFortress : MonoBehaviour
{

    [SerializeField]
    List<GameObject> Structures;
    void Start()
    {
        if (Structures.Count != 0)
        {
            GameObject Structure = Instantiate(Structures[Random.Range(0, Structures.Count)]);
            Structure.transform.position = transform.position;
        }
    }
}
