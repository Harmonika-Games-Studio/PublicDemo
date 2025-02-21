using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;

public class CustomBuild
{
    public static void BuildWithCustomAssets()
    {
        string customJson = GetCommandLineArgument("-customJson");

        Debug.Log("=== STARTING CUSTOM BUILD ===");

        // Debug all received arguments
        string[] args = System.Environment.GetCommandLineArgs();
        foreach (string arg in args)
        {
            Debug.Log("Received argument: " + arg);
        }

        // Ensure JSON is not empty
        if (string.IsNullOrEmpty(customJson))
        {
            Debug.LogError("customJson argument is empty or missing!");
            return;
        }

        Debug.Log("Received customJson: " + customJson);

        // Ensure the Resources directory exists
        string resourcesPath = "Assets/Resources/";
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        // Save JSON to file
        File.WriteAllText(Path.Combine(resourcesPath, "gameConfig.json"), customJson);
        Debug.Log("Game config saved at: " + Path.Combine(resourcesPath, "gameConfig.json"));

        // Parse JSON and attempt to download the image
        try
        {
            GameConfig config = JsonUtility.FromJson<GameConfig>(customJson);
            Debug.Log("JSON parsed successfully!");

            if (!string.IsNullOrEmpty(config.customImageUrl))
            {
                using (WebClient client = new WebClient())
                {
                    byte[] imageData = client.DownloadData(config.customImageUrl);
                    string imagePath = Path.Combine(resourcesPath, "customImage.bytes");
                    File.WriteAllBytes(imagePath, imageData);
                    Debug.Log("Custom image downloaded successfully at: " + imagePath);
                }

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error processing customJson: " + e.Message);
        }

        AssetDatabase.Refresh();

        // Ensure the Build directory exists
        string buildPath = "Builds/Android/";
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }

        // Execute the build
        BuildPipeline.BuildPlayer(
            new[] { "Assets/_Harmonika/PublicDemo/PublicDemo.unity" },
            buildPath + "DemoBuild.apk",
            BuildTarget.Android,
            BuildOptions.None
        );

        Debug.Log("Build completed successfully!");
    }

    private static string GetCommandLineArgument(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith(name + "="))
            {
                string value = args[i].Substring(args[i].IndexOf('=') + 1);
                return value.Trim('"').Trim();
            }
            else if (args[i] == name && i + 1 < args.Length)
            {
                return args[i + 1].Trim();
            }
        }
        return null;
    }

    [System.Serializable]
    private class GameConfig
    {
        public string colorSphere;
        public string colorPrism;
        public string colorSquare;
        public string customImageUrl;
    }
}