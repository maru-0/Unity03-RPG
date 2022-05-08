using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

/// <summary>
/// Classe que representa a arma do player
/// </summary>
public class Weapons : MonoBehaviour
{
    public GameObject ammoPrefab; // prefab da munição
    static List<GameObject> ammoPool; // pool de munição
    public int poolSize; // tamanho do pool
    public float weaponSpeed; // velocidade do projetil para acertar o alvo

    bool shooting; // flag do estado "atirando"
    [HideInInspector]
    public Animator animator; // guarda o Animator

    Camera localCamera; // guarda a camera local

    float positiveSlope; //slope positivo usado para definir o quadrando do tiro
    float negativeSlope; //slope negativo usado para definir o quadrando do tiro

    public Sprite initialSprite;
    public Sprite upgradeSprite;

    //enum de quadrantes de direção de tiro
    enum Quadrant{
        East,
        South,
        West,
        North
    }

    private void Start() {
        animator = GetComponent<Animator>();
        shooting = false;
        localCamera = Camera.main;
        Vector2 downLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 downRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        Vector2 upLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 upRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        positiveSlope = GetSlope(downLeft, upRight);
        negativeSlope = GetSlope(upLeft, downRight);
    }

    /* verifica se está acima ou abaixo do slope positivo
    */
    bool AbovePositiveSlope(Vector2 pos){
        Vector2 playerPos = gameObject.transform.position;
        Vector2 mousePos = localCamera.ScreenToWorldPoint(pos);
        float yIntersection = playerPos.y - (positiveSlope*playerPos.x);
        float inputIntersection = mousePos.y - (positiveSlope*mousePos.x);

        return inputIntersection > yIntersection;
    }

    /* verifica se está acima ou abaixo do slope negativo
    */
    bool AboveNegativeSlope(Vector2 pos){
        Vector2 playerPos = gameObject.transform.position;
        Vector2 mousePos = localCamera.ScreenToWorldPoint(pos);
        float yIntersection = playerPos.y - (negativeSlope*playerPos.x);
        float inputIntersection = mousePos.y - (negativeSlope*mousePos.x);

        return inputIntersection > yIntersection;
    }

    /*usa os slopes para identificar em qual quadrante o tiro ocorreu
    */
    Quadrant GetQuadrant(){
        Vector2 playerPos = transform.position;
        Vector2 mousePos = Input.mousePosition;

        bool aboveNegativeSlope = AboveNegativeSlope(mousePos);
        bool abovePositiveSlope = AbovePositiveSlope(mousePos);

        if(abovePositiveSlope){
            if(aboveNegativeSlope){
                return Quadrant.North;
            }else{
                return Quadrant.West;
            }
        }else{
            if(aboveNegativeSlope){
                return Quadrant.East;
            }else{
                return Quadrant.South;
            }
        }

    }


    private void Awake() {
        if(ammoPool == null){
            ammoPool = new List<GameObject>();
        }

        ammoPrefab.GetComponent<Ammo>().damageDealt = 1;
        ammoPrefab.GetComponent<SpriteRenderer>().sprite = initialSprite;
        ammoPrefab.GetComponent<CircleCollider2D>().radius = 0.22f;

        for(int i = 0; i < poolSize; i++){
            GameObject ammoO = Instantiate(ammoPrefab);
            DontDestroyOnLoad(ammoO);
            ammoO.SetActive(false);
            ammoPool.Add(ammoO);
        }

    }
  

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            shooting = true;
            ShootAmmo();
        }
        UpdateState();
    }

    float GetSlope(Vector2 p1, Vector2 p2){
        return (p2.y - p1.y)/(p2.x - p1.x);
    }

    /* atualiza status da ação e da animação de atirar
    */
    void UpdateState(){
        if(shooting){
            Vector2 quadrantVector;
            Quadrant quadrantEnum = GetQuadrant();

            switch(quadrantEnum){
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }
            animator.SetBool("Shooting", true);
            animator.SetFloat("ShootX", quadrantVector.x);
            animator.SetFloat("ShootY", quadrantVector.y);
            shooting = false;
        }else{
            animator.SetBool("Shooting", false);
        }
    }

    /*faz spawn da munição
    */
    GameObject SpawnAmmo(Vector3 pos){
        foreach(GameObject ammo in ammoPool){
            if(ammo.activeSelf == false){
                ammo.SetActive(true);
                ammo.transform.position = pos;
                return ammo;
            }
        }
        return null;
    }

    /*dispara a munição
    */
    void ShootAmmo(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject ammo = SpawnAmmo(transform.position);
        if(ammo != null){
            Arc arcScript = ammo.GetComponent<Arc>();
            float trajectoryDuration = 1.0f/weaponSpeed;
            StartCoroutine(arcScript.TrajectoryArc(mousePosition, trajectoryDuration));
        }
    }

    private void OnDestroy() {
        ammoPool = null;
    }

    /*faz upgrade da arma para bola de fogo
    */
    public void UpgradeWeapon(){


        foreach(GameObject ammo in ammoPool){
            Destroy(ammo);
        }


        ammoPrefab.GetComponent<Ammo>().damageDealt = 2;
        ammoPrefab.GetComponent<SpriteRenderer>().sprite = upgradeSprite;
        ammoPrefab.GetComponent<CircleCollider2D>().radius = 0.42f;

        ammoPool = new List<GameObject>();

        for(int i = 0; i < poolSize; i++){
            GameObject ammoO = Instantiate(ammoPrefab);
            DontDestroyOnLoad(ammoO);
            ammoO.SetActive(false);
            ammoPool.Add(ammoO);
        }

        
    }
}
