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

    [SerializeField]
    private float range; 

    private bool Activated = false; 

    private RaycastHit hitInfo; 

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    [SerializeField]

    private void Start()
    {
        fire.SetActive(false);
        input = GetComponentInParent<StarterAssetsInputs>();
        gameManager = FindObjectOfType<GameManger>();
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

        if(systemOff)
            fire.SetActive(true);
    }

    private void CanAction()
    {
        if (Activated)
        {
            if (hitInfo.transform != null)
            {
                systemOff = true;
                Destroy(hitInfo.transform.gameObject);
                Disappear();
            }
        }

    }

    private void CheckTrigger()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "ClearTrigger")
            {
                Appear();
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
        actionText.gameObject.SetActive(true);
        actionText.text = "½Ã½ºÅÛ ÆÄ±« " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void Disappear()
    {
        Activated = false;
        actionText.gameObject.SetActive(false);
    }

}
