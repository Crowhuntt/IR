using UnityEngine;
using System.Collections;

public class CamaraControl : MonoBehaviour {

    public GameObject Jugador;

	// Use this for initialization
	void Start () {
        //Muevo la camara 8 posiciones a la izquierda del jugador
        //transform.position = new Vector3(Jugador.transform.position.x + 8, transform.position.y, transform.position.z);
        transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 5;
    }

    // Update is called once per frame
    void Update () {
        //x = y
        //Muevo la camara 8 posiciones a la izquierda del jugador
        //transform.position = new Vector3(Jugador.transform.position.x + 8, transform.position.y, transform.position.z);
        //Mover la camara gradualmente desde posicion local a la que queramos
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(1, 1, -10), 1 * Time.deltaTime);
    }
}
