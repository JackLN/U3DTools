using UnityEngine;
using UnityEditor;
using System.Collections;

public class UVEditor : EditorWindow 
{
	//表示纹理预置对象
	public GameObject AtlasDataObject = null;
	//表示图集中的数据
	public PackageData AtlasDataComponent = null;
	//下拉列表中默认选中项的索引
	public int PopupIndex = 0;
	
	//精灵选择模式: sprites 或者 custom 
	//(sprites = 从图集中选择精灵, custom = 通过UV坐标选择精灵)
	public string[] Modes = {"Select By Sprites", "Select By UVs"};
	
	//选中的模式的索引
	public int ModeIndex = 0;
	
	public Rect CustomRect = new Rect(0,0,0,0);
	
	[MenuItem("Tools/TexturePacker UV Editor")]
	static void Init()
	{
		//显示窗口
		GetWindow(typeof(UVEditor),false,"Texture Atlas",true);
	}
	void OnGUI()
	{
		//绘制纹理图集选择器
		GUILayout.Label("Atlas Generation", EditorStyles.boldLabel);
		AtlasDataObject = (GameObject) EditorGUILayout.ObjectField("Atlas Object", AtlasDataObject, 
		                                                           typeof (GameObject), true);
		
		//如果没有可用的纹理图集对象，则取消
		if(AtlasDataObject == null)
			return;
		
		//获取纹理预置对象的atlas data组件
		AtlasDataComponent = AtlasDataObject.GetComponent<PackageData>();
		//如果组件中没有可用的数据，则取消
		if(!AtlasDataComponent)
			return;
		
		//精灵模式的选择
		ModeIndex = EditorGUILayout.Popup(ModeIndex, Modes);
		
		//如果选择的是Select By Sprites
		if(ModeIndex != 1)
		{
			//弹出当前可用纹理的选择器
			PopupIndex = EditorGUILayout.Popup(PopupIndex, AtlasDataComponent.TextureNames);
			
			//单击“Select Sprite From Atlas”按钮以后
			if(GUILayout.Button("Select Sprite From Atlas"))
			{
				//开始设置选中的网格对象的UV坐标
				if(Selection.gameObjects.Length > 0)
				{
					foreach(GameObject Obj in Selection.gameObjects)
					{
						//确认是否是网格对象
						if(Obj.GetComponent<MeshFilter>())
							UpdateUVs(Obj, AtlasDataComponent.TextureUVs[PopupIndex]);
					}
				}
			}
		}
		else  
		{
			//选中的是Select By UVs
			GUILayout.Label ("X");
			CustomRect.x = EditorGUILayout.FloatField(CustomRect.x);
			GUILayout.Label ("Y");
			CustomRect.y = EditorGUILayout.FloatField(CustomRect.y);
			GUILayout.Label ("Width");
			CustomRect.width = EditorGUILayout.FloatField(CustomRect.width);
			GUILayout.Label ("Height");
			CustomRect.height = EditorGUILayout.FloatField(CustomRect.height);
			
			//单击“Select Sprite From Atlas”按钮以后
			if(GUILayout.Button("Select Sprite From Atlas"))
			{
				//开始设置选中的网格对象的UV坐标
				if(Selection.gameObjects.Length > 0)
				{
					foreach(GameObject Obj in Selection.gameObjects)
					{
						//确认是否是网格对象
						if(Obj.GetComponent<MeshFilter>())
							UpdateUVs(Obj, CustomRect);
					}
				}
			}
		}
	}
	//用于修改选中的网格对象的UV坐标
	void UpdateUVs(GameObject MeshOject, Rect AtlasUVs)
	{
		Debug.Log(AtlasUVs.x + ";" + AtlasUVs.y + ";"+ AtlasUVs.width + ";"+ AtlasUVs.height);
		bool Reset = false;
		//获取Mesh Filter组件
		MeshFilter MFilter = MeshOject.GetComponent<MeshFilter>();
		Mesh MeshObject = MFilter.sharedMesh;
		Vector3[] Vertices = MeshObject.vertices;
		Vector2[] UVs = new Vector2[Vertices.Length];
		//矩形左下角的坐标
		UVs[0].x=(Reset) ? 0.0f : AtlasUVs.x;
		UVs[0].y=(Reset) ? 0.0f : AtlasUVs.y;
		//矩形右下角的坐标
		UVs[1].x=(Reset) ? 1.0f : AtlasUVs.x+AtlasUVs.width;
		UVs[1].y=(Reset) ? 1.0f : AtlasUVs.y+AtlasUVs.height;
		//矩形左上角的坐标
		UVs[2].x=(Reset) ? 1.0f : AtlasUVs.x+AtlasUVs.width;
		UVs[2].y=(Reset) ? 0.0f : AtlasUVs.y;
		//矩形右上角的坐标
		UVs[3].x=(Reset) ? 0.0f : AtlasUVs.x;
		UVs[3].y=(Reset) ? 1.0f : AtlasUVs.y+AtlasUVs.height;
		
		//		int[] Triangles = new int[6];
		//		Triangles [0] = 3;
		//		Triangles [1] = 1;
		//		Triangles [2] = 2;
		//		Triangles [3] = 2;
		//		Triangles [4] = 1;
		//		Triangles [5] = 0;
		MeshObject.uv = UVs;
		MeshObject.vertices = Vertices;
		//MeshObject.triangles = Triangles;
		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
	}
}