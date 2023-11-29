using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float maxShootDelay = 0.2f;
    [SerializeField]
    private float curruntShootDelay = 0.2f;
    [SerializeField]
    private Text bulletText;
    private int maxBullet = 30;
    private int curruntBullet = 0;


    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFX;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private Transform weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFX;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        curruntShootDelay = 0;

        InitBullet();
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = curruntBullet + " / " + maxBullet;
    }

    public void Shooting(Vector3 targetPosition)
    {
        curruntShootDelay += Time.deltaTime;

        if (curruntShootDelay < maxShootDelay || curruntBullet <= 0)
            return;

        curruntBullet -= 1;
        curruntShootDelay = 0;

        Instantiate(weaponFlashFX, bulletPoint);
        Instantiate(bulletCaseFX, bulletCasePoint);

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;
        Instantiate(bulletObj, bulletPoint.position, Quaternion.LookRotation(aim, Vector3.up));
    }

    public void ReloadClip()
    {
        Instantiate(weaponClipFX, weaponClipPoint);
        InitBullet();
    }

    private void InitBullet()
    {
        curruntBullet = maxBullet;
    }
}