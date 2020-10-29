using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public RawImage IdleTrapImage;

    public void ChangeIdleTrap(Renderer TrapRenderer)
    {
        IdleTrapImage.color = TrapRenderer.sharedMaterial.color;
    }
}
