using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;


    public Camera playerCamera;
    public bool isShooting, readyToShoot;
    public float shootingDelay = 2f;
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    public float spreadInternsity;
    bool allowReset = true;

    public enum ShootingMode
    {
        Single,
        Burst, Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);

        } else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
        //if(Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    FireWeapon();


        //}
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }


        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;

    }

    public Vector3 CalculateDirectionSpread()
    {
        // Get the center of the camera's viewport (middle of the screen)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        // If ray hits an object, use that hit point as the target point
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Otherwise, set the target point 100 units in front of the camera
            targetPoint = ray.GetPoint(100);
        }

        // Calculate the direction from the spawn point to the target point
        Vector3 direction = targetPoint - bulletSpawn.position;

        // Apply spread on all three axes
        float xSpread = UnityEngine.Random.Range(-spreadInternsity, spreadInternsity);
        float ySpread = UnityEngine.Random.Range(-spreadInternsity, spreadInternsity);
        float zSpread = UnityEngine.Random.Range(-spreadInternsity, spreadInternsity);

        // Return the direction with the spread applied
        return direction + new Vector3(xSpread, ySpread, zSpread);
    }
    private IEnumerator  DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
