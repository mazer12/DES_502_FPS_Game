using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Gun : Item
{

    [SerializeField] Camera cam;
    public Transform attackPoint;
    public GameObject bullet;

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
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f));
        RaycastHit hit;

        Vector3 targetPoint;

        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            //Debug.Log("we shot " + hit.collider.gameObject.name);
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo)itemInfo).damage);

        } else { targetPoint = ray.GetPoint(75);}

        Vector3 dirOfBullet = targetPoint- attackPoint.position;

        GameObject currentBullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = dirOfBullet.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(dirOfBullet.normalized * 20.0f, ForceMode.Impulse);

      
    }

   

}
