using UnityEngine;
using System.Collections;

public class MovimientoObstaculos : MonoBehaviour {

    [SerializeField] private float m_MaxSpeed = -0.05f;                    // The fastest the player can travel in the x axis.

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Muevo el objeto hacia la izquierda
        transform.Translate(m_MaxSpeed, 0, 0);
    }
}
