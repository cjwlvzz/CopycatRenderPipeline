using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private const string bufferName = "Render Camera";
    
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit") , litShaderTagId = new ShaderTagId("CustomLit");
    
    CommandBuffer buffer = new CommandBuffer
    {
        name = bufferName
    };

    ScriptableRenderContext context;

    CullingResults cullingResults;

    Camera camera;

    private Lighting _lighting = new Lighting();

    public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
    {
        this.context = context;
        this.camera = camera;
        
        PrepareBuffer();
        
        //must be called before culling
        PrepareForSceneWindow();
        
        if (!Cull())
        {
            return;
        }

        Setup();
        
        _lighting.Setup(context,cullingResults);

        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);

        DrawUnsupportedShaders();
        
        DrawGizmos();
        
        Submit();

    }

    void Submit()
    {

        buffer.EndSample(SampleName);

        Executebuffer();

        context.Submit();
    }

    
    //处理自定义RP不支持的shader
    

    //绘制可见的几何体
    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        var sortingSettings = new SortingSettings(camera);
        //为什么实例化drawsettings一定要使用UnlitShaderTagId
        var drawingSettings = new DrawingSettings(unlitShaderTagId,sortingSettings)
        {
            enableInstancing = useGPUInstancing,
            enableDynamicBatching = useDynamicBatching
        };
        drawingSettings.SetShaderPassName(1,litShaderTagId);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        
        context.DrawRenderers(
            cullingResults,ref drawingSettings,ref filteringSettings
            );

        context.DrawSkybox(camera);
        
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );
    }

    void Setup()
    {
        context.SetupCameraProperties(camera);

        CameraClearFlags flags = camera.clearFlags;
        
        buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth , flags <= CameraClearFlags.Color , flags == CameraClearFlags.Color ?
            camera.backgroundColor.linear : Color.clear);
        
        //This is to inject the profiler samples
        buffer.BeginSample(SampleName);
        
        Executebuffer();
    }

    void Executebuffer()
    {
        context.ExecuteCommandBuffer(buffer);

        buffer.Clear();

    }

    //剔除
    bool Cull()
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }
    
    

}
