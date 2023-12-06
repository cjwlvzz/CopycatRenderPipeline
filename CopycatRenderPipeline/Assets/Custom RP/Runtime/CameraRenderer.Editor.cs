using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

//partial class CameraRenderer,implement the DrawUnsupportedShaders method
partial class CameraRenderer
{

    partial void DrawUnsupportedShaders();
    
    partial void DrawGizmos();

    partial void PrepareForSceneWindow();
    
    partial void PrepareBuffer();
    
    #if UNITY_EDITOR
        //the error material is used to render any shader that is not supported by this Render Pipeline
        private static Material errorMaterial;
        
        string SampleName { get; set; }

        static ShaderTagId[] legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        //process any shaders that are not supported by this Render Pipeline
    
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
        
        /*
        在为场景窗口进行渲染时，我们必须以相机为参数调用 ScriptableRenderContext.EmitWorldGeometryForSceneView 来显式地将用户界面添加到世界几何体中
        当场景相机的 cameraType 属性等于 CameraType.SceneView 时，我们将使用场景相机进行渲染。
        https://docs.unity3d.com/ScriptReference/CameraType.html，CameraType.SceneView,是用于渲染场景视图的相机，在editor模式下，场景窗口就是场景视图
        */
        /*
        When rendering for the scene window, we need to invoke ScriptableRenderContext.EmitWorldGeometryForSceneView with the camera as a parameter to 
        explicitly add the user interface to the world geometry. When the cameraType property of the scene camera equals CameraType.
        SceneView, we will use the scene camera for rendering.https://docs.unity3d.com/ScriptReference/CameraType.html， CameraType.
        SceneView is a camera type used for rendering the scene view, and in editor mode, the scene window represents the scene view.
         */
        partial void PrepareForSceneWindow()
        {
            
            if (camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
        }

        //makes the name of the command buffer match the name of the camera
        partial void PrepareBuffer()
        {
            Profiler.BeginSample("Editor Only");
            buffer.name = SampleName = camera.name;
            Profiler.EndSample();
        }
#else
     
    string SampleName => bufferName;

#endif
}