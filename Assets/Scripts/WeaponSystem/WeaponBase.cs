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

    public virtual void FadeCrosshair()
    {

    }

    public virtual void Update()
    {


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
