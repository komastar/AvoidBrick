using Assets.Framework.Foundation;
using Assets.Framework.Foundation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Framework.Editor
{
    public class DevAssist : MonoBehaviour
    {
        private static AppSettingModel appSetting;

        [MenuItem("DevAssist/Test")]
        public static void Test()
        {
            LoadAppSetting();
            Log.Debug(appSetting.Url);
            for (int i = 0; i < appSetting.Scenes.Length; i++)
            {
                Log.Debug(appSetting.Scenes[i]);
            }
        }

        [MenuItem("DevAssist/Build/Auto", priority = 0)]
        public static void BuildAPK()
        {
            LoadAppSetting();
            Debug.Log("APK Build Begin");
            string[] levels = new string[]
            {
            };

            var latestAssetBundleInfo = GetAssetBundleInfo();
            int bundleVersionCode = int.Parse(latestAssetBundleInfo.BundleVersionCode);
            PlayerSettings.Android.bundleVersionCode = bundleVersionCode + 1;
            latestAssetBundleInfo.BundleVersionCode = $"{bundleVersionCode + 1}";
            string latestBundleVersion = latestAssetBundleInfo.BundleVersion;
            string newBundleVersion = DateTime.Now.ToString("yyyy.MM.dd");

            var splitVersionString = latestBundleVersion.Split('.');
            if (false == int.TryParse(splitVersionString.Last(), out int buildCount))
            {
                buildCount = 1;
            }

            string latestVersion = latestBundleVersion.Substring(0, 10);
            buildCount = latestVersion == newBundleVersion ? ++buildCount : 1;

            string prevBundleVersion = latestAssetBundleInfo.BundleVersion;
            latestAssetBundleInfo.BundleVersion = $"{newBundleVersion}.{buildCount}";
            PlayerSettings.bundleVersion = latestAssetBundleInfo.BundleVersion;
#if ASSET_MANAGER_TEST
        Debug.Log("BUILD APK PASS");
#else
            string apkSavePath = $"AOS/{PlayerSettings.productName}-{PlayerSettings.bundleVersion}.apk";
            string defineSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions()
            {
                scenes = levels,
                target = BuildTarget.Android,
                locationPathName = apkSavePath
            });

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defineSymbol);
            if (0 < report.summary.totalErrors)
            {
                //  APK 빌드 실패시 BundleVersion, BundleVersionCode 복구
                Debug.Log($"Build Error Count : {report.summary.totalErrors}");
                PlayerSettings.bundleVersion = prevBundleVersion;
                PlayerSettings.Android.bundleVersionCode = bundleVersionCode;

                return;
            }
#endif

            //  APK 빌드 성공시 에셋번들 빌드 진행
            BuildAssetBundles(latestAssetBundleInfo);

            EditorUtility.RevealInFinder(apkSavePath);

            Debug.Log("APK Build End");
        }

        private static void LoadAppSetting()
        {
            string appSettingJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Framework/appsettings.json").text;
            appSetting = JsonUtility.FromJson<AppSettingModel>(appSettingJson);
        }

        private static AssetBundleModel GetAssetBundleInfo()
        {
            string url = $"{appSetting.Url}/api/ApiAssetBundle/defensquare/release";
            UnityWebRequest getRequest = UnityWebRequest.Get(url);
            var result = getRequest.SendWebRequest();
            while (!result.isDone) { }
            string resultString = result.webRequest.downloadHandler.text;

            return JsonUtility.FromJson<AssetBundleModel>(resultString);
        }

        public static void BuildAssetBundles(AssetBundleModel assetBundleModel)
        {
            Debug.Log("Build AssetBundles Begin");
            if (null == assetBundleModel)
            {
                Debug.Log("-----BUILD ONLY-----");
            }

            EditorUtility.DisplayProgressBar("Build AssetBundles", "Building...", 0.25f);

            string assetBundlesPath = "Assets/AssetBundles.bin";
            string persistentPath = $"{Application.persistentDataPath}/AssetBundles";
            if (false == Directory.Exists(assetBundlesPath))
            {
                Directory.CreateDirectory(assetBundlesPath);
            }

            if (false == Directory.Exists(persistentPath))
            {
                Directory.CreateDirectory(persistentPath);
            }

            Debug.Log("AssetBudnels Building...");
            BuildPipeline.BuildAssetBundles(
                assetBundlesPath
                , BuildAssetBundleOptions.UncompressedAssetBundle
#if UNITY_ANDROID
            , BuildTarget.Android);
#elif UNITY_STANDALONE
            , BuildTarget.StandaloneWindows);
#endif

            var files = Directory.GetFiles(assetBundlesPath);

            EditorUtility.DisplayProgressBar("Build AssetBundles", "Building...", 0.5f);
            //  copy to ignore folder
            assetBundlesPath = "Assets/.AssetBundles";
            if (false == Directory.Exists(assetBundlesPath))
            {
                Directory.CreateDirectory(assetBundlesPath);
            }

            List<IMultipartFormSection> multiPartForm = null;
            if (null != assetBundleModel)
            {
                multiPartForm = new List<IMultipartFormSection>();
                string username = string.IsNullOrEmpty(CloudProjectSettings.userName) ? "UNKNOWN" : CloudProjectSettings.userName;
                multiPartForm.Add(new MultipartFormDataSection("Name", $"UNITY_{username}"));
                multiPartForm.Add(new MultipartFormDataSection("Project", assetBundleModel.Project));
                multiPartForm.Add(new MultipartFormDataSection("Branch", assetBundleModel.Branch));
                multiPartForm.Add(new MultipartFormDataSection("BundleVersionCode", assetBundleModel.BundleVersionCode));
                multiPartForm.Add(new MultipartFormDataSection("BundleVersion", assetBundleModel.BundleVersion));
            }

            foreach (var item in files)
            {
                string ext = Path.GetExtension(item);
                if (".meta" == ext || "meta" == ext)
                {
                    continue;
                }

                string filename = Path.GetFileName(item);
                if ("" == ext)
                {
                    filename = $"{filename}";
                }

                File.Copy(item, $"{assetBundlesPath}/{filename}", true);
                File.Copy(item, $"{persistentPath}/{filename}", true);

                if (null != multiPartForm)
                {
                    AddMultipartForm(ref multiPartForm, filename, File.ReadAllBytes($"{assetBundlesPath}/{filename}"));
                }
            }

            if (null != multiPartForm)
            {
                EditorUtility.DisplayProgressBar("Build AssetBundles", "Uploading...", 0.75f);
                Debug.Log("Upload AssetBundles Begin");
                string url = $"{appSetting.Url}/api/ApiAssetBundle";
                UnityWebRequest uploadRequest = UnityWebRequest.Post(url, multiPartForm);
                var uploadResult = uploadRequest.SendWebRequest();
                while (!uploadResult.isDone) { }
                Debug.Log("Upload AssetBundles End");
            }
            else
            {
                EditorUtility.DisplayProgressBar("Build AssetBundles", "Upload PASS...", 0.75f);
                Debug.Log("Upload AssetBundles PASS");
            }

            EditorUtility.DisplayProgressBar("Build AssetBundles", "Building...", 1f);
            EditorUtility.RevealInFinder(persistentPath);

            EditorUtility.ClearProgressBar();
            Debug.Log("Build AssetBundles End");
        }

        private static void AddMultipartForm(ref List<IMultipartFormSection> multiPartForm, string filename, byte[] data)
        {
            multiPartForm.Add(new MultipartFormFileSection("Files", data, filename, "application/octet-stream"));
        }
    }
}