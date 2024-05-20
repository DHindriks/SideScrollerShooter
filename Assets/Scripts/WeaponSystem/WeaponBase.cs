using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[Serializable]
public struct WeaponStats
{    //heat accumulation per shot fired
    public float HeatAccumulation;

    public float overHeatCooldown;

    public float fireRateRPS;

    public int damage;

    public float BulletSpeed;

    public float BulletLifeTime;

    public GameObject BulletPrefab;

    public GameObject Crosshairobj;
}




public class WeaponBase : MonoBehaviour
{

    [HideInInspector] public bool overHeating;
    [HideInInspector] public float OverHeatScale;
    [HideInInspector] public float currentCooldown;
    public ParticleSystem OverHeatParticles;

    public WeaponStats weaponStats;



    //aiming
    [SerializeField] LayerMask mask; //mask for any environment in free aim(also includes targettable)
    [SerializeField] LayerMask Targetmask; //mask for targettable objects
    [SerializeField] Transform PlanePos;
    Plane AimPlane;
    bool FreeAim;
    float ClickTimeStamp;
    GameObject Target;
    public Vector3 targetPos; 


    [SerializeField] Camera cam;


    //crosshair
    [SerializeField] Transform CrosshairObj;
    GameObject CrosshairInstance;
    Animator CrosshairAnimator;


    private void Start()
    {
        CrosshairInstance = Instantiate(weaponStats.Crosshairobj, CrosshairObj);
        CrosshairAnimator = CrosshairInstance.GetComponent<Animator>();
    }

    public virtual void Shoot()
    {

    }

    public virtual void Equip()
    {

    }

    public virtual void Unequip()
    {

    }

    public virtual void StopAim()
    {
        if (FreeAim)
        {
            FreeAim = false;
            Target = null;
        }
    }

    public virtual void Update()
    {
        //MARK TARGET
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray TargetFinder = cam.ScreenPointToRay(Input.mousePosition); ;
            RaycastHit[] hit = Physics.SphereCastAll(TargetFinder, 5, 200, Targetmask, QueryTriggerInteraction.UseGlobal);

            if (hit.Length > 0)
            {
                GameObject NewTarget = null;
                float Dist = 100;

                foreach(RaycastHit raycastHit in hit)
                {
                    if (Vector3.Distance(raycastHit.point, raycastHit.transform.position) < Dist)
                    {
                        Dist = Vector3.Distance(TargetFinder.GetPoint(Vector3.Distance(cam.transform.position, raycastHit.transform.position)), raycastHit.transform.position);
                        NewTarget = raycastHit.transform.gameObject;
                    }
                }

                Target = NewTarget;
                targetPos = Target.transform.position;
            }else
            {
                FreeAim = true;
            }


            ClickTimeStamp = Time.time + 0.3f;
        }


        //FREE AIM
        if (Input.GetKey(KeyCode.Mouse0)  && (ClickTimeStamp < Time.time || FreeAim) && !EventSystem.current.IsPointerOverGameObject())
        {
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
                //CROSSHAIR
                if (!CrosshairAnimator.GetBool("FadedIn"))
                {
                    CrosshairAnimator.SetBool("FadedIn", true);
                }
                

                transform.rotation = Quaternion.LookRotation((hit.point - transform.position).normalized);
                CrosshairObj.position = cam.WorldToScreenPoint(hit.point);
                targetPos = hit.point;
                Shoot();

            }
            else if (AimPlane.Raycast(Pray, out PHit)) //if no environment object was hit use the background plane
            {
                Vector3 dir = Pray.GetPoint(PHit);
                targetPos = Pray.GetPoint(PHit);
                transform.rotation = Quaternion.LookRotation((dir - transform.position).normalized);
                CrosshairObj.position = cam.WorldToScreenPoint(dir);

                //CROSSHAIR
                if (!CrosshairAnimator.GetBool("FadedIn"))
                {
                    CrosshairAnimator.SetBool("FadedIn", true);
                }

                Shoot();
            }
        }else if (Target != null) //Shoot at target
        {
            //CROSSHAIR
            if (!CrosshairAnimator.GetBool("FadedIn"))
            {
                CrosshairAnimator.SetBool("FadedIn", true);
            }


            transform.rotation = Quaternion.LookRotation((Target.transform.position - transform.position).normalized);
            CrosshairObj.position = cam.WorldToScreenPoint(Target.transform.position);
            targetPos = Target.transform.position;
            Shoot();
        }
        else //if there is no target and no free aiming, stop aiming
        {
            //CROSSHAIR
            if (CrosshairAnimator.GetBool("FadedIn"))
            {
                CrosshairAnimator.SetBool("FadedIn", false);
                StopAim();
            }
        }

        //DISABLE FREE AIM
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopAim();
        }

    }

    public virtual void ResetOverheat()
    {
        overHeating = false;
        OverHeatScale = 0;
        if (OverHeatParticles != null)
        {
            OverHeatParticles.Stop();
        }
    }


}
