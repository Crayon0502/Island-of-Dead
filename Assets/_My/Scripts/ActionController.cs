using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    private QManager qManager;
    private StarterAssetsInputs input;
    private GameManger gameManager;
    public bool isHasKey = false;
    public bool talkShowing = false;
    public bool questActivated = false;

    [SerializeField]
    private float range; // ���� ������ �ִ�Ÿ�

    private bool pickupActivated = false; // ���� ������ �� true
    
    private RaycastHit hitInfo; // �浹ü ���� ���� 

    //������ ���̾�� ���� �ϵ��� ���̾� ����ũ ����
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;

    private void Start()
    {
        qManager = FindObjectOfType<QManager>();
        input = GetComponentInParent<StarterAssetsInputs>();
        gameManager = FindObjectOfType<GameManger>();
    }

    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if(input.interaction)
        {
            CheckItem();
            CanPickUp();
            if(questActivated)
            {
                talkShowing = true;
                actionText.gameObject.SetActive(false);
                StartTalk();
                questActivated = false;


            }

        }
    }


    private void StartTalk()
    {
        if(questActivated)
        {
            qManager.TalkStart();
        }
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "Ammo")
                    gameManager.maxBullet += 60;

                else if (hitInfo.transform.tag == "Key")
                {
                    isHasKey = true;
                    theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                }
      
                else
                    theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);

                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }

    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }

            else if (hitInfo.transform.tag == "Key")
            {
                ItemInfoAppear();
            }

            else if (hitInfo.transform.tag == "Ammo")
            {
                AmmoInfoAppear();
            }
            else if (hitInfo.transform.tag == "NPC")
            {
                if (!talkShowing)
                    QInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void QInfoAppear()
    {
        questActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "�λ���� ���ο��� ���ɱ� " + "<color=yellow>" + "(F)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void AmmoInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "���� ���� ź ȹ�� " + "<color=yellow>" + "(F)" + "</color>";
    }
}
