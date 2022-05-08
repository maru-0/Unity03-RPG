using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que representa personagens
/// </summary>
public abstract class Character : MonoBehaviour
{

    
    public int maxHealthPoints; //quantidade maxima de vida
    public int initialHealthPoints; // quantidade inicial

    /* Reset do personagem ao seu estado inicial
    */
    public abstract void ResetCharacter();

    /* Efeito de flicker no sprite, usado quando dano é recebido
    */
    public virtual IEnumerator CharacterFlicker(){
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    /*aplica dano
    */
    public abstract IEnumerator CharacterDamage(int damage, float interval);

    /* destroi game object do personagem quando derrotado
    */
    public virtual void KillCharacter(){
        Destroy(gameObject);
    }

 
}
