using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	Vector3 speed = Vector3.zero;
	public Transform cameraTransform;
	public Bird bird;

	float minDistance = -7f;
	float maxDistance = -15f;

	void LateUpdate () {

		float cameraDistance = bird.NormalizedVelocityMagnitude * (maxDistance - minDistance) + minDistance;

		Vector3 currentPosition = transform.position;
		Vector3 targetPosition = target.position + cameraDistance*cameraTransform.forward; //prev -10 distance

		currentPosition.x = Smoothing.SpringSmooth(currentPosition.x, targetPosition.x, ref speed.x, 0.5f, Time.deltaTime);
		currentPosition.y = Smoothing.SpringSmooth(currentPosition.y, targetPosition.y, ref speed.y, 0.5f, Time.deltaTime);
		currentPosition.z = Smoothing.SpringSmooth(currentPosition.z, targetPosition.z, ref speed.z, 0.5f, Time.deltaTime);

		transform.position = currentPosition;
		Quaternion targetRotation = target.rotation * Quaternion.AngleAxis(20f, Vector3.right);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Clamp01(Time.deltaTime * 5f));
	}
}
