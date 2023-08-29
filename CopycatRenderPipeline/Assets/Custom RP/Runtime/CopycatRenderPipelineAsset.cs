using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CopycatRenderPipelineAsset")]
public class CopycatRenderPipelineAsset : RenderPipelineAsset
{

    protected override RenderPipeline CreatePipeline()
    {
        return new CopycatRenderPipeline();
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
