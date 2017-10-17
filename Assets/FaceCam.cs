using UnityEngine;
using System.Collections;

public class FaceCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 toCam = transform.position- Camera.main.transform.position;
        toCam.Normalize();

        Quaternion quat = Quaternion.FromToRotation(Vector3.forward, transform.InverseTransformDirection(toCam));

        

        transform.Rotate(new Vector3(0, quat.eulerAngles.y, 0));
    }
}
