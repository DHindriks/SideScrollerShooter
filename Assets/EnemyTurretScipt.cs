using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretScipt : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject RocketPrefab;
    [SerializeField] int BarrageSize;
    [SerializeField] float RocketFireRate;
    [SerializeField] GameObject DeathParticles;

    int[] Layers = new int[] {25, 20, 15};
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Shoot()
    {
        if (player != null)
        {
            animator.SetBool("Activated", true);
            Invoke("SpawnRocket", 5 + RocketFireRate);
        }
    }

    void SpawnRocket()
    {
        GameObject Rocket = Instantiate(RocketPrefab);
        Rocket.transform.position = new Vector3(player.transform.position.x + 50, Layers[Random.Range(0, Layers.Length)], player.transform.position.z);
        BarrageSize--;
        if (BarrageSize > 0)
        {
            Invoke("SpawnRocket", RocketFireRate);
        }
    }

    public void HealthZero()
    {
        CancelInvoke();
        GameObject deathPart = Instantiate(DeathParticles);
        deathPart.transform.position = transform.position;
        Destroy(gameObject);
    }

}
