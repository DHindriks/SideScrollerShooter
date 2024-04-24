using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    public Sprite CrosshairSprite;
}




public class WeaponBase : MonoBehaviour
{

    [HideInInspector] public bool overHeating;
    [HideInInspector] public float OverHeatScale;
    [HideInInspector] public float currentCooldown;
    public ParticleSystem OverHeatParticles;

    public WeaponStats weaponStats;


    GameObject Target;

    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask Targetmask;


    [SerializeField] Camera cam;

    [SerializeField]
    Transform PlanePos;

    [SerializeField] Transform CrosshairObj;
    [SerializeField] Animator CrosshairAnimator;
    Plane AimPlane;
    bool FreeAim;
    float ClickTimeStamp;
    private void Start()
    {
        CrosshairObj.GetComponent<Image>().sprite = weaponStats.CrosshairSprite;
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

    public virtual void Update()
    {
        //MARK TARGET
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
            }else
            {
                FreeAim = true;
            }


            ClickTimeStamp = Time.time + 0.75f;
        }


        //FREE AIM
        if (Input.GetKey(KeyCode.Mouse0)  && (ClickTimeStamp < Time.time || FreeAim))
        {
            //Plane ray
            Ray Pray = cam.ScreenPointToRay(Input.mousePosition);
            float PHit = 0;
            AimPlane = new Plane(PlanePos.forward, PlanePos.position);

            //phys ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Target != null)
            {
                Target = null;
            }

            if (Physics.Raycast(ray, out hit, 200, mask))
            {
                //if (hit.transform.root.GetComponent<Health>())
                //{
                //    Target = hit.transform.root.gameObject;
                //}

                //CROSSHAIR
                if (!CrosshairAnimator.GetBool("FadedIn"))
                {
                    CrosshairAnimator.SetBool("FadedIn", true);
                }
                

                transform.rotation = Quaternion.LookRotation((hit.point - transform.position).normalized);
                CrosshairObj.position = cam.WorldToScreenPoint(hit.point);
                Shoot();

            }
            else if (AimPlane.Raycast(Pray, out PHit))
            {
                Vector3 dir = Pray.GetPoint(PHit);
                transform.rotation = Quaternion.LookRotation((dir - transform.position).normalized);
                CrosshairObj.position = cam.WorldToScreenPoint(dir);

                //CROSSHAIR
                if (!CrosshairAnimator.GetBool("FadedIn"))
                {
                    CrosshairAnimator.SetBool("FadedIn", true);
                }

                Shoot();
            }
        }else if (Target != null)
        {
            //CROSSHAIR
            if (!CrosshairAnimator.GetBool("FadedIn"))
            {
                CrosshairAnimator.SetBool("FadedIn", true);
            }


            transform.rotation = Quaternion.LookRotation((Target.transform.position - transform.position).normalized);
            CrosshairObj.position = cam.WorldToScreenPoint(Target.transform.position);
            Shoot();
        }
        else
        {
            //CROSSHAIR
            if (CrosshairAnimator.GetBool("FadedIn"))
            {
                CrosshairAnimator.SetBool("FadedIn", false);
            }
        }

        //DISABLE FREE AIM
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (FreeAim)
            {
                FreeAim = false;
                Target = null;
            }
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
