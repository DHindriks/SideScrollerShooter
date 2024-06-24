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

    GameObject Structure;
    void Start()
    {
        if (Structures.Count != 0)
        {
            Structure = Instantiate(Structures[Random.Range(0, Structures.Count)]);
            Structure.transform.position = transform.position;
        }
    }

    private void OnDestroy()
    {
        Destroy(Structure);
    }
}
