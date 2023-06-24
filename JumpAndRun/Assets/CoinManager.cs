using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int money;
    public Text money_text;

    
    /// <summary>
    /// wei�t money dem dazugeh�rigen Text zu
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        money_text.text = money.ToString();
    }

    /// <summary>
    /// AddMoney() wird in Player ausgef�hrt wenn man eine M�nze ber�hrt und soll die Variable Money erh�hen
    /// </summary>
    public void Addmoney()
    {
        money++;
    }
}
