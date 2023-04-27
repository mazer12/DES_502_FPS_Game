using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VFX;

public class Gun : Item
{

    [SerializeField] Camera cam;
    [SerializeField] VisualEffect muzzleFlash;
    public Transform attackPoint;
    public GameObject bullet;
    public float bulletSpeed;

    PhotonView PV;

    public void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        shoot();
    }

   
    private void shoot()
    {
        muzzleFlash.Play();

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        RaycastHit hit;

        Vector3 targetPoint;

        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            Debug.Log("we shot " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo)itemInfo).damage);

        } else { targetPoint = ray.GetPoint(75);}

        Vector3 dirOfBullet = targetPoint- attackPoint.position;

        GameObject currentBullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = dirOfBullet.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(dirOfBullet.normalized * bulletSpeed, ForceMode.Impulse);

      
    }

   

}
