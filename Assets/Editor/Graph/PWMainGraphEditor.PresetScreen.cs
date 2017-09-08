﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using PW;
using PW.Core;
using PW.Node;

//Preset screen, before the main screen
public partial class PWMainGraphEditor : PWGraphEditor {
	
	//id for the object picker
	int					currentPickerWindow;

	//scroll position on the preset screen
	Vector2				presetScrollPos;

	void DrawPresetLineHeader(string header)
	{
		EditorGUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		EditorGUI.indentLevel = 5;
		EditorGUILayout.LabelField(header, whiteText);
		EditorGUI.indentLevel = 0;
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndVertical();
	}

	void DrawPresetLine(Texture2D tex, string description, Action callback, bool disabled = true)
	{
		EditorGUILayout.BeginVertical();
		{
			GUILayout.FlexibleSpace();
			EditorGUI.BeginDisabledGroup(disabled);
			if (tex != null)
				if (GUILayout.Button(tex, GUILayout.Width(100), GUILayout.Height(100)))
				{
					currentGraph.presetChoosed = true;
					graphNeedReload = true;
					callback();
					currentGraph.UpdateStep(parentGraph.step);
					currentGraph.UpdateChunkSize(parentGraph.chunkSize);
					currentGraph.UpdateSeed(parentGraph.seed);
				}
			EditorGUILayout.LabelField(description, whiteText);
			EditorGUI.EndDisabledGroup();
			GUILayout.FlexibleSpace();
		}
		EditorGUILayout.EndVertical();
	}

	void DrawPresetPanel()
	{
		GUI.DrawTexture(new Rect(0, 0, position.width, position.height), defaultBackgroundTexture);

		presetScrollPos = EditorGUILayout.BeginScrollView(presetScrollPos);

		EditorGUILayout.LabelField("Procedural Worlds");
		
		//Load graph button:
		EditorGUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Load graph"))
			{
				currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive) + 100;
				EditorGUIUtility.ShowObjectPicker< PWNodeGraph >(null, false, "", currentPickerWindow);
			}
			
			if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				UnityEngine.Object selected = null;
				selected = EditorGUIUtility.GetObjectPickerObject();
				if (selected != null)
				{
					Debug.Log("graph " + selected.name + " loaded");
					currentGraph = (PWNodeGraph)selected;
				}
			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		{
			GUILayout.FlexibleSpace();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			{
				GUILayout.FlexibleSpace();

				//3 DrawPresetLine per line + 1 header:
				EditorGUILayout.BeginHorizontal();
				DrawPresetLineHeader("2D");
				DrawPresetLine(preset2DSideViewTexture, "2D sideview procedural terrain", () => {});
				DrawPresetLine(preset2DTopDownViewTexture, "2D top down procedural terrain", () => {
					currentGraph.outputType = PWGraphTerrainType.TopDown2D;
					CreateNewNode(typeof(PWNodePerlinNoise2D));
					PWNode perlin = currentGraph.nodes.Last();
					perlin.windowRect.position += Vector2.left * 400;
					CreateNewNode(typeof(PWNodeTopDown2DTerrain));
					PWNode terrain = currentGraph.nodes.Last();

					//FIXME
					// perlin.AttachLink("output", terrain, "texture");
					// terrain.AttachLink("texture", perlin, "output");
					terrain.AttachLink("terrainOutput", currentGraph.outputNode, "inputValues");
					currentGraph.outputNode.AttachLink("inputValues", terrain, "terrainOutput");
				}, false);
				DrawPresetLine(null, "", () => {});
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				DrawPresetLineHeader("3D");
				DrawPresetLine(preset3DPlaneTexture, "3D plane procedural terrain", () => {});
				DrawPresetLine(preset3DSphericalTexture, "3D spherical procedural terrain", () => {});
				DrawPresetLine(preset3DCubicTexture, "3D cubic procedural terrain", () => {});
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				DrawPresetLineHeader("Density fields");
				DrawPresetLine(preset1DDensityFieldTexture, "1D float density field", () => {});
				DrawPresetLine(preset2DDensityFieldTexture, "2D float density field", () => {});
				DrawPresetLine(preset3DDensityFieldTexture, "3D float density field", () => {});
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				DrawPresetLineHeader("Others");
				DrawPresetLine(presetMeshTetxure, "mesh", () => {});
				DrawPresetLine(null, "", () => {});
				DrawPresetLine(null, "", () => {});
				EditorGUILayout.EndHorizontal();
				
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
	}

}
