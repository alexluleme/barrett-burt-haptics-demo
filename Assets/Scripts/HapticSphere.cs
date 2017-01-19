﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticSphere : HapticObject {

	void Awake () {
		// set default values for kp and kp
		kp = 700.0f;
		kd = 100.0f;
	}

	override protected void CalcForce (Collision c) {
		Vector3 position = this.gameObject.transform.position;

		// This should be a sphere, but just in case it's not, check all the dimensions.
		Vector3 dims = this.gameObject.transform.localScale;
		float radius = c.contacts [0].thisCollider.GetComponent<SphereCollider> ().radius *
			Mathf.Max (dims.x, dims.y, dims.z);

		Vector3 contactPos = c.contacts [0].point;
		float depth = radius - (position - contactPos).magnitude;  // > 0

		Vector3 otherVelocity = c.contacts [0].otherCollider.gameObject.GetComponent<RobotController>().GetVelocity();

		Vector3 direction = (position - contactPos).normalized;
		force = -kp * depth * direction +  // stiffness: pushes outward
			-kd * Vector3.Dot (otherVelocity, direction) * direction;  // damping: pushes against radial velocity (+ or -)
	}
}
