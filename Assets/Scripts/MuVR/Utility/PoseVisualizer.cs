﻿using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace MuVR.Utility {
	
	// Component that visualizes a pose in a pose slot
	public class PoseVisualizer : SyncPose {
		private UserAvatar.PoseRef target => getTarget;
		
		[PropertyTooltip("The prefab that should be spawned to visualize this pose")]
		public GameObject visualizationPrefab;
		// Reference to the spawned prefab
		private GameObject spawnedPrefab;

		// When the object is created make sure to update the target
		public new void Start() {
			UpdateTarget();
			RespawnVisualization();
		}

		// Function that gets rid of the old visualization and spawns a new one in its place
		public void RespawnVisualization() {
			if (spawnedPrefab is not null) Destroy(spawnedPrefab);
			spawnedPrefab ??= Instantiate(visualizationPrefab, target?.pose.position ?? Vector3.zero, target?.pose.rotation ?? Quaternion.identity, transform);
			spawnedPrefab.name = slot;
		}

		// Update is called once per frame to make sure that the visualization is properly synced with the pose
		public new void LateUpdate() {
			spawnedPrefab.transform.position = UpdatePosition(spawnedPrefab.transform.position, target.pose.position);
			spawnedPrefab.transform.rotation = UpdateRotation(spawnedPrefab.transform.rotation, target.pose.rotation);
		}
	}

#if UNITY_EDITOR
	// Editor that makes hooking up a sync pose to slots much easier
	[CustomEditor(typeof(PoseVisualizer))]
	[CanEditMultipleObjects]
	public class PoseVisualizerEditor : SyncPoseEditor {
		// Properties of the object we wish to show a default UI for
		protected SerializedProperty visualizationPrefab;

		protected new void OnEnable() {
			base.OnEnable();
			visualizationPrefab = serializedObject.FindProperty("visualizationPrefab");
		}

		// Immediate mode GUI used to edit a SyncPose in the inspector
		public override void OnInspectorGUI() {
			var sync = (PoseVisualizer)target;

			serializedObject.Update();

			TargetAvatarField(sync);
			PoseSlotField(sync);

			// Allow selection of the prefab spawned to visualize the pose
			EditorGUILayout.PropertyField(visualizationPrefab);

			// Toggle hiding additional settings
			sync.showSettings = EditorGUILayout.Foldout(sync.showSettings, "Additional Settings");
			if (sync.showSettings) {
				PositionSettingsField(sync);
				RotationSettingsField(sync);
				
				// Present a field with the pose offset
				EditorGUILayout.PropertyField(localOffset);
				EditorGUILayout.PropertyField(globalOffset);
			}

			// Apply changes to the fields
			var oldAvatar = sync.targetAvatar;
			serializedObject.ApplyModifiedProperties();
			// If the target avatar has changed, automatically select its first slot
			if (sync.targetAvatar != oldAvatar && sync.targetAvatar is not null)
				sync.slot = sync.targetAvatar.slots.Keys.First();
		}
	}
#endif
}