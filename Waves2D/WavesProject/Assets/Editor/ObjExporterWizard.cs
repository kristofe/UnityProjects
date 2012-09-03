using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
 
class ObjExporterWizard : ScriptableWizard
{
    public MeshFilter meshFilter;
    public string filename;
    public bool append;

    [MenuItem("CustomTools/Export Model as OBJ")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Export", typeof(ObjExporterWizard));

        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard("Create Light", typeof(WizardCreateLight));
    }

    void OnWizardCreate()
    {
        ObjExporterScript.MeshToFile(meshFilter, filename, append);
    }

    void OnWizardUpdate()
    {
    	filename =  Application.dataPath + "/"+meshFilter.name + ".obj";
        helpString = "Please Choose a MeshFilter";
    }

}
 
public class ObjExporterScript {
 
    public static string MeshToString(MeshFilter mf) {
        Mesh m = mf.sharedMesh;
        Material[] mats = mf.renderer.sharedMaterials;
        
        StringBuilder sb = new StringBuilder();
        
        sb.Append("g ").Append(mf.name).Append("\n");
        foreach(Vector3 v in m.vertices) {
            sb.Append(string.Format("v {0} {1} {2}\n",v.x,v.y,v.z));
        }
        sb.Append("\n");
        foreach(Vector3 v in m.normals) {
            sb.Append(string.Format("vn {0} {1} {2}\n",v.x,v.y,v.z));
        }
        sb.Append("\n");
        foreach(Vector2 v in m.uv) {
            sb.Append(string.Format("vt {0} {1}\n",v.x,v.y));
        }
        sb.Append("\n");
        foreach(Vector2 v in m.uv1) {
            sb.Append(string.Format("vt1 {0} {1}\n",v.x,v.y));
        }
        sb.Append("\n");
        foreach(Vector2 v in m.uv2) {
            sb.Append(string.Format("vt2 {0} {1}\n",v.x,v.y));
        }
        sb.Append("\n");
        foreach(Color c in m.colors) {
            sb.Append(string.Format("vc {0} {1} {2} {3}\n",c.r,c.g,c.b,c.a));
        }
        for (int material=0; material < m.subMeshCount; material ++) {
            sb.Append("\n");


           	Material mat = mats[material];
           	if(mat != null){
           		sb.Append("usemtl ").Append(mat.name).Append("\n");
           		sb.Append("usemap ").Append(mat.name).Append("\n");
           	}
        	
            int[] triangles = m.GetTriangles(material);
            for (int i=0;i<triangles.Length;i+=3) {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", 
                    triangles[i]+1, triangles[i+1]+1, triangles[i+2]+1));
            }
        }
        return sb.ToString();
    }
    
    public static void MeshToFile(MeshFilter mf, string filename, bool append)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(filename, append)) 
            {
                sw.WriteLine(MeshToString(mf));
            }
        }
        catch (System.Exception e)
        {
        	Debug.LogError(e);
        }
    }
}