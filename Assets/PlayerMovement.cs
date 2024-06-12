using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    enum heightLayers
    {
        L1,
        L2,
        L3
    };

    [SerializeField] List<WeaponBase> Weapons;
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;

    heightLayers Currentlayer = heightLayers.L2;
    bool SwitchingLayers = false;
    float LayerSwitchTimeS = 1;

    //aiming
    [SerializeField] LayerMask mask; //mask for any environment in free aim(also includes targettable)
    [SerializeField] LayerMask Targetmask; //mask for targettable objects
    [SerializeField] Transform PlanePos;
    Plane AimPlane;
    bool FreeAim;
    bool Aiming;
    float ClickTimeStamp;
    GameObject Target;


    [SerializeField] Camera cam;


    void Start()
    {
        
    }

    void Update()
    {
        //transform.position += transform.forward * 10 * Time.deltaTime;
        rb.AddForce((transform.forward * speed) * Time.deltaTime);

        //MARK TARGET
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {

            //TODO: Record mouse pos for later comparison
            foreach(WeaponBase weapon in Weapons)
            {
                ClickTimeStamp = Time.time + 0.3f;

            }
        }


        //FREE AIM
        if (Input.GetKey(KeyCode.Mouse0) && (ClickTimeStamp < Time.time || FreeAim) && !EventSystem.current.IsPointerOverGameObject())
        {
            FreeAim = true;
            Aiming = true;
            //Plane ray
            Ray Pray = cam.ScreenPointToRay(Input.mousePosition);
            float PHit = 0;
            AimPlane = new Plane(PlanePos.forward, PlanePos.position);

            //phys ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Remove target if there is one
            if (Target != null)
            {
                Target = null;
            }

            //free aim with phys ray(Collides with environment)
            if (Physics.Raycast(ray, out hit, 200, mask))
            {
                foreach(WeaponBase weapon in Weapons)
                {
                    weapon.FadeCrosshair(true);


                    weapon.transform.rotation = Quaternion.LookRotation((hit.point - transform.position).normalized);
                weapon.targetPos = hit.point;
                weapon.CrosshairObj.position = cam.WorldToScreenPoint(hit.point);
                weapon.Shoot();
                }

            }
            else if (AimPlane.Raycast(Pray, out PHit)) //if no environment object was hit use the background plane
            {
                Vector3 dir = Pray.GetPoint(PHit);

                foreach (WeaponBase weapon in Weapons)
                {
                    weapon.transform.rotation = Quaternion.LookRotation((dir - transform.position).normalized);
                    weapon.CrosshairObj.position = cam.WorldToScreenPoint(dir);
                    weapon.targetPos = Pray.GetPoint(PHit);

                    weapon.FadeCrosshair(true);

                    weapon.Shoot();
                }
            }
        }
        else if (Target != null) //Shoot at target
        {
            foreach (WeaponBase weapon in Weapons)
            {

                weapon.FadeCrosshair(true);

                weapon.targetPos = Target.transform.position;
                weapon.transform.rotation = Quaternion.LookRotation((Target.transform.position - transform.position).normalized);
                weapon.CrosshairObj.position = cam.WorldToScreenPoint(Target.transform.position);
                weapon.Shoot();
            }
        }
        else if (Target == null && Aiming)
        {
            StopAim();
        }

        //DISABLE FREE AIM
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (FreeAim)
            {
                StopAim(); //stop freeaim
                return;

            }
            else if (!FreeAim)  //MARK TARGET
            {
                Ray TargetFinder = cam.ScreenPointToRay(Input.mousePosition); ;
                RaycastHit[] hit = Physics.SphereCastAll(TargetFinder, 5, 200, Targetmask, QueryTriggerInteraction.UseGlobal);

                if (hit.Length > 0)
                {
                    GameObject NewTarget = null;
                    float Dist = 100;

                    foreach (RaycastHit raycastHit in hit)
                    {
                        if (Vector3.Distance(raycastHit.point, raycastHit.transform.position) < Dist)
                        {
                            Dist = Vector3.Distance(TargetFinder.GetPoint(Vector3.Distance(cam.transform.position, raycastHit.transform.position)), raycastHit.transform.position);
                            NewTarget = raycastHit.transform.gameObject;
                        }
                    }

                    Target = NewTarget;
                    Aiming = true;
                    foreach (WeaponBase weapon in Weapons)
                    {
                        weapon.targetPos = Target.transform.position;
                    }
                }
            }


        }
    }

    public virtual void StopAim()
    {

        FreeAim = false;
        Aiming = false;
        Target = null;
        Debug.Log("MARK");

        foreach (WeaponBase weapon in Weapons)
        {
            weapon.FadeCrosshair(false);
        }
    }

    public void LayerUp()
    {
        switch (Currentlayer)
        {
            case heightLayers.L1:
                SwitchHeight(heightLayers.L2);
                break;
            case heightLayers.L2:
                SwitchHeight(heightLayers.L3);
                break;
            case heightLayers.L3:
                Debug.LogWarning("cannot go Higher");
                break;
        }
    }

    public void LayerDown()
    {
        switch (Currentlayer)
        {
            case heightLayers.L3:
                SwitchHeight(heightLayers.L2);
                break;
            case heightLayers.L2:
                SwitchHeight(heightLayers.L1);
                break;
            case heightLayers.L1:
                Debug.LogWarning("cannot go Lower");
                break;
        }
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
            yield return null;
        }


        SwitchingLayers = false;
        yield return null;
    }
}
