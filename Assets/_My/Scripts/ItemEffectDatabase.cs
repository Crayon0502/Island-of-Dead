using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
 public class ItemEffect
{
    public string itemName; // �������� �̸� (Ű��)
    [Tooltip("HP, Pollution")]
    public string[] part; // ����
    public int[] num; // ��ġ
}



public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    private const string HP = "HP", POLLUTION = "POLLUTION";

    [SerializeField]
    private PlayerManager playerManager;

    public void UseItem(Item _item)
    {

        if (_item.itemType == Item.ItemType.Ammo)
        {
            Debug.Log(_item.itemName);
        }

        else if (_item.itemType == Item.ItemType.HealPack)
        {


            for (int x = 0; x < itemEffects.Length; x++)
            {


                if (itemEffects[x].itemName == _item.itemName)
                {
                    
                    
                    for (int y = 0; y< itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP :
                                playerManager.playerCurrentHP += itemEffects[x].num[y];

                                break;

                            case POLLUTION :
                                playerManager.playerCurrentInfection -= itemEffects[x].num[y];

                                break;
                            default:
                                Debug.Log("�߸��� Status ����");

                                break;
                        }
                        playerManager.infection = false;
                        Debug.Log(_item.itemName + " �� ����߽��ϴ�.");
                    }

                    return;
                }


            }

            Debug.Log("ItemEffectDatalbase�� ��ġ�ϴ� itemName�� ����");

        }

    }
}
