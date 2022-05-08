using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{


    public override void KillCharacter(){
        base.KillCharacter();
        GameObject.Find("PlayerO(Clone)").GetComponent<Player>().KillCharacter();
        SceneManager.LoadScene(3);
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
