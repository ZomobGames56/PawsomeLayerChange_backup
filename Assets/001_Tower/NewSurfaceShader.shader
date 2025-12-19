Shader "Custom/LitInstanced"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // ✅ Enable GPU Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling

            // ✅ URP lighting
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // =====================
            // Structs
            // =====================
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
                float2 uv          : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            // =====================
            // Instancing Buffer
            // =====================
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
            UNITY_INSTANCING_BUFFER_END(Props)

            // =====================
            // Textures
            // =====================
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            // =====================
            // Vertex
            // =====================
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionWS = worldPos;
                OUT.positionHCS = TransformWorldToHClip(worldPos);

                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;

                return OUT;
            }

            // =====================
            // Fragment
            // =====================
            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                float3 normalWS = normalize(IN.normalWS);

                // ✅ Get URP main directional light
                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(normalWS, mainLight.direction));
                float3 diffuse = mainLight.color * NdotL;

                // ✅ Sample texture with alpha
                float4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                // ✅ Combine with instanced color
                float3 baseColor = texColor.rgb * UNITY_ACCESS_INSTANCED_PROP(Props, _BaseColor).rgb;
                float alpha = texColor.a * UNITY_ACCESS_INSTANCED_PROP(Props, _BaseColor).a;

                float3 finalColor = baseColor * (diffuse + 0.1); // +0.1 = ambient

                return half4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
}
