using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

    // Use this for initialization

   public Transform target;
    public float speed;
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
        //transform.Translate(Vector3.right * Time.deltaTime*speed);
        transform.RotateAround(target.position, Vector3.up, Time.deltaTime * speed);
    }
}
