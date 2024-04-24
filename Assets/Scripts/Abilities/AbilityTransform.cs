using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTransform : AbilityBase
{
    [SerializeField]
    GameObject TransformInto;

    [SerializeField]
    bool RotateTransformed;

    void Update()
    {
        if (!AbilityLocked && Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject Transformed = Instantiate(TransformInto);
            Transformed.transform.position = transform.position;
            if (RotateTransformed)
            {
                Transformed.transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
                Debug.Log(GetComponent<Rigidbody>().velocity);
            }

            Destroy(gameObject);
        }
    }

}
