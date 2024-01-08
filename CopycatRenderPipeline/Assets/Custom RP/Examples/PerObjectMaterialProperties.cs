using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerObjectMaterialProperties : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField]
    
    private Color baseColor = Color.white;
    
    static MaterialPropertyBlock block;
    
    /*OnValidate gets invoked in the Unity editor when the component is loaded or changed. So each time the scene is loaded and when we edit the component.
    Thus, the individual colors appear immediately and respond to edits.*/
    private void OnValidate()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }

        block.SetColor(baseColorId, baseColor);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
    
    void Awake () {
        OnValidate();
    }

}
