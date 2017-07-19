using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Jugar()
    {
        SceneManager.LoadScene("Nivel");
    }
    public void Clasificacion()
    {
        //SceneManager.LoadScene("Clasificación");
    }
    public void Opciones()
    {
        //SceneManager.LoadScene("Opciones");
    }
    public void Salir()
    {
        Application.Quit();
    }
}
