using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Rigidbody bulletRigibody;

    [SerializeField]
    private float moveSpeed = 10f;
    private float destroyTime = 3f;


    void Start()
    {
        bulletRigibody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0)
        {
            DestroyBullet();
        }

        BullectMove();
    }

    private void BullectMove()
    {
        bulletRigibody.velocity = transform.forward * moveSpeed;
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
        destroyTime = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyBullet();
    }
}