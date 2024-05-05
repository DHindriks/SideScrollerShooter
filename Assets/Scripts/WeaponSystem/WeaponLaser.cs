using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : WeaponBase
{

    [SerializeField] LineRenderer laser;

    public override void Shoot()
    {
        if (!overHeating && currentCooldown < Time.time && targetPos != Vector3.zero)
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
                overHeating = true;
                laser.gameObject.SetActive(false);
                Invoke("ResetOverheat", weaponStats.overHeatCooldown);
                OverHeatParticles.Play();
            }
        }
    }

    public override void ResetOverheat()
    {
        base.ResetOverheat();
        laser.gameObject.SetActive(true);
    }

    public override void StopAim()
    {
        base.StopAim();
        laser.gameObject.SetActive(false);
    }
}
