using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCannon : WeaponBase
{
    [SerializeField]
    float SpreadAngleX;

    [SerializeField]
    float SpreadAngleY;


    public override void Shoot()
    {
        if (!overHeating && currentCooldown < Time.time)
        {
            CancelInvoke();
            GameObject bullet = Instantiate(weaponStats.BulletPrefab);

            Vector3 BulletRot = transform.rotation.eulerAngles;
            BulletRot.x += Random.Range(-SpreadAngleX, SpreadAngleX);
            BulletRot.y += Random.Range(-SpreadAngleY, SpreadAngleY);
            bullet.transform.rotation = Quaternion.Euler(BulletRot.x, BulletRot.y, BulletRot.z);
            bullet.GetComponent<ProjectileData>().Origin = transform.root.gameObject;
            bullet.GetComponent<ProjectileData>().Damage = weaponStats.damage;
            bullet.GetComponent<ProjectileData>().Lifetime = weaponStats.BulletLifeTime;

            bullet.transform.position = transform.position;
            //bullet.GetComponent<Rigidbody>().velocity = transform.root.GetComponent<Rigidbody>().velocity;
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * weaponStats.BulletSpeed, ForceMode.VelocityChange);
            OverHeatScale += weaponStats.HeatAccumulation;

            currentCooldown = Time.time + weaponStats.fireRateRPS;

            if (OverHeatScale >= 10)
            {
                overHeating = true;
                Invoke("ResetOverheat", weaponStats.overHeatCooldown);
                OverHeatParticles.Play();
            }
        }
    }
}
