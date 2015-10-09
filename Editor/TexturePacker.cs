using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TexturePacker :  ScriptableWizard{

	public Texture2D[] Textures;
	public string PackageName = "TexturePackage";
	public int padding = 4;

	[MenuItem("Tools/TexturePacker")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard ("Texture Packer",typeof(TexturePacker));
	}

	public void ConfigureForPacker(string path)
	{
		TextureImporter pImporter = AssetImporter.GetAtPath (path) as TextureImporter;
		pImporter.textureType = TextureImporterType.Advanced;

		TextureImporterSettings pSetting = new TextureImporterSettings ();
		pImporter.ReadTextureSettings (pSetting);
		//only 2D false
		pSetting.mipmapEnabled = false;
		pSetting.readable = true;
		pSetting.maxTextureSize = 4096;
		pSetting.textureFormat = TextureImporterFormat.ARGB32;
		pSetting.filterMode = FilterMode.Point;
		pSetting.wrapMode = TextureWrapMode.Clamp;
		pSetting.npotScale = TextureImporterNPOTScale.None;
		pImporter.SetTextureSettings (pSetting);

		AssetDatabase.ImportAsset (path,ImportAssetOptions.ForceUpdate);
		AssetDatabase.Refresh ();
	}

	public void GeneratePackage()
	{
		GameObject pPackage = new GameObject ("obj_" + PackageName);
		PackageData pData = pPackage.AddComponent<PackageData> ();
		pData.TextureNames = new string[Textures.Length];

		for (int i = 0; i < Textures.Length; i++) 
		{
			string pTexpath = AssetDatabase.GetAssetPath(Textures[i]);
			ConfigureForPacker(pTexpath);
			pData.TextureNames[i] = pTexpath;
		}

		//total Texture
		Texture2D pTex = new Texture2D (1, 1, TextureFormat.ARGB32, false);
		pData.TextureUVs = pTex.PackTextures (Textures,padding,4096);
		string pTexAssetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/" + PackageName + ".png");
		byte[] bytes = pTex.EncodeToPNG ();
		System.IO.File.WriteAllBytes (pTexAssetPath,bytes);
		bytes = null;
		UnityEngine.Object.DestroyImmediate (pTex);
		AssetDatabase.ImportAsset (pTexAssetPath);
		pData.TotalTexture = AssetDatabase.LoadAssetAtPath (pTexAssetPath, typeof(Texture2D)) as Texture2D;
		ConfigureForPacker (AssetDatabase.GetAssetPath (pData.TotalTexture));
		pTexAssetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/data_" + PackageName + ".prefab");
		Object prefab = PrefabUtility.CreateEmptyPrefab (pTexAssetPath);
		PrefabUtility.ReplacePrefab (pPackage, prefab, ReplacePrefabOptions.ConnectToPrefab);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
		DestroyImmediate (pPackage);
	}

	void OnEnable()
	{
		List<Texture2D> pTextureList = new List<Texture2D> ();
		if(Selection.objects != null && Selection.objects.Length > 0)
		{
			Object[] pObjects = EditorUtility.CollectDependencies(Selection.objects);
			foreach(Object o in pObjects)
			{
				Texture2D pTex = o as Texture2D;
				if(pTex != null)
				{
					pTextureList.Add(pTex);
				}
			}
		}

		if (pTextureList.Count > 0) 
		{
			Textures = new Texture2D[pTextureList.Count];
			for(int i = 0; i < pTextureList.Count ; i++)
			{
				Textures[i] = pTextureList[i];
			}
		}
	}



	void OnWizardCreate()
	{
		GeneratePackage ();
	}
}
