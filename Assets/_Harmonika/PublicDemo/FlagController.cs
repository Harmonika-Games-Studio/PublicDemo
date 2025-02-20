using UnityEngine;
using UnityEngine.UI;

public class FlagController : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Renderer _sphere;
    [SerializeField] Renderer _prism;
    [SerializeField] Renderer _square;

    private void Start()
    {
        ApplyColor(_sphere, "customColorSphere");
        ApplyColor(_prism, "customColorPrism");
        ApplyColor(_square, "customColorSquare");
    }

    private void ApplyColor(Renderer renderer, string prefsKey)
    {
        string colorHex = PlayerPrefs.GetString(prefsKey, "#FFFFFF");
        if (ColorUtility.TryParseHtmlString(colorHex, out Color customColor))
        {
            renderer.material.color = customColor;
        }
        else
        {
            Debug.LogWarning($"Cor inválida: {colorHex} pra {prefsKey}");
        }
    }
}