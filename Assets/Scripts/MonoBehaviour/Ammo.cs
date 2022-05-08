using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que representa munição
/// </summary>
public class Ammo : MonoBehaviour
{
    public int damageDealt; //dano causado pela munição


    private void OnTriggerEnter2D(Collider2D other) {
        if(other is BoxCollider2D){
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            StartCoroutine(enemy.CharacterDamage(damageDealt, 0.0f));
            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
