using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData : MonoBehaviour
{
    public Sprite Icon;

    public float Damage;

    public bool DoImpactDMG;

    public bool DoImpactDMGOnce;

    public GameObject Origin;

    public float Lifetime;

    AbilityBase ability;

    Rigidbody rb;
    bool Impacted = false;
    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            rb = GetComponent<Rigidbody>();
        }else
        {
            Debug.LogWarning("Could not find rigidbody on this object: " + gameObject.name);
        }
        if (GetComponent<AbilityBase>())
        {
            ability = GetComponent<AbilityBase>();
        }

        Invoke("DestroySelf", Lifetime);

    }

    void OnTriggerEnter(Collider other)
    {
        if (DoImpactDMG && other.GetComponent<Health>() && !Impacted)
        {
            other.GetComponent<Health>().Damage(Damage);
            if (DoImpactDMGOnce)
            {
                Impacted = true;
            }
        }

        if (ability)
        {
            ability.Activate();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (DoImpactDMG && other.gameObject.GetComponent<Health>() && !Impacted)
        {
            other.gameObject.GetComponent<Health>().Damage(Damage);
            if (DoImpactDMGOnce)
            {
                Impacted = true;
            }
        }

        if (ability)
        {
            ability.Activate();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
