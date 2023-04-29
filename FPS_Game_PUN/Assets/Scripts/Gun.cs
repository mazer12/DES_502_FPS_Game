using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.VFX;

public class Gun : Item
{

    [SerializeField] Camera cam;
    public Transform attackPoint;
    public GameObject bullet;
    public VisualEffect muzzleFlash;
    public GameObject hitMarker;
    public AudioSource hitSound;
    public float bulletSpeed = 250f;
    List<GameObject> bulletArray = new List<GameObject>();

    PhotonView PV;

    public void Awake()
    {
        PV = GetComponent<PhotonView>();
        hitMarker.SetActive(false);
    }
    public override void Use()
    {
        shoot();
        //kill();
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
            if (hit.collider.tag == "Enemy" || hit.collider.tag == "Player")
            {
                hitMarker.SetActive(true);
                hitSound.Play();
                StartCoroutine(HitOff());
            }

        } else { targetPoint = ray.GetPoint(75);}

        Vector3 dirOfBullet = targetPoint- attackPoint.position;

        GameObject currentBullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = dirOfBullet.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(dirOfBullet.normalized * bulletSpeed, ForceMode.Impulse);
        //bulletArray.Add(currentBullet);
   
    }

    IEnumerator HitOff()
    {
        yield return new WaitForSeconds(0.2f);
        hitMarker.SetActive(false);
    }

    //private void OnCollisionEnter(Collision collison)
    //{
    //    kill();
        
    //}

    //IEnumerator wait()
    //{
    //    yield return new WaitForSeconds(2);
    //}
    //private void kill()
    //{
    //    StartCoroutine(wait());
    //    PhotonNetwork.Destroy(bulletArray[0]);
    //    bulletArray.RemoveAt(0);
    //}
}
