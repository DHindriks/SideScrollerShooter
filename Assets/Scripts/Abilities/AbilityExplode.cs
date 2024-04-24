using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityExplode : AbilityBase
{
    [SerializeField]
    int ExplosionRange;


    [SerializeField]
    int ExplosionForce = 40000;

    [SerializeField]
    GameObject ParticlePrefab;

    [SerializeField]
    int Delay = 0;

    bool exploding = false;

    ProjectileData data;

    public override void Activate()
    {
        if (!exploding)
        {
            data = GetComponent<ProjectileData>();
            data.CancelInvoke();
            exploding = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Invoke("Explode", Delay);
            GameObject Particle = Instantiate(ParticlePrefab);
            Particle.transform.position = transform.position;
        }
    }

    void Explode()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, ExplosionRange);
        foreach (Collider c in objects)
        {
            Rigidbody r = c.GetComponent<Rigidbody>();
            if (r != null && c.tag != "Projectile")
            {
                r.AddExplosionForce(ExplosionForce, transform.position, ExplosionRange);
            }

            if (c.GetComponent<Health>())
            {
                c.GetComponent<Health>().Damage(data.Damage);
            }

        }

        Destroy(gameObject);
    }

}
