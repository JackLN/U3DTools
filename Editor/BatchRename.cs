using UnityEditor;
using UnityEngine;
using System.Collections;

public class BatchRename : ScriptableWizard {

	public string BaseName = "Object_ ";
	public int StartNumber = 0;
	public int Increment = 1;

	[MenuItem("Tools/BatchRename")]

	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard ("Batch Name",typeof(BatchRename),"Rename");
	}

	void Updatehelpstring()
	{
		helpString = "";
		if (Selection.objects != null)
			helpString = "Select Object Number : " + Selection.objects.Length;
	}

	void OnEnable()
	{
		Updatehelpstring ();
	}

	void OnSelectionChange()
	{
		Updatehelpstring ();
	}

	void OnWizardCreate()
	{
		if (Selection.objects == null)
			return;
		int num = StartNumber;
		foreach (Object o in Selection.objects) {
			o.name = BaseName + num;
			num += Increment;
		}
	}
}
