Shader "2 Texture Blend" { 
   Properties { 
      _MainTex ("Base (RGB)", 2D) = "white" 
      _EnvTex ("Blend (RGB)", 2D) = "white" 
   } 
   SubShader { 
      Pass { 
      	 BindChannels { 
            Bind "Vertex", vertex
            //Bind "Normals", normal 
            Bind "texcoord", texcoord0 // main uses 1st uv 
            Bind "texcoord1", texcoord1  // decal uses 2nd uv 
         } 
         Cull Off
         //Blend One One 
         SetTexture[_MainTex] 
          { 
             //constantColor(1,1,1,1) 
             constantColor(0,0,0,1.0)
             Combine texture// + constant
            
          } 
          
          SetTexture[_EnvTex]     
          { 
          	//constantColor(0.05,0.05,0.05,1)
          	Combine texture + previous , texture * previous
            //Combine texture * previous, texture * constant 
          }  
      } 
   } 
}
