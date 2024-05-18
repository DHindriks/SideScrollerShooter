using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum heightLayers
    {
        L1,
        L2,
        L3
    };

    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;

    heightLayers Currentlayer;
    bool SwitchingLayers = false;
    float LayerSwitchTimeS = 1;

    void Start()
    {
        
    }

    void Update()
    {
        //transform.position += transform.forward * 10 * Time.deltaTime;
        rb.AddForce((transform.forward * speed) * Time.deltaTime);
    }

    void SwitchHeight(heightLayers NewLayer)
    {
        if (SwitchingLayers)
        {
            return;
        }
        Currentlayer = NewLayer;
        IEnumerator Move;
        switch (Currentlayer)
        {
            case heightLayers.L1:
                Move = MoveLayers(15);
                StartCoroutine(Move);
                break;
            case heightLayers.L2:
                Move = MoveLayers(20);
                StartCoroutine(Move); 
                break;
            case heightLayers.L3:
                Move = MoveLayers(25);
                StartCoroutine(Move);
                break;
        }
    }

    IEnumerator MoveLayers(float NewHeight)
    {
        SwitchingLayers = true;
        float EndTime = Time.time + LayerSwitchTimeS;
        float CurrentHeight = transform.position.y;
        float CurrentProgress;
        float StartTime = Time.time;
        float CurrentY;
        while (Time.time < EndTime)
        {
            CurrentProgress = Mathf.InverseLerp(StartTime, EndTime, Time.time);
            CurrentY = Mathf.Lerp(CurrentHeight, NewHeight, CurrentProgress);
            transform.position = new Vector3(transform.position.x, CurrentY, transform.position.z);
        }

        yield return null;
    }
}
