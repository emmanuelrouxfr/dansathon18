﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AugmentaCamera))]
public class AugmentaCameraEditor : Editor
{
    private static GUIStyle ToggleButtonStyleNormal = null;
    private static GUIStyle ToggleButtonStyleToggled = null;
    private static GUIStyle CenteredLabelStyle = null;

    public override void OnInspectorGUI()
    {
        AugmentaCamera augmentaCamera = (AugmentaCamera)target;

        if (ToggleButtonStyleNormal == null)
        {
            ToggleButtonStyleNormal = "Button";
            ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
        }

        if(CenteredLabelStyle == null)
            CenteredLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Target Camera", EditorStyles.boldLabel);
        augmentaCamera.TargetCameraName = EditorGUILayout.TextField("Target Camera Name", augmentaCamera.TargetCameraName);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Update Settings", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("On Start", CenteredLabelStyle, GUILayout.ExpandWidth(true));
        GUILayout.BeginHorizontal();
        augmentaCamera.updateCameraOnStart = GUILayout.Toggle(augmentaCamera.updateCameraOnStart, "Camera", augmentaCamera.updateCameraOnStart ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        augmentaCamera.updateTransformOnStart = GUILayout.Toggle(augmentaCamera.updateTransformOnStart, "Transform", augmentaCamera.updateTransformOnStart ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        augmentaCamera.updatePostProcessOnStart = GUILayout.Toggle(augmentaCamera.updatePostProcessOnStart, "PostProcess", augmentaCamera.updatePostProcessOnStart ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        augmentaCamera.updateAugmentaOnStart = GUILayout.Toggle(augmentaCamera.updateAugmentaOnStart, "Augmenta", augmentaCamera.updateAugmentaOnStart ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Every Frame", CenteredLabelStyle, GUILayout.ExpandWidth(true));
        GUILayout.BeginHorizontal();
        augmentaCamera.alwaysUpdateCamera = GUILayout.Toggle(augmentaCamera.alwaysUpdateCamera, "Camera", augmentaCamera.alwaysUpdateCamera ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        augmentaCamera.alwaysUpdateTransform = GUILayout.Toggle(augmentaCamera.alwaysUpdateTransform, "Transform", augmentaCamera.alwaysUpdateTransform ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        augmentaCamera.alwaysUpdatePostProcess = GUILayout.Toggle(augmentaCamera.alwaysUpdatePostProcess, "PostProcess", augmentaCamera.alwaysUpdatePostProcess ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        augmentaCamera.alwaysUpdateAugmenta = GUILayout.Toggle(augmentaCamera.alwaysUpdateAugmenta, "Augmenta", augmentaCamera.alwaysUpdateAugmenta ? ToggleButtonStyleToggled : ToggleButtonStyleNormal);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        augmentaCamera.disableAfterUpdate = EditorGUILayout.Toggle("Disable After Update", augmentaCamera.disableAfterUpdate);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Augmenta Area Anchor Settings", EditorStyles.boldLabel);
        augmentaCamera.useAnchor = EditorGUILayout.Toggle("Use Anchor", augmentaCamera.useAnchor);
        augmentaCamera.augmentaAreaAnchor = (AugmentaAreaAnchor)EditorGUILayout.ObjectField("Augmenta Area Anchor", augmentaCamera.augmentaAreaAnchor, typeof(AugmentaAreaAnchor), true);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Augmenta Camera Settings", EditorStyles.boldLabel);

        augmentaCamera.Zoom = EditorGUILayout.FloatField("Zoom", augmentaCamera.Zoom);
        augmentaCamera.CamDistToAugmenta = EditorGUILayout.Slider("Cam Dist To Augmenta", augmentaCamera.CamDistToAugmenta, 0.0f, 200.0f);
        augmentaCamera.NearFrustrum = EditorGUILayout.FloatField("Near Frustrum", augmentaCamera.NearFrustrum);
        augmentaCamera.drawNearCone = EditorGUILayout.Toggle("Draw Near Cone", augmentaCamera.drawNearCone);
        augmentaCamera.drawFrustum = EditorGUILayout.Toggle("Draw Frustum", augmentaCamera.drawFrustum);
        augmentaCamera.centerOnAugmentaArea = EditorGUILayout.Toggle("Center On Augmenta Area", augmentaCamera.centerOnAugmentaArea);
        augmentaCamera.lookTarget = (Transform)EditorGUILayout.ObjectField("Look Target", augmentaCamera.lookTarget, typeof(Transform), true);
    }
}
