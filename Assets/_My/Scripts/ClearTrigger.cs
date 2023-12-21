using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cleartrigger : MonoBehaviour
{
    public bool systemOff = false;

    public GameObject fire;

    private StarterAssetsInputs input;
    private GameManger gameManager;
    private PlayerManager pm;
    private QManager qManager;

    [SerializeField]
    private float range; 

    private bool Activated = false;
    private bool hasExploded = false;

    private RaycastHit hitInfo; 

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText2;
    [SerializeField]
    private AudioClip boomSound;
    private AudioSource boom;

    private void Start()
    {
        boom = GetComponent<AudioSource>();
        fire.SetActive(false);
        pm = FindObjectOfType<PlayerManager>();
        input = GetComponentInParent<StarterAssetsInputs>();
        gameManager = FindObjectOfType<GameManger>();
        qManager = FindObjectOfType<QManager>();
    }

    void Update()
    {
        CheckTrigger();
        TryAction();
    }

    private void TryAction()
    {
        if (input.interaction)
        {
            CheckTrigger();
            CanAction();
        }

        if (systemOff && !hasExploded)
        {
            BoomSound();
            fire.SetActive(true);
            hasExploded = true; 
        }

    }

    private void CanAction()
    {
        if (Activated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "ClearTrigger")
                {
                    systemOff = true;
                    Destroy(hitInfo.transform.gameObject);
                    Disappear();
                }

                if (hitInfo.transform.tag == "End")
                {
                    pm.End();
                    Disappear();
                }

            }
        }

    }

    private void BoomSound()
    {
        boom.clip = boomSound;
        boom.Play();
    }

    private void CheckTrigger()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "ClearTrigger")
            {
                Appear();
            }

            if (hitInfo.transform.tag == "End")
            {
                Appear2();
            }

        }
        else
        {
            Disappear();
        }
    }

    private void Appear()
    {
        Activated = true;
        actionText2.gameObject.SetActive(true);
        actionText2.text = "½Ã½ºÅÛ ÆÄ±« " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void Appear2()
    {
        if (qManager.lastQComplete)
        {
            Activated = true;
            actionText2.gameObject.SetActive(true);
            actionText2.text = "º¸Æ®¿¡ Å¾½Â " + "<color=yellow>" + "(F)" + "</color>";
        }
    }

    private void Disappear()
    {
        Activated = false;
        actionText2.gameObject.SetActive(false);
    }

}
