using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que representa ponto de spawn
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    public GameObject spawnPrefab; //prefab a ser spawnado
    public float repetitionInterval; //intervalo de repetição do spawn
    // Start is called before the first frame update
    public void Start()
    {
        if(repetitionInterval > 0){
            InvokeRepeating("SpawnO", 0.0f, repetitionInterval);
        }
    }

    public GameObject SpawnO(){
        if(spawnPrefab){
            return Instantiate(spawnPrefab, transform.position, Quaternion.identity);
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
