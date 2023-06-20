using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelManager : MonoBehaviour
{
    public Text panel_text;
    private string x;
    
    // Start is called before the first frame update
    void Start()
    {
        x = "f";
    }

    // Update is called once per frame
    void Update()
    {
        panel_text.text = x;
    }
    public void TextAktualisieren(string ereignis)
    {
        x = ereignis;
    }
}
