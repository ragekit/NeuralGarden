using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

    // Use this for initialization

    public float speed;
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(Vector3.right * Time.deltaTime*speed);
        transform.RotateAround(GetComponent<FitToScreen>().BoundingBoxCenter, Vector3.up, Time.deltaTime * speed);
    }
}
