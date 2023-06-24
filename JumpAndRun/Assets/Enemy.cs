using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed;
    public float rechts;
    public float links;
    private Vector3 rotation;

    /// <summary>
    /// Start() Diese Methode wird automatisch von Unity erstellt und wird immer ausgeführt, wenn man das Spiel startet
    /// </summary>
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        rechts = transform.position.x + rechts;
        links = transform.position.x - links;
        rotation = transform.eulerAngles;
    }
    /// <summary>
    /// Update() Wird automatisch von Unity erstellt und wird jedes Frame aufgerufen(also so gut wie immer) Sie bestimmt den Speed des Gegners und sorgt dafür, dass er sich umdreht
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < links)
        {
            transform.eulerAngles = rotation - new Vector3(0, 180, 0);
            
        }
        if (transform.position.x > rechts)
        {
            transform.eulerAngles = rotation;
            
        }
    }
}
