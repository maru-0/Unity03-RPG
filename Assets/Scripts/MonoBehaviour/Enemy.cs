using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe que representa o inimigo
/// </summary>
public class Enemy : Character
{
    public int healthPoints; // pontos de vida
    public int attackValue; // dano que o ataque/encostar no inimigo causa

    Coroutine damageCoroutine; // co rotina usada para aplicar dano 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable(){
        ResetCharacter();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Player player = other.gameObject.GetComponent<Player>();
            if(damageCoroutine == null){
                damageCoroutine = StartCoroutine(player.CharacterDamage(attackValue, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(damageCoroutine != null){
                StopCoroutine(damageCoroutine);
                damageCoroutine = null; 
            }
        }    
    }

    /*recebe dano
    */
    public override IEnumerator CharacterDamage(int damage, float interval)
    {
        while(true){
            StartCoroutine(CharacterFlicker());
            healthPoints -= damage;
            
            if(healthPoints <= 0){
                if(SceneManager.GetActiveScene().name.Equals("03")){
                    RPGGameManager.questProgress++;
                }
                
                if(!RPGGameManager.questComplete && RPGGameManager.questProgress == 5){
                    RPGGameManager.questComplete = true;
                    GameObject.Find("NPCO").GetComponent<NPC>().textNPC.text = "Obrigado!";
                    GameObject.Find("NPCO").GetComponent<NPC>().potionO.SetActive(true);
                }

                KillCharacter();
                break;
            }
            if(interval <= float.Epsilon){
                yield return new WaitForSeconds(interval);
            }else{
                break;
            }
        }
    }

    /*dá reset no inimigo, deixando com a quantidade inicial de vida
    */
    public override void ResetCharacter()
    {
        healthPoints = initialHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
