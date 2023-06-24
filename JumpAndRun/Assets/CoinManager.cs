using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int money;
    public Text money_text;

    
    /// <summary>
    /// weißt money dem dazugehörigen Text zu
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        money_text.text = money.ToString();
    }

    /// <summary>
    /// AddMoney() wird in Player ausgeführt wenn man eine Münze berührt und soll die Variable Money erhöhen
    /// </summary>
    public void Addmoney()
    {
        money++;
    }
}
