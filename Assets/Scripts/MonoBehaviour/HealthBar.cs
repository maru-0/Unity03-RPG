using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que representa a barra de vida do player
/// </summary>
public class HealthBar : MonoBehaviour
{
    public HealthPoints healthPoints; // pontos de vida
    public Player player; // player, vinculado com a barra
    public Image meterImage; // barra da health bar que varia conforme a quantidade de vida
    public Text hpText;   //texto que mostra a vida de forma numérica
    int maxHealthPoints;//quantidade maxima de vida

    // Start is called before the first frame update
    void Start()
    {
        maxHealthPoints = player.maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(player){
            meterImage.fillAmount = (float)healthPoints.value/(float)maxHealthPoints;
            hpText.text = "HP: " + healthPoints.value;
        }
    }
}
