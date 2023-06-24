using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;



public class Player : MonoBehaviour
{
    //Wenn dem Objekt zugewiesene Variabeln public sind k�nnen sie nur noch �ber die grafische Oberfl�che ver�ndert werden (kp warum)
    public float        speed;
    public float        sprungHoehe;
    public GameObject   panel;
    public GameObject   spielerKamera;
    public Text         spielZeitText;
    public Text         anzahlVersucheText;
    public float        vergangeneZeit;
    public GameObject   herz1;
    public GameObject   herz2;
    public GameObject   herz3;

    private int         anzahlLeben = 3;
    private bool        isGrounded = false;
    private Rigidbody2D rb;
    private Text        spielEndeText;
    private Text        anzahlTodeText;
    private Animator    anim;
    private Vector3     rotation;
    private CoinManager cManager;
    private float       startTime;
    private string      dbName = "URI=file:ProjektLF6.db";
 
    /// <summary>
    /// Start() Diese Methode wird automatisch von Unity erstellt und wird immer ausgef�hrt, wenn man das Spiel startet
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        
        CreateDBTable();
        //Wird immer am Anfang ausgef�hrt, da man eine neue Runde startet
        WriteDB();

        startTime = Time.time;

        //Um Zugriff auf den TextSpielEnde und AnzahlVersuche zu erhalten: greife ich von dem Parent drauf zu.  GetChild(Zahl) die Zahl sagt hierbei aus welchen Index das jeweilige Element hat
        spielEndeText = panel.transform.GetChild(1).GetComponent<Text>();
        anzahlTodeText = panel.transform.GetChild(2).GetComponent<Text>();
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rotation = transform.eulerAngles;
        cManager = GameObject.FindGameObjectWithTag("Text").GetComponent<CoinManager>();

    }

    /// <summary>
    /// Update() Wird automatisch von Unity erstellt und wird jedes Frame aufgerufen(also so gut wie immer)
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        DisplayData();
        float richtung = Input.GetAxis("Horizontal");

        //der Spieler soll sterben, sobald er herunterf�llt. dazu �berpr�fe ich seine Y Koordinaten und zerst�re ihn, sollte er zu niedrig sein
        if (gameObject.transform.position.y < -6)
        {
            while (anzahlLeben >= 0)
            {
                LebenVerlieren();
                //LebenVerlieren();
                //LebenVerlieren();
            }
        }

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

        //�berpr�fen ob der Spieler springen darf: Wenn er in der Luft ist bzw (IsJumping) soll er nicht nochmal spirngen d�rfen 
        if(isGrounded == false)
        {
            anim.SetBool("IsJumping", true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        //Sprung hinzuf�gen
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //Forcemode Impulse sorgt f�r eine Art Fade Bewegung (macht den Sprung nicht statisch)
            rb.AddForce(Vector2.up * sprungHoehe, ForceMode2D.Impulse); 
            isGrounded = false; 
        }
        //Kamera und herzen sollen sich mit dem Spieler 
        spielerKamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
       

        //Beim Start weise ich der Variable StartTime die aktuelle Zeit zu. Um die Zeit im Spiel zu tracken nehme ich die aktuelle zeit - die startzeit. ToString("F2") bewirkt hierbei, dass die Zeit in Milliesekunden angegeben wird
        vergangeneZeit = Time.time - startTime;
        spielZeitText.text = vergangeneZeit.ToString("F2");

        //Die Herzen bzw Leben sollen dem Spieler folgen, dies wird hier festgelegt. Es wird jedes mal �berpr�ft ob die Herzen noch existieren, weil sonst ein nicht vorhandenes Objekt angesprochen wird
        if (!Object.ReferenceEquals(herz1, null))
        { 
            herz1.transform.position = new Vector3(transform.position.x - 8, transform.position.y + 4, 0);
        }
        if (!Object.ReferenceEquals(herz2, null))
        {
            herz2.transform.position = new Vector3(transform.position.x - 7, transform.position.y + 4, 0);
        }
        if (!Object.ReferenceEquals(herz3, null))
        {
            herz3.transform.position = new Vector3(transform.position.x - 6, transform.position.y + 4, 0);
        }

        
    }

    /// <summary>
    /// Lebenverlieren() soll ausgef�hrt werden wenn der Spieler mit einem Spike oder einem Gegner in Ber�hrung kommt dies soll jedes mal ein Herz zerst�ren, da diese einzelne Objekte sind m�ssen sie in der dortigen Reihenfolge zerst�rt werden
    /// </summary>
    void LebenVerlieren()
    {
        anzahlLeben--;

        if (anzahlLeben == 2)
        {
            Destroy(herz3);
        }
        else if (anzahlLeben == 1)
        {
            Destroy(herz2);
        }
        else if (anzahlLeben == 0)
        {
            Destroy(herz1);
            panel.gameObject.SetActive(true);

            spielEndeText.text = "Game Over";
            Destroy(gameObject);
            //DBAktualisieren();

        }
    }

    /// <summary>
    /// OnCollisionEnter2() Diese Methode kann man hinzuf�gen, nachdem man einem Objekt eine Hitbox in Unity zugewiesen hat, sie wird ausgef�hrt wenn man ein Objekt mit Hitbox ber�hrt
    /// </summary>
    /// <param name="collision"> Input je nachdem ob man einen Enemy ber�hrt oder den ground</param>
    //Pr�fen ob der Spieler den Boden ber�hrt hat. Wenn das wahr ist wird is_grounded true und der Spieler kann wieder springen
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            LebenVerlieren();
        }
    }

    /// <summary>
    /// OnTriggerEnter2D �hnlich wie die obere Methode. Sie wird ausgef�hrt wenn man entweder einen Coin einen Spike oder Finish ber�hrt. So sol bei der BEr�hrung einer M�nze diese zerst�rt werden bei einem Spike soll der Spieler schaden bekommen und wenn man Finish ber�hrt soll man zerst�rt werden und das Panel soll aktiviert werden
    /// </summary>
    /// <param name="other">Input je nachdem was man ber�hrt Coin, Spike oder Finish</param>
    // Bestimmt was passieren soll, wenn man ein Objekt ber�hrt
    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.gameObject.tag == "Coin") 
        {
            cManager.Addmoney();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Spike")
        {
            LebenVerlieren();
        }

        if(other.gameObject.tag == "Finish")
        {

            
            panel.gameObject.SetActive(true);
            
            spielEndeText.text = "Gewonnen";
            Destroy(gameObject);
            //DBAktualisieren();
        }

    }


    /// <summary>
    /// CreateDB() hat die Aufgabe einen Datenbanktable zu erstellen, sollte dieser nicht existieren. Er soll die Felder Versuche, BestZeit und Zeit beinhalten
    /// </summary>
    public void CreateDBTable()
    {
        //neue DatenBankverbindung erstellen
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            //command erm�glicht die Kontrolle �ber die DB
            using (var command = connection.CreateCommand())
            {
                //erstellt einen Table

                command.CommandText = "CREATE TABLE IF NOT EXISTS Spieler (Versuche INTEGER, BestZeit VARCHAR(30));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }


    /// <summary>
    /// DisplayData() hat die Aufgabe bestimmte Daten aus der Datenbank einem Textfeld zuzuweisen (bis jetzt nur die Anzahl der Versuche)
    /// </summary>
    public void DisplayData()
    {
        anzahlVersucheText.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Spieler LIMIT 1;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())

                        anzahlVersucheText.text = $"Anzahl Versuche: {reader["Versuche"].ToString()} \n {reader["Versuche"].ToString()}";

                    reader.Close();
                }
            }

            connection.Close();
        }
    }


    /// <summary>
    /// WriteDB() hat die Aufgabe die Datenbankfelder zu aktualisieren. Immer wenn sie ausgef�hrt wird, wird werden die Versuche um 1 erh�ht
    /// </summary>
    public void WriteDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {

                //Hier werden die Werte der Datenbank aktualisiert           
                command.CommandText = "UPDATE Spieler SET Versuche = Versuche + 1;";
                command.ExecuteNonQuery();

            }
            connection.Close();
        }
    }
    /// Die Methode sollte eigentlich einen weiteren Datenbankzugriff erstellen. Dies hat leider nicht geklappt, da die Zeit zu knapp wurde :(

    //public void DBAktualisieren()
    //{
    //    using (var connection = new SqliteConnection(dbName))
    //    {
    //        connection.Open();
    //        using (var command = connection.CreateCommand())
    //        {
    //            // SELECT-Abfrage ausf�hren, um den aktuellen Wert abzurufen
    //            command.CommandText = "SELECT BestZeit FROM Spieler LIMIT 1;";

    //            using (var reader = command.ExecuteReader())
    //            {
    //                float alterWert = 0;
    //                if (reader.Read())
    //                {
    //                    alterWert = 0;
    //                    alterWert = reader.GetFloat(0);
    //                    //double i = (double)alterWert;
                        
    //                }

    //                reader.Close();

    //                if (vergangeneZeit < alterWert)
    //                {
    //                    // UPDATE-Abfrage ausf�hren, um den Wert in der Datenbank zu �ndern (nur wenn deineVariable gr��er ist)
    //                    command.CommandText = $"UPDATE Spieler SET BestZeit = {vergangeneZeit}, Versuche = Versuche + 1 LIMIT 1;";
                        
    //                }
    //                else
    //                {
    //                    command.CommandText = $"UPDATE Spieler SET Versuche = Versuche + 1 LIMIT 1;";
    //                }
    //                command.ExecuteNonQuery();

    //                connection.Close();
    //            }
    //        }
    //    }
  
    //}

}
