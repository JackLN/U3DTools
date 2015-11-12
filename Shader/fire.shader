Shader "Custom/fire" {
	
	CGINCLUDE    
  
        #include "UnityCG.cginc"                
        #pragma target 3.0    
        struct vertOut {    
            float4 pos:SV_POSITION;    
            float4 srcPos;   
        };  
  
        vertOut vert(appdata_base v) {  
            vertOut o;  
            o.pos = mul (UNITY_MATRIX_MVP, v.vertex);  
            o.srcPos = ComputeScreenPos(o.pos);  
            return o;  
        }  
        
        float noise(fixed3 p) //Thx to Las^Mercury
		{
			fixed3 i = floor(p);
			fixed4 a = dot(i, fixed3(1.0, 57.0, 21.0)) + fixed4(0.0, 57.0, 21.0, 78.0);
			fixed3 f = cos((p-i)*acos(-1.))*(-.5)+.5;
			a = lerp(sin(cos(a)*a),sin(cos(1.+a)*(1.+a)), f.x);
			a.xy = lerp(a.xz, a.yw, f.y);
			return lerp(a.x, a.y, f.z);
		}
		
		float sphere(fixed3 p, fixed4 spr)
		{
			return length(spr.xyz-p) - spr.w;
		}
		
		float flame(fixed3 p)
		{
			float d = sphere(p*fixed3(1.,.5,1.), fixed4(.0,-1.,.0,1.));
			return d + (noise(p+fixed3(.0,_Time.y*2.,.0)) + noise(p*3.)*.5)*.25*(p.y);
		}
		
		float scene(fixed3 p)
		{
			return min(100.-length(p) , abs(flame(p)) );
		}
		
		fixed4 raymarch(fixed3 org, fixed3 dir)
		{
			float d = 0.0, glow = 0.0, eps = 0.02;
			fixed3  p = org;
			bool glowed = false;
	
			for(int i=0; i<64; i++)
			{		
				d = scene(p) + eps;
				p += d * dir;
				if( d>eps )
				{
					if(flame(p) < .0)
					glowed=true;
				if(glowed)
       				glow = float(i)/64.;
				}
			}
			return fixed4(p,glow);
		}
  
        fixed4 frag(vertOut i) : COLOR0 {  
        
	        fixed2 v = -1.0 + 2.0 * i.srcPos.xy / i.srcPos.w;
			//v.x *= i.srcPos.x/i.srcPos.y;
		
			fixed3 org = fixed3(0., -2., 4.);
			fixed3 dir = normalize(fixed3(v.x*1.6, -v.y, -1.5));
		
			fixed4 p = raymarch(org, dir);
			float glow = p.w;
		
			fixed4 col = lerp(fixed4(1.,.5,.1,1.), fixed4(0.1,.5,1.,1.), p.y*.02+.4);
		
			return lerp(fixed4(0.), col, pow(glow*2.,4.));



//            fixed3 COLOR1 = fixed3(0.0,0.0,0.3);  
//            fixed3 COLOR2 = fixed3(0.5,0.0,0.0);  
//            float BLOCK_WIDTH = 0.03;  
//  
//            float2 uv = (i.srcPos.xy/i.srcPos.w);  
//  
//            // To create the BG pattern  
//            fixed3 final_color = fixed3(1.0);  
//            fixed3 bg_color = fixed3(0.0);  
//            fixed3 wave_color = fixed3(0.0);  
//  
//            float c1 = fmod(uv.x, 2.0* BLOCK_WIDTH);  
//            c1 = step(BLOCK_WIDTH, c1);  
//            float c2 = fmod(uv.y, 2.0* BLOCK_WIDTH);  
//            c2 = step(BLOCK_WIDTH, c2);  
//            bg_color = lerp(uv.x * COLOR1, uv.y * COLOR2, c1*c2);  
//  
//            // TO create the waves   
//            float wave_width = 0.01;  
//            uv = -1.0 + 2.0*uv;  
//            uv.y += 0.1;  
//            for(float i=0.0; i<10.0; i++) {  
//                uv.y += (0.07 * sin(uv.x + i/7.0 +  _Time.y));  
//                wave_width = abs(1.0 / (150.0 * uv.y));  
//                wave_color += fixed3(wave_width * 1.9, wave_width, wave_width * 1.5);  
//            }  
//            final_color = bg_color + wave_color;  
//  
            //return fixed4(1.0,0.0,0.0, 1.0);  
        }  
  
    ENDCG    
  
    SubShader {    
        Pass {    
            CGPROGRAM    
  
            #pragma vertex vert    
            #pragma fragment frag    
            #pragma fragmentoption ARB_precision_hint_fastest     
  
            ENDCG    
        }    
  
    }     
    FallBack Off   
}



















