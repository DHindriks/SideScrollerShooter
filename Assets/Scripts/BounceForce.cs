using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceForce : MonoBehaviour
{
    [SerializeField]
    float ForceMultiplier;

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity *= ForceMultiplier;
        }
    }
}
