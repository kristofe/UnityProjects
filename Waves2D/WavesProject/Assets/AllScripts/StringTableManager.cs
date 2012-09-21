using UnityEngine;
using System.Collections;

public class StringTable {
	private static Hashtable _hashtable;
	private static string _language;
	
	/* Going to support the following languages
	 * English
	 * Chinese
	 * Korean
	 * Japanese
	 * German
	 * French
	 * Spanish
	 * Portugese
	 * Russian?
	 * Turkish?
	 */
	
	public static void loadFile(string language = "") {
		_hashtable = new Hashtable();
		_language = language;
		Debug.Log("StringTableManager: systemLanguage = " + Application.systemLanguage.ToString());
		
		string fileName = "StringTable";
		if(language.Length == 0)
		{
			_language = Application.systemLanguage.ToString();
			switch(Application.systemLanguage)
			{
			case SystemLanguage.Chinese:
				fileName += "_CHINESE";
				break;
				
			case SystemLanguage.Korean:
				fileName += "_KOREAN";
				break;
				
			case SystemLanguage.Japanese:
				fileName += "_JAPANESE";
				break;
				
			case SystemLanguage.German:
				fileName += "_GERMAN";
				break;
				
			case SystemLanguage.French:
				fileName += "_FRENCH";
				break;
			
			case SystemLanguage.Spanish:
				fileName += "_SPANISH";
				break;
				
			case SystemLanguage.Portuguese:
				fileName += "_PORTUGUESE";
				break;
				
			case SystemLanguage.English:	
				fileName += "_ENGLISH";
				break;
			default:
				fileName += "_ENGLISH";
				break;
			}
		}
		else
		{
			fileName += "_" + language.ToUpper();	
		}
		//fileName = "StringTable_GERMAN";
		Debug.Log("Loading: " + fileName);
		TextAsset taStringTable = Resources.Load(fileName) as TextAsset;

		StringTable.processFile(taStringTable.text);
		
	}
	
	
	private static void processFile(string txt)
	{
		string[] lines = txt.Split('\n');

		StringTable._hashtable.Clear();
		
		foreach(string line in lines)
		{
			string l = line.Trim();
			string[] fields = l.Split('\t');
			
			if(fields.Length < 2)
				continue;
			string key = fields[0];
			
			StringTable._hashtable.Add(key.ToLower(),fields[1]);
		}	
	}
	
	public static string getString(string key)
	{
		string str = key;
		key = key.ToLower();
		if(StringTable._hashtable.ContainsKey(key))
			str = (string)StringTable._hashtable[key];
		return str;
	}
	
	
}
