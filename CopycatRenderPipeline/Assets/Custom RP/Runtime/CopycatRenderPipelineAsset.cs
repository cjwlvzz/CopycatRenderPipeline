using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CopycatRenderPipelineAsset")]
public class CopycatRenderPipelineAsset : RenderPipelineAsset
{

    [SerializeField]
    bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CopycatRenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher);
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
