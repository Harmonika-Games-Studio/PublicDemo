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
        ApplyCustomizations();
    }

    private void ApplyCustomizations()
    {
        // Load JSON from Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("gameConfig");
        if (jsonFile == null)
        {
            Debug.LogWarning("Config JSON not found in Resources");
            return;
        }

        GameConfig config = JsonUtility.FromJson<GameConfig>(jsonFile.text);
        ApplyColor(_sphere, config.colorSphere);
        ApplyColor(_prism, config.colorPrism);
        ApplyColor(_square, config.colorSquare);

        // Load image from Resources
        Texture2D texture = LoadImageFromResources("customImage");
        if (texture != null)
        {
            Sprite customSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            _image.sprite = customSprite;
        }
        else
        {
            Debug.LogWarning("Custom image not found in Resources");
        }
    }

    private Texture2D LoadImageFromResources(string fileName)
    {
        TextAsset imageFile = Resources.Load<TextAsset>(fileName);
        if (imageFile == null)
        {
            return null;
        }

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageFile.bytes);
        return texture;
    }

    private void ApplyColor(Renderer renderer, string colorHex)
    {
        if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning($"Invalid color: {colorHex}");
        }
    }

    [System.Serializable]
    private class GameConfig
    {
        public string colorSphere;
        public string colorPrism;
        public string colorSquare;
    }
}
