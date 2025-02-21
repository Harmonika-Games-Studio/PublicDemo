using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;
using System;

public class CustomBuild
{
    public static void BuildWithCustomAssets()
    {
        string customJson = GetCommandLineArgument("-CUSTOM_JSON");

        if (!string.IsNullOrEmpty(customJson))
        {
            File.WriteAllText("Assets/Resources/gameConfig.json", customJson);

            // Download image if URL is provided
            GameConfig config = JsonUtility.FromJson<GameConfig>(customJson);
            if (!string.IsNullOrEmpty(config.customImageUrl))
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(config.customImageUrl, "Assets/Resources/customImage.png");
                    }
                }
                catch (System.Exception)
                {
                    // Handle image download error
                }
            }
        }

        AssetDatabase.Refresh();

        // Build process
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

    [System.Serializable]
    private class GameConfig
    {
        public string colorSphere;
        public string colorPrism;
        public string colorSquare;
        public string customImageUrl;
    }
}