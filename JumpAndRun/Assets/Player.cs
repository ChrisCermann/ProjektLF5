using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using MySql.Data.MySqlClient;

public class Player : MonoBehaviour
{
    //Wenn dem Objekt zugewiesene Variabeln public sind können sie nur noch über die grafische Oberfläche verändert werden (kp warum)
    public float speed;
    public float sprungHoehe;
    public GameObject panel;
    public GameObject spielerKamera;

    private int anzahlLeben = 3;
    private int anzahlTode;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Text spielEndeText;
    private Text anzahlTodeText;
    private Animator anim;
    private Vector3 rotation;
    private CoinManager cManager;

    void LebeneVerlieren()
    {
        anzahlLeben--;
        anzahlTode++;
        if (anzahlLeben == 0)
        {
            panel.gameObject.SetActive(true);
            anzahlTodeText.text = $"Du bist {anzahlTode} mal gestorben";
            spielEndeText.text = "Game Over";
            Destroy(gameObject);

        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
        spielEndeText = panel.transform.GetChild(1).GetComponent<Text>();
        //test
        anzahlTodeText = panel.transform.GetChild(2).GetComponent<Text>();
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.eulerAngles;
        cManager = GameObject.FindGameObjectWithTag("Text").GetComponent<CoinManager>();
        

            
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

        if(isGrounded == false)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        //Sprung hinzufügen
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //Forcemode Impulse sorgt für eine Art Fade Bewegung (macht den Sprung nicht statisch)
            rb.AddForce(Vector2.up * sprungHoehe, ForceMode2D.Impulse); 
            isGrounded = false; 
        }

        spielerKamera.transform.position = new Vector3(transform.position.x, 0, -10);
    }

    //Prüfen ob der Spieler den Boden berührt hat. Wenn das wahr ist wird is_grounded true und der Spieler kann wieder springen
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            LebeneVerlieren();
        }
    }

    // Bestimmt was passieren soll, wenn man ein Objekt berührt
    private void OnTriggerEnter2D(Collider2D other) 
    {
        

        if (other.gameObject.tag == "Coin") 
        {
            cManager.Addmoney();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Spike")
        {
            LebeneVerlieren();
        }

        if(other.gameObject.tag == "Finish")
        {
            panel.gameObject.SetActive(true);
            anzahlTodeText.text = $"Du bist {anzahlTode} mal gestorben";
            spielEndeText.text = "Gewonnen";
            Destroy(gameObject);
        }

    }

    
}
