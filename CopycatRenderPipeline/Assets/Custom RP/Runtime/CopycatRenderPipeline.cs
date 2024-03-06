using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CopycatRenderPipeline : RenderPipeline
{

    CameraRenderer renderer = new CameraRenderer();
    
    bool useDynamicBatching, useGPUInstancing;
    
    //constructor
    public CopycatRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
        GraphicsSettings.lightsUseLinearIntensity = true;
        Debug.Log("CopycatRenderPipeline constructor");
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        for(int i = 0;i < cameras.Length;i++)   
        {
            renderer.Render(context, cameras[i], useDynamicBatching, useGPUInstancing);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
