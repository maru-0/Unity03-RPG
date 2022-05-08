using UnityEngine;

[CreateAssetMenu(fileName = "Item")]

/// <summary>
/// Classe que representa os itens
/// </summary>
public class Item : ScriptableObject {
    public string objName; // nome do item
    public Sprite sprite; // sprite do item
    public int quantity; // quantidade vinculada ao item
    public bool stackable; // propriedade de "empilhavel"

    // enum tipos de item
    public enum ItemType{
        COIN,
        HEALTH,
        RUBY,
        SAPPHIRE,
        EMERALD,
        DIAMOND,
        POTION
    }

    public ItemType itemType; // tipo do item
    
}
