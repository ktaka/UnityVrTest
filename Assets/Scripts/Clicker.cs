using UnityEngine;
using System.Collections;

public class Clicker {

	public bool clicked () {
	#if (UNITY_ANDROID)
		return GvrController.ClickButtonDown;
	//#if (UNITY_ANDROID || UNITY_IPHONE)
	//	return Cardboard.SDK.Triggered;
	#else
		return Input.anyKeyDown;
	#endif
	}
	
}
