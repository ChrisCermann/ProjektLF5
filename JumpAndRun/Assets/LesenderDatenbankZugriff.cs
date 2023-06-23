//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
//using MySqlConnector;

//class LesenderDatenbankzugriff

//{
//    static void Main(string[] args)
//    {

//        //Datenbankverbindung();

//    }
//    public void Datenbankverbindung()
//    {

//        string connectionString = "Server=localhost;database=gamedatenbank;Uid=root;password=passwort123;";
//        MySqlConnection connection = new MySqlConnection(connectionString);


//        connection.Open();


//        string sqlQuery = "SELECT * FROM benutzer;";


//        MySqlCommand command = new MySqlCommand(sqlQuery, connection);

//        ;
//        MySqlDataReader reader = command.ExecuteReader();


//        while (reader.Read())
//        {
//            // Daten lesen und verarbeiten
//            // Beispiel: Ausgabe des Wertes in der Spalte "columnname"
//            Console.WriteLine(reader["leben"]);
//            Console.ReadKey();
//        }

//        reader.Close();


//    }

//}




