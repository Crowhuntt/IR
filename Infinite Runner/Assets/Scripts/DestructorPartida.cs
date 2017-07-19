using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestructorPartida : MonoBehaviour {

    public GameObject manager;
    private Puntuacion puntuacion_script;

    // Use this for initialization
    void Start()
    {
        puntuacion_script = manager.GetComponent<Puntuacion>();
    }

    void OnTriggerEnter2D(Collider2D collisionador)
    {
        if (collisionador.gameObject.tag == "Player")
        {
            //Debug.Log(collisionador.gameObject.transform.name);
            //Debug.Log(collisionador.gameObject.transform.tag);
            //Cambiar de escena a GameOver
            //Debug.Log(puntuacion_script.puntuacionGanada);
            puntuacion_script.GuardarPuntuacion("Puntuacion_Actual", (int)puntuacion_script.puntuacionGanada);
            //Esta linea es lo mismo que la de encima
            //PlayerPrefs.SetInt("Puntuacion_Actual", (int)puntuacion_script.puntuacionGanada);
            puntuacion_script.CalcularPuntuacionMayor((int)puntuacion_script.puntuacionGanada);
            //Debug.Log("Pun Actu: " + puntuacion_script.RecuperarPuntuacion("Puntuacion_Actual"));
            //Debug.Log("Pun Mayor: " + puntuacion_script.RecuperarPuntuacion("Puntuacion_Mayor"));
            //Debug.Break();
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
