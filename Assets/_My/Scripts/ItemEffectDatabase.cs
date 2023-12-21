using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
 public class ItemEffect
{
    public string itemName; // 아이템의 이름 (키값)
    [Tooltip("HP, Pollution")]
    public string[] part; // 부위
    public int[] num; // 수치
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
                                Debug.Log("잘못된 Status 부위");

                                break;
                        }
                        playerManager.infection = false;
                        Debug.Log(_item.itemName + " 을 사용했습니다.");
                    }

                    return;
                }


            }

            Debug.Log("ItemEffectDatalbase에 일치하는 itemName이 없음");

        }

    }
}
