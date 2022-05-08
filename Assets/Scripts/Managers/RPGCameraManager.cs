using UnityEngine;
using Cinemachine;

/// <summary>
/// Classe que gerencia a camera do jogo
/// </summary>
public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null; // usado para implementar design pattern singleton

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;


    private void Awake(){
        if(sharedInstance && sharedInstance != this){
            Destroy(gameObject);
        }else{
            sharedInstance = this;
        }
        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");
        DontDestroyOnLoad(vCamGameObject);
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstance && sharedInstance != this){
            Destroy(gameObject);
        }else{
            sharedInstance = this;
        }
        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");
        DontDestroyOnLoad(vCamGameObject);
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*Atribui a bounding box da layer floor ao confiner*/
    public static void SetConfiner(){
        GameObject.FindWithTag("Virtual Camera").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find("Grid/Layer_floor").GetComponent<Collider2D>();
    }
}
