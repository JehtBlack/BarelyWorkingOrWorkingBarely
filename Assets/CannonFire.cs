﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour, IWeapon
{
    [SerializeField]
    private GameObject BulletObj = null;
    [Range(1, 100)][SerializeField]
    public float Power = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            UseWeapon();
    }

    public void UseWeapon()
    {
        if(BulletObj != null)
        {
            BulletControl bulletInstance = Instantiate(BulletObj, transform.position, Quaternion.Euler(new Vector3(0, 0, 1))).GetComponent<BulletControl>();

            if (bulletInstance != null)
                bulletInstance.SetDirection(transform, false);
            

            //if (bulletBody != null) {
            //    bulletBody.velocity = transform.forward * bulletInstance.Spe;

            //    Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            //}
        }
    }

}
