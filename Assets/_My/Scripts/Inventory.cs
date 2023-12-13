using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoruActivated = false;

    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotParent;

    //����
    private Slot[] slots;

    void Start()
    {
        slots = go_SlotParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
        UpdateMouseCursor();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoruActivated = !inventoruActivated;

            if (inventoruActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // ���콺 Ŀ�� ��� ����
        Cursor.visible = true; // ���콺 Ŀ�� ǥ��
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� ���
        Cursor.visible = false; // ���콺 Ŀ�� ����
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemName == _item.itemName)
                {
                    slots[i].SetSlotCount(_count);
                    return;
                }
            }
            
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }

    private void UpdateMouseCursor()
    {
        // �κ��丮�� �������� ���� �׻� ���콺 Ŀ���� ���̰� ����
        Cursor.visible = inventoruActivated;
    }
}
