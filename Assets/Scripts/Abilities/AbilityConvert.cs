using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityConvert : AbilityBase
{
    [SerializeField]
    int ExplosionRange;

    [SerializeField]
    Rigidbody ConvertedInto;

    [SerializeField]
    Material NewMat;

    [SerializeField]
    int NewHealth;

    [SerializeField]
    GameObject ParticlePrefab;

    void Update()
    {
        if (!AbilityLocked && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, ExplosionRange);

            foreach (Collider c in objects)
            {
                Rigidbody r = c.GetComponent<Rigidbody>();
                if (c.gameObject.GetComponent<MeshRenderer>() && c.gameObject.tag == "Block")
                {
                    c.gameObject.GetComponent<MeshRenderer>().material = NewMat;
                }
                if (c.gameObject.GetComponent<Health>())
                {
                    c.gameObject.GetComponent<Health>().health = NewHealth;
                }
                if (r != null)
                {
                    r = ConvertedInto;
                }
            }

            GameObject Particle = Instantiate(ParticlePrefab);
            Particle.transform.position = transform.position;

            Destroy(gameObject);
        }
    }

}
