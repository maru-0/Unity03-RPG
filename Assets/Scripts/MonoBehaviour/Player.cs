using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Classe que representa o player
/// </summary>
public class Player : Character
{
    public Inventory inventoryPrefab; // prefab do inventario
    Inventory inventory; //inventaio do player
    public HealthBar healthBarPrefab; // prefab da barra de vida
    HealthBar healthBar; // barra de vida do player

    private bool rubyCollected = false; //indica se o ruby já foi coletado
    private bool sapphireCollected = false; //indica se o sapphire já foi coletado
    private bool emeraldCollected = false; //indica se o emerald já foi coletado
    private bool diamondCollected = false; //indica se o diamond já foi coletado
    private bool warpActivated = false;   //indica se o warp ativado pelas gemas já foi ativado

    public HealthPoints healthPoints; // pontos de vida


    /*soma uma quantidade aos pontos de vida atuais
    */
    public void AdjustHealthPoints(int quantity){
        healthPoints.value += quantity;

        if(healthPoints.value < 0){
            healthPoints.value = 0;
        }else{
            healthPoints.value = healthPoints.value > maxHealthPoints ? maxHealthPoints: healthPoints.value;
        }
    }

    private void Start(){
        inventory = Instantiate(inventoryPrefab);
        healthPoints.value = initialHealthPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.player = this;

        DontDestroyOnLoad(inventory);
        DontDestroyOnLoad(healthBar);
    }

    /* dá dano no player no decorrer de um intervalo
    */
    public override IEnumerator CharacterDamage(int damage, float interval)
    {
        while(true){
            StartCoroutine(CharacterFlicker());
            healthPoints.value -= damage;
            if(healthPoints.value <= 0){
                KillCharacter();
                break;
            }
            if(interval > float.Epsilon){
                yield return new WaitForSeconds(interval);
            }else{
                break;
            }
        }
    }

    /*destroy player quando ele é derrotado
    */
    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
        SceneManager.LoadScene(5);
        
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.player = this;
        healthPoints.value = initialHealthPoints;
        
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        //colisão com área do npc
        if(collision.gameObject.CompareTag("NPC")){
            collision.gameObject.GetComponent<NPC>().textNPC.enabled = true;
            return;
        }
        //colisão com warp
        print("Colidi tag "+collision.tag);
        if(collision.gameObject.CompareTag("Warp")){
            RPGGameManager.ChangeStage(collision.gameObject.GetComponent<Warp>().stageToWarp);
            return;
        }
        //colisão com coletáveis
        if(collision.gameObject.CompareTag("Collectable")){
            Item hit = collision.gameObject.GetComponent<Consumable>().item;
            bool shouldCollect = false;
            if(hit){
                print(hit.objName + " get!");

                switch(hit.itemType){
                    case Item.ItemType.COIN:
                        shouldCollect = inventory.AddItem(hit);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldCollect = healthPoints.value < maxHealthPoints;
                        if(shouldCollect){
                            AdjustHealthPoints(hit.quantity);
                        }
                        break;
                    case Item.ItemType.RUBY:
                        shouldCollect = inventory.AddItem(hit);
                        rubyCollected = true;
                        break;
                    case Item.ItemType.SAPPHIRE:
                        shouldCollect = inventory.AddItem(hit);
                        sapphireCollected = true;
                        break;
                    case Item.ItemType.EMERALD:
                        shouldCollect = inventory.AddItem(hit);
                        emeraldCollected = true;
                        break;
                    case Item.ItemType.DIAMOND:
                        shouldCollect = inventory.AddItem(hit);
                        diamondCollected = true;
                        break;
                    case Item.ItemType.POTION:
                        shouldCollect = true;
                        RPGGameManager.weaponUpgraded = true;
                        gameObject.GetComponent<Weapons>().UpgradeWeapon();
                        break;
                    default:
                        break;
                }
            }
            if(shouldCollect){
                collision.gameObject.SetActive(false);
            }
            if(!warpActivated && rubyCollected && sapphireCollected && emeraldCollected && diamondCollected ){
                GameObject.Find("WarpO").GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.CompareTag("NPC")){
            collision.gameObject.GetComponent<NPC>().textNPC.enabled = false;
            return;
        }
    }
}
