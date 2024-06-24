using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : WeaponBase
{

    [SerializeField] LineRenderer laser;


    Health TargetHealth;
    int CurrentDmg = 50;

    public override void Shoot()
    {
        if (!overHeating && currentCooldown < Time.time && targetPos != Vector3.zero && !FreeAim)
        {
            CancelInvoke();

            if (!laser.gameObject.activeSelf)
            {
                laser.gameObject.SetActive(true);
            }

            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, targetPos);

            OverHeatScale += weaponStats.HeatAccumulation;

            currentCooldown = Time.time + weaponStats.fireRateRPS;

            if (OverHeatScale >= 10)
            {
                if (Target.GetComponent<Health>())
                {
                    Target.GetComponent<Health>().Damage(CurrentDmg);
                    CurrentDmg += 50;
                }
                //overHeating = true;
                //laser.gameObject.SetActive(false);
                OverHeatParticles.Play();
                Invoke("ResetOverheat", weaponStats.overHeatCooldown);
            }
        }
    }

    public override void ResetOverheat()
    {
        overHeating = false;
        OverHeatScale = 0;
        laser.gameObject.SetActive(true);
    }

    public override void StopAim()
    {
        base.StopAim();
        laser.gameObject.SetActive(false);
        CurrentDmg = 50;
    }
}
