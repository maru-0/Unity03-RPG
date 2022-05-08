using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe que gerencia a lógica do jogo
/// </summary>
public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager instanciaCompartilhada = null;// usado para implementar design pattern singleton

    public RPGCameraManager cameraManager; //camera manager do jogo

    SpawnPoint playerSpawnPoint; //ponto de spawn do player

    public static bool sceneChanged = false; //flag que indica se a cena mudou
    
    public static bool coinComplete = false; //flag que indica se todas as moedas foram coletadas
    public static bool questComplete = false; //flag que indica se a quest do npc foi cumprida
    public static int  questProgress = 0;     //quantidade de monstros já derrotadas para a quest
    public static bool weaponUpgraded = false; //flag que indica se houve upgrade na arma


    private void Awake(){
        // print("Awake manager");
        // if(instanciaCompartilhada && instanciaCompartilhada != this){
        //     Destroy(gameObject);
        // }else{
        //     instanciaCompartilhada = this;
        //     DontDestroyOnLoad(gameObject);
        //     DontDestroyOnLoad(cameraManager);
        //     DontDestroyOnLoad(GameObject.Find("Main Camera"));
        // }
    }
    // Start is called before the first frame update
    void Start()
    {
        print("Awake manager");
        if(instanciaCompartilhada && instanciaCompartilhada != this){
            Destroy(gameObject);
        }else{
            instanciaCompartilhada = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(cameraManager);
            DontDestroyOnLoad(GameObject.Find("Main Camera"));
        }
        
        print("Start manager");
        playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").GetComponent<SpawnPoint>();
        SetupScene(); 
    }

    /* Metodo de setup da cena que dá spawn no player
    */
    public void SetupScene(){
        SpawnPlayer();
    }

    /*Faz spawn do player e faz com que a camera o siga
    */
    public void SpawnPlayer(){
        if(playerSpawnPoint){
            GameObject player = playerSpawnPoint.SpawnO();
            cameraManager.virtualCamera.Follow = player.transform;
            DontDestroyOnLoad(player);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneChanged){
            if(GameObject.FindWithTag("Virtual Camera").GetComponent<CinemachineConfiner>().m_BoundingShape2D == null){
                RPGCameraManager.SetConfiner();
                GameObject.Find("PlayerO(Clone)").transform.position = GameObject.Find("PlayerSpawnPoint").transform.position;
                sceneChanged = false;
            }
       }
    }

    /*muda de fase indo pra cena correspondente
    */
    public static void ChangeStage(int stage){
        SceneManager.LoadScene(stage-1);
        
        RPGGameManager.sceneChanged = true;
    }
    
}
