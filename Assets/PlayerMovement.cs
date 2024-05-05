using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Queue<Transform> WayPoints;
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;

    void Start()
    {
        
    }

    void Update()
    {
        //transform.position += transform.forward * 10 * Time.deltaTime;
        rb.AddForce((transform.forward * speed) * Time.deltaTime);
    }
}
