using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using MySql.Data.MySqlClient;

public class Player : MonoBehaviour
{
    //Wenn dem Objekt zugewiesene Variabeln public sind können sie nur noch über die grafische Oberfläche verändert werden (kp warum)
    public float speed = 5;
    public float sprung_hoehe = 8;
    private Rigidbody2D rb;
    private bool is_grounded = false;

    private Animator anim;
    private Vector3 rotation;

    private CoinManager C_Manager;
    //private PanelManager P_Manager;

    public GameObject panel;
    public Text panelText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.eulerAngles;
        C_Manager = GameObject.FindGameObjectWithTag("Text").GetComponent<CoinManager>();
        //P_Manager = GameObject.FindGameObjectWithTag("Text").GetComponent<PanelManager>();

            
    }

    // Update is called once per frame
    void Update()
    {
        float richtung = Input.GetAxis("Horizontal");

        //Laufanimation oder Stehanimation verwenden
        if(richtung != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        //Bewegungsrichtung bestimmen (Player soll sich umdrehen, sobald er in eine andere Richtung schaut)
        if(richtung < 0)
        {
            
            transform.eulerAngles = rotation - new Vector3(0, 180, 0);
            transform.Translate(Vector2.right * speed * -richtung * Time.deltaTime);
        }
        else if(richtung > 0)
        {
            transform.eulerAngles = rotation;
            transform.Translate(Vector2.right * speed * richtung * Time.deltaTime);
        }

        if(is_grounded == false)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        //Sprung hinzufügen
        if (Input.GetKeyDown(KeyCode.Space) && is_grounded)
        {
            rb.AddForce(Vector2.up * sprung_hoehe, ForceMode2D.Impulse); //Forcemode Impulse sorgt für eine Art Fade Bewegung (macht den Sprung nicht statisch)
            is_grounded = false; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //Prüfen ob der Spieler den Boden berührt hat. Wenn das wahr ist wird is_grounded true und der Spieler kann wieder springen
    {
        if(collision.gameObject.tag == "ground")
        {
            is_grounded = true;
        }
    }

    // Bestimmt was passieren soll, wenn man ein Objekt berührt
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Coin") 
        {
            C_Manager.Addmoney();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Spike") 
        {
            panel.SetActive(true);
            panelText.text = "Game Over";           
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Finish")
        {
            panel.SetActive(true);
            panelText.text = "Gewonnen";            
            Destroy(gameObject);
        }

    }


}
