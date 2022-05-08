using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]


/// <summary>
/// Classe que representa o comportamento de "perambular" do inimigo
/// </summary>
public class WalkAround : MonoBehaviour
{
    public float chasingSpeed; //velocidade quando perseguindo o player
    public float walkingSpeed; // velocidade quando perambulando
    float currentSpeed; // velocidade atual

    public float directionChangeInterval; // tempo de duração da mudança de direção
    public bool chasingPlayer; // flag que indica se está perseguindo player

    Coroutine moveCoroutine; // co rotina de movimentação

    Rigidbody2D rb2D; // armazena rigid body 2d
    Animator animator; // armazena animator

    Transform targetTransform = null; // usado para implementar design pattern singleton

    Vector3 finalPosition; //posição final após movimentação

    float currentAngle = 0; //angulo da movimentação

    CircleCollider2D circleCollider; // guarda o circle collider que representa a visão do inimigo
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentSpeed = walkingSpeed;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(WalkingRoutine());
        circleCollider = GetComponent<CircleCollider2D>();  
    }

    private void OnDrawGizmos() {
        if(circleCollider != null){
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
        
    }

    /*rotina que faz andar em uma direção
    */
    public IEnumerator WalkingRoutine(){
        while(true){
            ChooseNewFinalPosition();
            if(moveCoroutine != null){
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /*define uma direção e uma distância como nova posição final
    */
    void ChooseNewFinalPosition(){
        currentAngle += Random.Range(0,360);
        currentAngle = Mathf.Repeat(currentAngle, 360);
        finalPosition += AngleToVector3(currentAngle);
    }

    /*executa o movimento
    */
    public IEnumerator Move(Rigidbody2D rbToMove, float speed){
        float remainingDistance = (transform.position - finalPosition).sqrMagnitude;
        while(remainingDistance > float.Epsilon){
            if(targetTransform != null){
                finalPosition = targetTransform.position;
            }
            if(rbToMove != null){
                animator.SetBool("Walking", true);
                Vector3 newPosition = Vector3.MoveTowards(rbToMove.position, finalPosition, speed*Time.deltaTime);
                rb2D.MovePosition(newPosition);
                remainingDistance = (transform.position - finalPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("Walking", false);
    }

    /*converte angulo em grau para vetor 
    */
    Vector3 AngleToVector3(float entryAngle){
        float entryAngleRadians = entryAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(entryAngleRadians), Mathf.Sin(entryAngleRadians), 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && chasingPlayer){
            currentSpeed = chasingSpeed;
            targetTransform = other.gameObject.transform;
            if(moveCoroutine != null){
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(Move(rb2D, currentSpeed));
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            animator.SetBool("Walking", false);
            currentSpeed = walkingSpeed;
            if(moveCoroutine != null){
                StopCoroutine(moveCoroutine);
            }
            targetTransform = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(rb2D.position, finalPosition, Color.red);
    }
}
