using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;
using System;

public class CustomBuild
{
    public static void BuildWithCustomAssets()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"build_log-{timestamp}.log");

        try
        {
            using (StreamWriter logFile = new StreamWriter(logPath, false))
            {
                string customJson = GetCommandLineArgument("-CUSTOM_JSON");

                if (!string.IsNullOrEmpty(customJson))
                {
                    File.WriteAllText("Assets/Resources/gameConfig.json", customJson);
                    logFile.WriteLine("JSON file saved at Assets/Resources/gameConfig.json");

                    // Download image if URL is provided
                    GameConfig config = JsonUtility.FromJson<GameConfig>(customJson);
                    if (!string.IsNullOrEmpty(config.customImageUrl))
                    {
                        try
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(config.customImageUrl, "Assets/Resources/customImage.bytes");
                                logFile.WriteLine("Custom image saved at Assets/Resources/customImage.bytes");
                            }
                        }
                        catch (System.Exception e)
                        {
                            logFile.WriteLine($"Error downloading image: {e.Message}");
                        }
                    }
                }
                else
                {
                    logFile.WriteLine("No custom JSON provided");
                }

                AssetDatabase.Refresh();

                // Build process
                BuildPipeline.BuildPlayer(
                    new[] { "Assets/_Harmonika/PublicDemo/PublicDemo.unity" },
                    "Builds/Android/DemoBuild.apk",
                    BuildTarget.Android,
                    BuildOptions.None
                );

                logFile.WriteLine("Build process completed successfully");
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText(logPath, $"Build failed: {ex.Message}");
        }
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