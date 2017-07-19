using UnityEngine;
using System.Collections;

public class InstanciadorObstaculo1 : MonoBehaviour {

    public GameObject[] objetos;
    public float minCono = 1f;
    public float maxCono = 3f;
    public float minTramp = 4f;
    public float maxTramp = 5f;

    // Use this for initialization
    void Start()
    {
        Invoke("InstanciarCono", Random.Range(minCono, maxCono));
        Invoke("InstanciarTrampolin", Random.Range(minTramp, maxTramp));
    }

    void InstanciarCono()
    {
        //Instancia el objeto número 0
        Instantiate(objetos[0], transform.position, Quaternion.identity);
        //Invoco esta misma funcion cada X segundos
        Invoke("InstanciarCono",Random.Range(minCono, maxCono));

        //Instancia en un rango aleatorio para generar plataformas a diferentes distancias
        //Instantiate(objetos[Random.Range(0,objetos.Length)],transform.position,Quaternion.identity);
        //Invoco esta misma funcion cada rango aleatorio de segundos. Maximo y Minimo pasado por variable
        //Invoke("Instanciar",Random.Range(min,max));
    }

    void InstanciarTrampolin()
    {
        //Instancia el objeto número 1
        Instantiate(objetos[1], transform.position, Quaternion.identity);
        //Invoco la pared justo después del trampolín y la flecha de avisa delante
        InstanciarArrowUp();
        Invoke("InstanciarWall", 1f);
        //Invoco esta misma funcion cada X segundos
        Invoke("InstanciarTrampolin", Random.Range(minTramp, maxTramp));
    }

    void InstanciarArrowUp()
    {
        //Instancia el objeto número 2 antes del trampolin
        Instantiate(objetos[2], new Vector3(transform.position.x -5, -1, 0), Quaternion.identity);
    }

    void InstanciarWall()
    {
        //Instancia el objeto número 3
        Instantiate(objetos[3], new Vector3(transform.position.x, -1, 0), Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
