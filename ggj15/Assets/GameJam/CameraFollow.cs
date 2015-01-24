﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	Vector3 speed = Vector3.zero;
	public Transform cameraTransform;

	void LateUpdate () {

		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = target.position + -10f*cameraTransform.forward;

		currentPosition.x = Smoothing.SpringSmooth(currentPosition.x, targetPosition.x, ref speed.x, 0.5f, Time.deltaTime);
		currentPosition.y = Smoothing.SpringSmooth(currentPosition.y, targetPosition.y, ref speed.y, 0.5f, Time.deltaTime);
		currentPosition.z = Smoothing.SpringSmooth(currentPosition.z, targetPosition.z, ref speed.z, 0.5f, Time.deltaTime);

		transform.position = currentPosition;
	}
}