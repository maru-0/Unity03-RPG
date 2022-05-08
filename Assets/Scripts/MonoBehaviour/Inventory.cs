using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que representa o inventario
/// </summary>
public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab; //prefab do slot de inventario
    public const int slotNum = 5; // quantidade de slots no inventário

    Image[] itemImages = new Image[slotNum]; // imagem dos itens no inventário
    Item[] items = new Item[slotNum]; // itens no inventário
    GameObject[] slots = new GameObject[slotNum]; //slots do inventário 
      
    // Start is called before the first frame update
    void Start()
    {
        CreateSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* cria um novo slot de inventário
    */
    public void CreateSlot(){
        if(slotPrefab){
            for(int i = 0; i < slotNum; i++){
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    /*adiciona item coletado ao inventário
    */
    public bool AddItem(Item itemToAdd){
        for(int i = 0; i < items.Length; i++){
            if(items[i] && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable){
                items[i].quantity++;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.quantityText;
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();

                if(items[i].quantity == 30){
                    RPGGameManager.coinComplete = true;
                }

                return true;
            }else if(!items[i]){
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;

                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.quantityText;
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();
                return true;
            }

        }
        return false;
    }
}
