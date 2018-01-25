﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PW.Core;

namespace PW.Node
{
	public class PWNodeBiomeSurfaceModifiers : PWNode
	{

		//Called only when the node is created (not when it is enabled/loaded)
		public override void OnNodeCreation()
		{
			name = "PWNodeBiomeSurfaceModifiers";
		}

		public override void OnNodeEnable()
		{
			//initialize here all unserializable datas used for GUI (like Texture2D, ...)
		}

		public override void OnNodeGUI()
		{
			//your node GUI
		}

		public override void OnNodeProcess()
		{
			//write here the process which take inputs, transform them and set outputs.
		}
		
	}
}