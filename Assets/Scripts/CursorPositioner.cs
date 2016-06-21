using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CursorPositioner : MonoBehaviour {
	private float defaultPosZ;

	// Use this for initialization
	void Start () {
		defaultPosZ = transform.localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
		if (GvrController.IsTouching) {
			Transform camera = Camera.main.transform;
//		Ray ray = new Ray (camera.position, camera.rotation * Vector3.forward);
			//transform.rotation = GvrController.Orientation;
			Vector3 rayDir = GvrController.Orientation * Vector3.forward; // Daydream
			Ray ray = new Ray (camera.position, rayDir); // Daydream
			transform.position = ray.GetPoint(defaultPosZ);
			transform.LookAt (camera.position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				//if (hit.distance <= defaultPosZ) {
				//transform.position = hit.point;
//				transform.LookAt (camera.position);
				//float scale = hit.distance * Mathf.Tan (Mathf.PI / 180.0f * 30.0f);
				//transform.localScale *= scale;
				//transform.localPosition = new Vector3(0, 0, hit.distance);
				//transform.localPosition = hit.point;
//			} else {
//				transform.localPosition = new Vector3 (0, 0, defaultPosZ);
				//}
			}
		}
	}
}
