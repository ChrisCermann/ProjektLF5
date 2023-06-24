using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    /// <summary>
    /// Restart() Wird ausgeführt, wenn der User den Restart Button klickt. Hat die Aufgabe das Spiel neu zu starten
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("JumpAndRunScene");
    }
}
