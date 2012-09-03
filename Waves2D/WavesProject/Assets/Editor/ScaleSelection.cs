using UnityEngine;
using UnityEditor;
using System.Collections;

class ScaleSelection : ScriptableWizard
{
    public Vector3 scale = new Vector3(0.0078125f,0.0078125f,0.0078125f);
    //public bool scalePositionZ = false;
    //public bool scaleScaleZ = false;
    public bool scalePosition = true;
    public bool scaleSize = true;
    public bool topLevelOnly = false;
    public bool includePrefabs = false;

    [MenuItem("CustomTools/Scale Selection")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Scale Selection", typeof(ScaleSelection));

        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard("Create Light", typeof(WizardCreateLight));
    }

    void OnWizardCreate()
    {
        Transform[] objects;
        
        if(!topLevelOnly)
        {
        	if(includePrefabs)
        	{
        		objects = Selection.GetTransforms(SelectionMode.Deep);
        	}
        	else
        	{
        		objects = Selection.GetTransforms(SelectionMode.Deep | SelectionMode.ExcludePrefab);
        	}
        }
        else
        {
            if(includePrefabs)
        	{
        		objects = Selection.GetTransforms(SelectionMode.TopLevel);
        	}
        	else
        	{
        		objects = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        	}   	
        }
        
        for(int i = 0; i < objects.Length; i++)
        {
        	Transform t = objects[i];
        	if(scalePosition)
        	{
	        	Vector3 p = t.localPosition;
	        	//if(scalePositionZ)
	        	//{
	        	//	p = new Vector3(p.x*scale.y,p.y*scale.y,p.z*scale.z);
	        	//}
	        	//else
	        	//{
	        	//	p = new Vector3(p.x*scale,p.y*scale,p.z);	
	        	//}
	        	t.localPosition = new Vector3(p.x*scale.y,p.y*scale.y,p.z*scale.z);;
        	}
        	if(scaleSize)
        	{
        		Vector3 s = t.localScale;
        		//if(scaleScaleZ)
        		//{
        		//	s = s*scale;
        		//}
        		//else
        		//{
        		//	s = new Vector3(s.x*scale,s.y*scale,s.z);
        		//}
        		t.localScale = new Vector3(s.x*scale.y,s.y*scale.y,s.z*scale.z);;
        	}
        	
        }
        
        
    }

    void OnWizardUpdate()
    {
        helpString = "This script will scale the positions of each object selected in the heirarchy";
    }

}
