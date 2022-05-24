using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {
	// Enum representing linkage
	[Serializable]
	public enum Slot {
		INVALID,
		head,
		leftShoulder, rightShoulder,
		leftElbow, rightElbow,
		leftHand, rightHand,
		pelvis,
		leftKnee, rightKnee,
		leftFoot, rightFoot
	}

	// Class wrapper around unity's Pose to enable reference semantics
	[Serializable]
	public class PoseRef {
		public Pose pose = Pose.identity;
	}
	
	[Header("Pose Transforms (Read Only)")] 
	[ReadOnly] public PoseRef head;
	[ReadOnly] public PoseRef leftShoulder, rightShoulder,
		leftElbow, rightElbow,
		leftHand, rightHand,
		pelvis,
		leftKnee, rightKnee,
		leftFoot, rightFoot;
}
