using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public RawImage IdleTrapImage;

    public void ChangeIdleTrap(Renderer TrapRenderer)
    {
        IdleTrapImage.color = TrapRenderer.sharedMaterial.color;
    }
}