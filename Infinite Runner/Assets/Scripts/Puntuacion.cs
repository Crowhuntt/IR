using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Puntuacion : MonoBehaviour {

    public float puntuacionGanada = 0f;
    public float puntuacionGanadaProv = 0f;
    public Text textoDePuntuacion;
    public Text textoDePuntuacionProv;
    public Text textoMayor;
    public Text textoActual;
    public bool reset = false;

	// Use this for initialization
	void Start () {
        puntuacionGanada = 0f;
    }

    // Update is called once per frame
    void Update () {

        //Compruebo la scena en la que estamos actualmente
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Nivel")
        {
            //Puntuacion va subiendo con los segundos que pasan
            //puntuacionGanada = puntuacionGanada + Time.deltaTime * 10;
            //Escribir(textoDePuntuacion, "PUNTUACIÓN: ", (int)puntuacionGanada);
            //Debug.Log(puntuacionGanada);
        }
        else
        {
            //Debug.Log(puntuacionGanada);
            if (scene.name == "GameOver")
            {
                Escribir(textoMayor, "MAYOR PUNTUACIÓN: ", RecuperarPuntuacion("Puntuacion_Mayor"));
                Escribir(textoActual, "PUNTUACIÓN ACTUAL: ", RecuperarPuntuacion("Puntuacion_Actual"));
            }
            else
            {
               
            }
        }

        ResetearPuntuacion();
        //Debug.Log(puntuacionGanada);
	}

    public void Escribir(Text texto, string titulo, int punt)
    {
        texto.text = titulo + punt;

        //int puntuacionGanadaMostrar = (int)puntuacionGanada;
        //textoDePuntuacion.text = "PUNTUACIÓN: " + puntuacionGanadaMostrar;
    }

    public void GuardarPuntuacion(string clave, int punt)
    {
        //GUARDA LA PUNTUACION MAYOR
        PlayerPrefs.SetInt(clave, punt);
    }

    public int RecuperarPuntuacion(string clave)
    {
        //DEVUELVE LA PUNTUACION MAYOR
        return PlayerPrefs.GetInt(clave);
    }

    public void CalcularPuntuacionMayor(int puntuacionActual)
    {
        int puntuacionMayor = RecuperarPuntuacion("Puntuacion_Mayor");

        //Si no hay PUNTUACION MAYOR ponerle la actual
        if (puntuacionMayor == 0)
        {
            GuardarPuntuacion("Puntuacion_Mayor", puntuacionActual);
        }
        else
        {
            //Mirar si la P_Mayor es menor a la actual y actualizarla
            if (puntuacionMayor < puntuacionActual)
            {
                //Guardar en P_Mayor el P_Actual
                GuardarPuntuacion("Puntuacion_Mayor", puntuacionActual);
            }
        }
    }

    private void ResetearPuntuacion()
    {
        if (reset)
            PlayerPrefs.DeleteAll();
    }
}
