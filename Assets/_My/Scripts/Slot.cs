using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IPointerClickHandler , IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Vector3 originPos;

    public Item item; //»πµÊ«— æ∆¿Ã≈€
    public int itemCount; // »πµÊ«— æ∆¿Ã≈€ ∞πºˆ
    public Image itemImage; // »πµÊ«— æ∆¿Ã≈€ ¿ÃπÃ¡ˆ

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private ItemEffectDatabase database;

    private void Start()
    {
        database = FindObjectOfType<ItemEffectDatabase>();
        originPos = transform.position;
    }

    //¿ÃπÃ¡ˆ ≈ı∏Ìµµ ¡∂¿˝
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //æ∆¿Ã≈€ »πµÊ
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;

        itemCount = _count;

        itemImage.sprite = item.itemImage;

        if(item.itemType == Item.ItemType.Key)

        go_CountImage.SetActive(true);
        text_Count.text = itemCount.ToString();

        SetColor(1);

    }

    //æ∆¿Ã≈€ ∞πºˆ ¡∂¿˝
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    //ΩΩ∑‘ √ ±‚»≠
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        go_CountImage.SetActive(false);
        text_Count.text = "0";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null) 
            { 
                database.UseItem(item);
                if(item.itemType == Item.ItemType.HealPack)
                     SetSlotCount(-1);
                
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;
        }
        
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem( _tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
