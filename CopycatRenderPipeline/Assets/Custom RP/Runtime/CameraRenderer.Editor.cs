using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

partial class CameraRenderer
{

    partial void DrawUnsupportedShaders();
    
    #if UNITY_EDITOR
        //错误材质
        private static Material errorMaterial;

        static ShaderTagId[] legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

    //处理本RP不支持的Shader
        partial void DrawUnsupportedShaders()
        {
            if (errorMaterial == null)
            {
                errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }

            var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
            {
                //对于不支持的shader，使用错误材质
                overrideMaterial = errorMaterial
            };

            for (int i = 1; i < legacyShaderTagIds.Length; i++)
            {
                drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
            }

            var filteringSettings = FilteringSettings.defaultValue;
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        }

        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                context.DrawGizmos(camera,GizmoSubset.PreImageEffects);
                context.DrawGizmos(camera,GizmoSubset.PostImageEffects);
            }
        }

        partial void DrawGizmos();

#endif
}