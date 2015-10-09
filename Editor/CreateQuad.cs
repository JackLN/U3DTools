using UnityEditor;
using UnityEngine;
using System.Collections;

public class CreateQuad : ScriptableWizard {

	public bool CreateOneNewMesh;
	public PrimitiveType MeshType = PrimitiveType.Plane;


	public string MeshName = "Quad";
	public string ObjectName = "Plane";
	public string AssetFolder = "Assets";

	public float Width = 1.0f;
	public float Height = 1.0f;

	public enum AnchorPoint
	{
		TopLeft,
		TopMiddle,
		TopRight,
		RightMiddle,
		BottomRight,
		BottomMiddle,
		BottomLeft,
		LeftMiddle,
		Center,
		Custom,
	}

	public AnchorPoint Anchor = AnchorPoint.Center;
	public float AnchorX = 0.5f;
	public float AnchorY = 0.5f;

	[MenuItem("Tools/Create Object/Quad")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Create Quad",typeof(CreateQuad),"Create");
	}

	void OnInspectorUpdate()
	{
		switch(Anchor)
		{
		case AnchorPoint.TopLeft:
			AnchorX = 0.0f * Width;
			AnchorY = 1.0f * Height;
			break;
		case AnchorPoint.TopMiddle:
			AnchorX = 0.5f * Width;
			AnchorY = 1.0f * Height;
			break;
		case AnchorPoint.TopRight:
			AnchorX = 1.0f * Width;
			AnchorY = 1.0f * Height;
			break;
		case AnchorPoint.RightMiddle:
			AnchorX = 1.0f * Width;
			AnchorY = 0.5f * Height;
			break;
		case AnchorPoint.BottomRight:
			AnchorX = 1.0f * Width;
			AnchorY = 0.0f * Height;
			break;
		case AnchorPoint.BottomMiddle:
			AnchorX = 0.5f * Width;
			AnchorY = 0.0f * Height;
			break;
		case AnchorPoint.BottomLeft:
			AnchorX = 0.0f * Width;
			AnchorY = 0.0f * Height;
			break;
		case AnchorPoint.LeftMiddle:
			AnchorX = 0.0f * Width;
			AnchorY = 0.5f * Height;
			break;
		case AnchorPoint.Center:
			AnchorX = 0.5f * Width;
			AnchorY = 0.5f * Height;
			break;
		case AnchorPoint.Custom:
		default:
			break;
		}
	}

	void OnEnable()
	{
		GetFolderSelection ();
	}

	void GetFolderSelection()
	{
		if(Selection.objects != null && Selection.objects.Length == 1)
		{
			AssetFolder = AssetDatabase.GetAssetPath(Selection.objects[0]);
		}
	}

	void OnWizardCreate ()
	{

		if (CreateOneNewMesh) {
			//创建顶点
			Vector3[] Vertices = new Vector3[4];
			//基于轴点为四边形的4个顶点赋值
			//BottomLeft
			Vertices [0].x = -AnchorX;
			Vertices [0].y = -AnchorY;
			//BottomRight
			Vertices [1].x = Vertices [0].x + Width;
			Vertices [1].y = Vertices [0].y;
			//TopLeft
			Vertices [2].x = Vertices [0].x;
			Vertices [2].y = Vertices [0].y + Height;
			//TopRight
			Vertices [3].x = Vertices [0].x + Width;
			Vertices [3].y = Vertices [0].y + Height;
			
			//创建UV	
			Vector2[] UVs = new Vector2[4];
			//Assign UVs
			//BottomLeft
			UVs [0].x = 0.0f;
			UVs [0].y = 0.0f;
			//BottomRight
			UVs [1].x = 1.0f;
			UVs [1].y = 0.0f;
			//TopLeft
			UVs [2].x = 0.0f;
			UVs [2].y = 1.0f;
			//TopRight
			UVs [3].x = 1.0f;
			UVs [3].y = 1.0f;
			
			//构成四边形的2个三角形
			int[] Triangles = new int[6];
			//Assign triangles
			Triangles [0] = 3;
			Triangles [1] = 1;
			Triangles [2] = 2;
			Triangles [3] = 2;
			Triangles [4] = 1;
			Triangles [5] = 0;
			
			//Generate mesh
			Mesh mesh = new Mesh ();
			mesh.name = MeshName;
			mesh.vertices = Vertices;
			mesh.uv = UVs;
			mesh.triangles = Triangles;
			mesh.RecalculateNormals ();
			//Create asset in database
			AssetDatabase.CreateAsset (mesh, AssetDatabase.GenerateUniqueAssetPath (AssetFolder + "/" + MeshName) + ".asset");
			AssetDatabase.SaveAssets ();
			
			//Create plane game object
			GameObject plane = new GameObject (ObjectName);
			MeshFilter meshFilter = (MeshFilter)plane.AddComponent (typeof(MeshFilter));
			plane.AddComponent (typeof(MeshRenderer));
			//Assign mesh to mesh filter
			meshFilter.sharedMesh = mesh;
			mesh.RecalculateBounds ();
			//Add a box collider component
			//plane.AddComponent(typeof(BoxCollider));
		} 
		else {

			GameObject plane = GameObject.CreatePrimitive(MeshType);
			plane.name = ObjectName;
			//Destroy(plane.GetComponent(typeof(MeshCollider)));
			//MeshFilter meshFilter = (MeshFilter)plane.AddComponent (typeof(MeshFilter));
			//plane.AddComponent (typeof(MeshRenderer));
			//Assign mesh to mesh filter
			//meshFilter.sharedMesh = mesh;
			//mesh.RecalculateBounds ();
		}
	
	
	}


}
