using UnityEngine;
using System.Collections;

public class InstanciadorMonedas : MonoBehaviour {

    public GameObject[] objetos;
    public float minPizza = 1f;
    public float maxPizza = 2f;
    public float minHouse = 5f;
    public float maxHouse = 6f;

    // Use this for initialization
    void Start()
    {
        InstanciarPizza();
        InstanciarHouse();
    }

    void InstanciarPizza()
    {
        //Instancia el objeto número 0 "pizza"
        Instantiate(objetos[0], transform.position, Quaternion.identity);
        //Invoco esta misma funcion cada X segundos
        Invoke("InstanciarPizza", Random.Range(minPizza, maxPizza));
        //Debug.Log(gameObject.name);

        //Instancia en un rango aleatorio para generar plataformas a diferentes distancias
        //Instantiate(objetos[Random.Range(0,objetos.Length)],transform.position,Quaternion.identity);
        //Invoco esta misma funcion cada rango aleatorio de segundos. Maximo y Minimo pasado por variable
        //Invoke("Instanciar",Random.Range(min,max));
    }

    void InstanciarHouse()
    {
        //Instancia el objeto número 1 "house"
        Instantiate(objetos[1], transform.position, Quaternion.identity);
        //Invoco esta misma funcion cada X segundos
        Invoke("InstanciarHouse", Random.Range(minHouse, maxHouse));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
