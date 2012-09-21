using UnityEngine;
using System.Collections;


[System.Serializable]
public class CachedString
{
	public string id;
	public Texture2D cachedImage;
}

public class LocalizedCachedStringAsTexture : MonoBehaviour {
	public CachedString[] _cachedStrings;
	protected Hashtable _hashtable;
	public Texture2D _defaultImage;
	public static LocalizedCachedStringAsTexture _instance;

	void Start () {
		_instance = this;
		_hashtable = new Hashtable();
		string language = Application.systemLanguage.ToString();
		//Now populate the textures
		for(int i = 0; i < _cachedStrings.Length; i++)
		{
			CachedString cs = _cachedStrings[i];
			cs.cachedImage = (Texture2D)Resources.Load("image_label_" + cs.id + "_" + language);
			_hashtable.Add(cs.id,cs);
		}
	}
	
	public static LocalizedCachedStringAsTexture getInstance()
	{
		return _instance;
	}
	
	public Texture2D getCachedString(string key)
	{
		Texture2D tex = _defaultImage;
		key = key.ToLower();
		if(_hashtable.ContainsKey(key))
		{
			CachedString cs = (CachedString)_hashtable[key];
			if(cs != null)
			{
				tex = cs.cachedImage;	
			}
		}
		return tex;
	}
}
