using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToScreen : MonoBehaviour {

	// Use this for initialization
	public float CameraMargin;
	public Vector3 BoundingBoxCenter;
	public GameObject target;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Fit(){
		Camera main = Camera.main;
		GameObject current = target;
		Bounds bb = GetCompoundBB(current);
		Rect r = BoundsToScreenRect(bb);


		Debug.DrawLine(main.ScreenToWorldPoint(new Vector3(r.min.x,r.min.y,2)),main.ScreenToWorldPoint(new Vector3(r.max.x,r.min.y,2)));
		Debug.DrawLine(main.ScreenToWorldPoint(new Vector3(r.min.x,r.min.y,2)),main.ScreenToWorldPoint(new Vector3(r.min.x,r.max.y,2)));
		Debug.DrawLine(main.ScreenToWorldPoint(new Vector3(r.max.x,r.max.y,2)),main.ScreenToWorldPoint(new Vector3(r.min.x,r.max.y,2)));
		Debug.DrawLine(main.ScreenToWorldPoint(new Vector3(r.max.x,r.max.y,2)),main.ScreenToWorldPoint(new Vector3(r.max.x,r.min.y,2)));
		var worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);
		BoundingBoxCenter = bb.center;

		float stepBackRatio = 2;
			main.transform.position = BoundingBoxCenter + new Vector3(1,1,0)*Mathf.Max(bb.extents.x,bb.extents.y,bb.extents.z)*2;
			main.transform.LookAt(bb.center);
			stepBackRatio += 2;
		
		
		main.orthographicSize = (r.size.y/worldToPixels)/2 + CameraMargin;
	}



	Bounds GetCompoundBB(GameObject target){
		var filters = target.GetComponentsInChildren<MeshRenderer>();
		var bound = new Bounds();

		foreach(var filter in filters)
		{
			//if members are not tagged group or microlabel since these are just groups
			
				bound.Encapsulate(filter.bounds);
		}

		return bound;

	}

	 public Rect BoundsToScreenRect(Bounds bounds)
	{
		// Get mesh origin and farthest extent (this works best with simple convex meshes)
		Vector3[] points = new Vector3[8];
		
		points[0] = Camera.main.WorldToScreenPoint(bounds.min);
		points[1] = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x,bounds.max.y,bounds.min.z));
		points[2] = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x,bounds.min.y,bounds.max.z));
		points[3] = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x,bounds.max.y,bounds.max.z));
		points[4] = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x,bounds.min.y,bounds.min.z));
		points[5] = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x,bounds.max.y,bounds.min.z));
		points[6] = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x,bounds.min.y,bounds.max.z));
		points[7] = Camera.main.WorldToScreenPoint(bounds.max);

		Vector2 min,max;
		min = new Vector2(Mathf.Infinity,Mathf.Infinity);
		max = new Vector2(-Mathf.Infinity,-Mathf.Infinity);
	

		for (int i = 0; i < points.Length; i++)
		{
			
			Vector3 p = points[i];
			
			if(p.x > max.x) {
				max.x = p.x;
			}
			if(p.y > max.y) {
				max.y = p.y;
			}
			if(p.x < min.x) {
				min.x = p.x;
			}
			if(p.y < min.y) {
				min.y = p.y;
			}
		}

		
		// Create rect in screen space and return - does not account for camera perspective
		return new Rect(min,max-min);
	}
}
