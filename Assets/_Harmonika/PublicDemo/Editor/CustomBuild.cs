using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomBuild
{
    public static void BuildWithCustomAssets()
    {
        string customColorSphereHex = GetCommandLineArgument("-CUSTOM_COLOR_SPHERE");
        string customColorPrismHex = GetCommandLineArgument("-CUSTOM_COLOR_PRISM");
        string customColorSquareHex = GetCommandLineArgument("-CUSTOM_COLOR_SQUARE");
        string customImagePath = GetCommandLineArgument("-CUSTOM_IMAGE_PATH");

        if (!string.IsNullOrEmpty(customColorSphereHex))
            PlayerPrefs.SetString("customColorSphere", customColorSphereHex);
        else
            PlayerPrefs.SetString("customColorSphere", "#FFFFFF");

        if (!string.IsNullOrEmpty(customColorPrismHex))
            PlayerPrefs.SetString("customColorPrism", customColorPrismHex);
        else
            PlayerPrefs.SetString("customColorPrism", "#FFFFFF");

        if (!string.IsNullOrEmpty(customColorSquareHex))
            PlayerPrefs.SetString("customColorSquare", customColorSquareHex);
        else
            PlayerPrefs.SetString("customColorSquare", "#FFFFFF");

        PlayerPrefs.Save();

        // Carregar e salvar a imagem nos Resources para runtime
        if (!string.IsNullOrEmpty(customImagePath) && File.Exists(customImagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(customImagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            string outputPath = "Assets/Resources/CustomImage.png";
            File.WriteAllBytes(outputPath, texture.EncodeToPNG());

            AssetDatabase.ImportAsset(outputPath);
            Debug.Log($"Imagem personalizada salva em: {outputPath}");
        }
        else
        {
            Debug.LogWarning($"Imagem personalizada não encontrada no caminho: {customImagePath}");
        }

        // Agora faz a build normalmente
        BuildPipeline.BuildPlayer(
            new[] { "Assets/_Harmonika/PublicDemo/PublicDemo.unity" },
            "Builds/Android/DemoBuild.apk",
            BuildTarget.Android,
            BuildOptions.None
        );
    }

    private static string GetCommandLineArgument(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}