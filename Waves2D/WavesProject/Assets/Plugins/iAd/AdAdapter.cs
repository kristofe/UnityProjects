using UnityEngine;


public class AdAdapter : MonoBehaviour
{
	public bool bannerOnBottom = true;
	
	
	public void Start()
	{
		// start up iAd and destroy ourself
		AdBinding.createAdBanner( bannerOnBottom );

		Destroy( gameObject );
	}
}
