using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que representa a trajetoria do projetil
/// </summary>
public class Arc : MonoBehaviour
{
    /*descreve o arco com um alvo e uma duração
    */
    public IEnumerator TrajectoryArc(Vector3 target, float duration){
        var initialPosition = transform.position;
        var completePercent = 0.0f;
        while(completePercent < 1.0f){
            completePercent += Time.deltaTime / duration;
            var currentHeight = Mathf.Sin(Mathf.PI * completePercent);
            transform.position = Vector3.Lerp(initialPosition, target, completePercent) + Vector3.up * currentHeight;
            completePercent += Time.deltaTime / duration;
            yield return null;
        }
        gameObject.SetActive(false);
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
