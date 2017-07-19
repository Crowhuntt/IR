using UnityEngine;
using System.Collections;

public class InstanciadorSuelo : MonoBehaviour {

    public GameObject[] objetos;
    public float min = 1f;
    public float max = 2f;

	// Use this for initialization
	void Start () {
        Instanciar();
	}
	
    void Instanciar()
    {
        //Instancia el objeto número 0
        Instantiate(objetos[0],transform.position,Quaternion.identity);
        //Invoco esta misma funcion cada X segundos
        Invoke("Instanciar",1);

        //Instancia en un rango aleatorio para generar plataformas a diferentes distancias
        //Instantiate(objetos[Random.Range(0,objetos.Length)],transform.position,Quaternion.identity);
        //Invoco esta misma funcion cada rango aleatorio de segundos. Maximo y Minimo pasado por variable
        //Invoke("Instanciar",Random.Range(min,max));
    }

	// Update is called once per frame
	void Update () {
	
	}
}
