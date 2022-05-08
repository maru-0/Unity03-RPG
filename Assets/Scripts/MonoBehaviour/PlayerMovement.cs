using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que descreve o movimento do player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 3.0f; // equivale ao momento (impulso) a ser dado ao player
    Vector2 movement = new Vector2();  // detectar movimento pelo teclado

    Animator animator; // guarda componente animation controller do player


    Rigidbody2D rb2D; // guarda componente Rigidbody do Player

 

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();   
    }

    private void FixedUpdate() {
        CharacterMove();
    }

    /*move o player
    */
    private void CharacterMove(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        rb2D.velocity = movement * movementSpeed; 
    }

    /*atualiza status da ação e da animação de andar
    */
    private void UpdateState(){
        if(Mathf.Approximately(movement.x, 0) && (Mathf.Approximately(movement.y, 0))){
            animator.SetBool("Walking", false);
        }else{
            animator.SetBool("Walking", true);
        }
        animator.SetFloat("DirX", movement.x);
        animator.SetFloat("DirY", movement.y);

    }
}
