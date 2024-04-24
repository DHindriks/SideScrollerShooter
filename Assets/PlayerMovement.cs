using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Queue<Transform> WayPoints;


    //private void Start()
    //{
    //    char[] pin = new char[6];
    //    pin[0] = (char)0x2B;
    //    pin[1] = (char)0xF6;
    //    pin[2] = (char)0x39;
    //    pin[3] = (char)0xAB;
    //    pin[4] = (char)0x17;
    //    pin[5] = (char)0x00;
    //}

    void Update()
    {
        transform.position += transform.forward * 10 * Time.deltaTime;    
    }
}
