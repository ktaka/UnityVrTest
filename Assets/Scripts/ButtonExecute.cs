using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonExecute : MonoBehaviour {
	public GameObject cursorObject;
	public float cursorDistance = 0.5f;
	public GameObject controlOrientationObject;
	private GameObject currentButton;
	private Clicker clicker = new Clicker ();

	enum Tools {
		Pointer = 0,
		Hose,
		Num
	};
	private Tools currentTool = Tools.Pointer;
	private Vector2 prevTouchPos;

	void Start () {
		cursorObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (GvrController.AppButtonDown) {
			ChangeTool ();
		}

		switch (currentTool) {
		case Tools.Pointer: 
			ControlPointer ();
			break;
		case Tools.Hose:
			ControlHose ();
			break;
		}
	}

	Tools ChangeTool() {
		currentTool++;
		if (currentTool >= Tools.Num) {
			currentTool = Tools.Pointer;
		}
		return currentTool;
	}

	void ControlPointer() {
		if (GvrController.TouchUp) {
			cursorObject.SetActive (false);
		} else if (GvrController.TouchDown) {
			cursorObject.SetActive (true);
		}
		if (cursorObject.activeSelf) {
			Transform camera = Camera.main.transform;
			Vector3 rayDir = GvrController.Orientation * Vector3.forward; // Daydream
			Ray ray = new Ray (camera.position, rayDir); // Daydream

			cursorObject.transform.position = ray.GetPoint (cursorDistance);
			cursorObject.transform.LookAt (camera.position);

			RaycastHit hit;
			GameObject hitButton = null;
			PointerEventData data = new PointerEventData (EventSystem.current);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.gameObject.tag == "Button") {
					hitButton = hit.transform.parent.gameObject;
				}
			}
			if (currentButton != hitButton) {
				if (currentButton != null) { // ハイライトを外す
					ExecuteEvents.Execute<IPointerExitHandler> (currentButton, data, ExecuteEvents.pointerExitHandler);
				}
				currentButton = hitButton;
				if (currentButton != null) { // ハイライトする
					ExecuteEvents.Execute<IPointerEnterHandler> (currentButton, data, ExecuteEvents.pointerEnterHandler);
				}
			}
			if (currentButton != null) {
				if (clicker.clicked ()) {
					ExecuteEvents.Execute<IPointerClickHandler> (currentButton, data, ExecuteEvents.pointerClickHandler);
				}
			}
		}
	}

	void ControlHose() {
		controlOrientationObject.transform.rotation = GvrController.Orientation;
		if (GvrController.TouchDown) {
			prevTouchPos = GvrController.TouchPos;
		}
		if (GvrController.IsTouching) {
			Vector2 touchPosDelta = GvrController.TouchPos - prevTouchPos;
			//touchPosDelta *= 0.1f;
			Vector3 position = controlOrientationObject.transform.position;
			position.x += touchPosDelta.x;
			position.z -= touchPosDelta.y;
			controlOrientationObject.transform.position = position;
			prevTouchPos = GvrController.TouchPos;
		}
	}
}
