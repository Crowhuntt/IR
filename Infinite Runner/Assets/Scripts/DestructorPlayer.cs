using UnityEngine;
using System.Collections;

public class DestructorPlayer : MonoBehaviour {

    public GameObject manager;
    private Puntuacion puntuacion_script;
    //private CamaraControl camera_script; 
    //public float punt = 0f;
    private bool powerUpCamera = false;
    public float transicion = 1f;

    // Use this for initialization
    void Start () {
        puntuacion_script = manager.GetComponent<Puntuacion>();
        //camera_script = manager.GetComponent<CamaraControl>();
    }

    void OnTriggerEnter2D(Collider2D collisionador)
    {
        if (collisionador.gameObject.tag == "Pizza")
        {
            //Debug.Log(collisionador.gameObject.transform.name);
            //Debug.Log(collisionador.gameObject.transform.tag);
            //punt = punt + 1f;
            //Debug.Log("Nueva Punt: " + punt);

            //Sumo la puntuacion porque es una pizza
            puntuacion_script.puntuacionGanadaProv = puntuacion_script.puntuacionGanadaProv + 1f;
            //Destruyo la pizza
            Destroy(collisionador.gameObject);

            //NO HACE FALTA GUARDAR LA PUNTUACION AQUI, SE HARA EN EL DESTRUCTOR PARTIDA

        }
        else if (collisionador.gameObject.tag == "House")
        {
            puntuacion_script.puntuacionGanada = puntuacion_script.puntuacionGanada + puntuacion_script.puntuacionGanadaProv;
            //Debug.Log("prov: " + puntuacion_script.puntuacionGanadaProv);
            //Debug.Log("p: " + puntuacion_script.puntuacionGanada);
            puntuacion_script.puntuacionGanadaProv = 0f;
        }
        else if (collisionador.gameObject.tag == "PowerUpCamera")
        {
            Destroy(collisionador.gameObject);
            //camera_script.PowerUpCamera();
            //Debug.Log("ok");
            powerUpCamera = true;
            //Camera.main.transform.position = new Vector3(1, 1, -10);
            //Camera.main.orthographicSize = 6;
            Invoke("PowerDown", 10);
        }
        else
        {

        }
    }

    void PowerDown()
    {
        powerUpCamera = false;
        //Camera.main.orthographicSize = 5;
        //Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update () {

        puntuacion_script.Escribir(puntuacion_script.textoDePuntuacion, "PUNTUACIÓN: ", (int)puntuacion_script.puntuacionGanada);
        puntuacion_script.Escribir(puntuacion_script.textoDePuntuacionProv, "Provisional: ", (int)puntuacion_script.puntuacionGanadaProv);

        if (powerUpCamera)
        {
            Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(1f, 1f, -10f), transicion * Time.deltaTime);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 6, transicion * Time.deltaTime);
        }
        else
        {
            if (!powerUpCamera)
            {
                Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(0f, 0f, -10f), transicion * Time.deltaTime);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, transicion * Time.deltaTime);
            }
        }
    }
}
