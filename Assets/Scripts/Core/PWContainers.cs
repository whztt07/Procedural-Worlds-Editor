﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace PW
{
	/*
	**	Graph calculus storage classes (must not be serialized)
	*/

	public abstract class Sampler
	{
		public int		size;
		public float	step;
	}

	public class Sampler2D : Sampler
	{
		[NonSerializedAttribute]
		public float[,]		map;
		
		public float this[int x, int y]
		{
			get {return map[x, y];}
			set {map[x, y] = value;}
		}

		public Sampler2D(int size, float step)
		{
			this.map = new float[size, size];
			this.step = step;
			this.size = size;
		}

		public void Resize(int size)
		{
			this.map = new float[size, size];
			this.size = size;
		}

		public void Foreach(Func< int, int, float > callback)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					map[x, y] = callback(x, y);
		}
		
		public void Foreach(Func< int, int, float, float > callback)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					map[x, y] = callback(x, y, map[x, y]);
		}
		
		public void Foreach(Action< int, int, float > callback)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					callback(x, y, map[x, y]);
		}
		
		public override string ToString()
		{
			return "Samp2D(" + size + ")";
		}
	}

	[System.SerializableAttribute]
	public class Sampler3D : Sampler
	{
		[NonSerializedAttribute]
		public float[,,]	map;

		public Sampler3D(int size, float step)
		{
			this.map = new float[size, size, size];
			this.size = size;
			this.step = step;
		}

		public float this[int x, int y, int z]
		{
			get {return map[x, y, z];}
			set {map[x, y, z] = value;}
		}
		
		public void Foreach(Func< int, int, int, float > callback)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					for (int z = 0; z < size; z++)
						map[x, y, z] = callback(x, y, z);
		}
		
		public void Foreach(Func< int, int, int, float, float > callback)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					for (int z = 0; z < size; z++)
						map[x, y, z] = callback(x, y, z, map[x, y, z]);
		}
		
		public void Foreach(Action< int, int, int, float > callback)
		{
			for (int x = 0; x < size; x++)
				for (int y = 0; y < size; y++)
					for (int z = 0; z < size; z++)
						callback(x, y, z, map[x, y, z]);
		}

		public override string ToString()
		{
			return "Samp3D(" + size + ")";
		}
	}

	/*
	**	Terrain storage classes:
	*/

	[System.SerializableAttribute]
	public abstract class ChunkData
	{
		public int			size;

		public override string ToString()
		{
			return GetType().Name + "(" + size + ")";
		}
	}

	[System.SerializableAttribute]
	public class SideView2DData : ChunkData
	{
	}
	
	[System.SerializableAttribute]
	public class TopDown2DData : ChunkData
	{
		//TODO for chunk saving to file: encode image to png and store path.
		[System.NonSerializedAttribute]
		public Texture2D	texture;
	}

	//TODO: other storage classes

	/*
	**	Biomes
	*/

	//TODO: this is not float its sampler2D x)

	[System.SerializableAttribute]
	public enum BiomeDataType
	{
		BiomeData,
		BiomeData3D,
		WaterlessBiomeData,
		WaterlessBiomeData3D,
	}

	[System.SerializableAttribute]
	public class BasicEdaphicData
	{
		public Sampler2D		PH;
		public Sampler2D		drainage;
		public Sampler2D		nutrient;
		public Sampler2D		mineral;
	}

	[System.SerializableAttribute]
	public class ComplexEdaphicData : BasicEdaphicData
	{
		public Sampler2D		clay;
		public Sampler2D		silt;
		public Sampler2D		sand;
		public Sampler2D		gravel;
	}
	
	[System.SerializableAttribute]
	public class BasicEdaphicData3D
	{
		public Sampler3D		PH;
		public Sampler3D		drainage;
		public Sampler3D		nutrient;
		public Sampler3D		mineral;
	}

	[System.SerializableAttribute]
	public class ComplexEdaphicData3D : BasicEdaphicData
	{
		public Sampler3D		clay;
		public Sampler3D		silt;
		public Sampler3D		sand;
		public Sampler3D		gravel;
	}

	public class BiomeData
	{
		public float				waterLevel;
		public Sampler2D			waterHeight;
		public bool					aquatic;
		
		public Sampler2D			terrain;
		public Sampler3D			terrain3D;

		public Sampler2D			wetness;
		public Sampler2D			temperature;
		
		public Vector2				wind;
		public Sampler2D			lighting;
		public BasicEdaphicData		soil;
		
		public Sampler2D			air;
		public ComplexEdaphicData	complexSoil;

		public Sampler2D[]			datas;
		public string[]				dataNames;
	}
	
	public class BiomeData3D
	{
		public float				waterLevel;
		
		public Sampler2D			terrain;
		public Sampler3D			terrain3D;
		
		public Sampler3D			wetness;
		public Sampler3D			temperature;
		
		public Vector3				wind;
		public Sampler2D			lighting;
		public BasicEdaphicData3D	soil;
		
		public Sampler3D			air;
		public ComplexEdaphicData3D	complexSoil;

		public Sampler3D[]			datas;
		public string[]				dataNames;
	}
	
	public class WaterlessBiomeData
	{
		public Sampler2D			height;
		
		public Sampler2D			terrain;
		public Sampler3D			terrain3D;
		
		public Sampler2D			wetness;
		public Sampler2D			temperature;
		
		public Vector2				wind;
		public Sampler2D			lighting;
		public BasicEdaphicData		soil;
		
		public Sampler2D			air;
		public ComplexEdaphicData	complexSoil;

		public Sampler2D[]			datas;
		public string[]				dataNames;
	}
	
	public class WaterlessBiomeData3D
	{
		public Sampler2D			height;
		
		public Sampler2D			terrain;
		public Sampler3D			terrain3D;
		
		public Sampler3D			wetness;
		public Sampler3D			temperature;
		
		public Vector3				wind;
		public Sampler2D			lighting;
		public BasicEdaphicData3D	soil;
		
		public Sampler3D			air;
		public ComplexEdaphicData3D	complexSoil;

		public Sampler3D[]			datas;
		public string[]				dataNames;
	}

	/*
	**	Utils
	*/

	[System.SerializableAttribute]
	public struct Vector3i
	{
		public int	x;
		public int	y;
		public int	z;

		public Vector3i(float x, float y, float z)
		{
			this.x = (int)x;
			this.y = (int)y;
			this.z = (int)z;
		}

		public Vector3i(float a)
		{
			this.x = (int)a;
			this.y = (int)a;
			this.z = (int)a;
		}

		public static explicit operator Vector3(Vector3i v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		public static implicit operator Vector3i(Vector3 v)
		{
			return new Vector3i(v.x, v.y, v.z);
		}
	}

	[System.SerializableAttribute]
	public class Pair< T, U >
	{
		[SerializeField]
		public T	first;
		[SerializeField]
		public U	second;

		public Pair(T f, U s)
		{
			first = f;
			second = s;
		}
	}

	/*
	**	PWNode render settings
	*/

	[System.SerializableAttribute]
	public class PWGUISettings
	{
		public bool		active {get;  private set;}
		public Vector2	windowPosition;

		[System.NonSerializedAttribute]
		public object	oldState = null;

		//we put all possible datas for each inputs because unity serialization does not support inheritence :(
		
		//colorPicker:
		public SerializableColor	c;
		public Vector2				thumbPosition;

		//Sampler2D:
		public FilterMode			filterMode;
		public SerializableGradient	serializableGradient;
		[System.NonSerializedAttribute]
		public bool					update;

		[System.NonSerializedAttribute]
		public Gradient				gradient;
		[System.NonSerializedAttribute]
		public Texture2D			texture;

		//Texture:
		// public FilterMode		filterMode; //duplicated
		public ScaleMode			scaleMode;
		public float				scaleAspect;
		//TODO: light-weight serializableMaterial
		[System.NonSerializedAttribute]
		public Material				material;

		//Editor utils:
		[System.NonSerializedAttribute]
		public int					popupHeight;
		

		public PWGUISettings()
		{
			active = false;
			update = false;
		}

		public object Active(object o)
		{
			active = true;
			oldState = o;
			return o;
		}

		public object InActive()
		{
			active = false;
			return oldState;
		}

		public object Invert(object o)
		{
			if (active)
				return InActive();
			else
				return Active(o);
		}
	}
}