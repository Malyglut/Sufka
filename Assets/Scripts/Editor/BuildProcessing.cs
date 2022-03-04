#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;

namespace Sufka.Editor
{
    public class BuildProcessing : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private const string USER_TRACKING_USAGE_FIELD = "NSUserTrackingUsageDescription";
        private const string USER_TRACKING_USAGE_DESCRIPTION = "Zebrane dane będą wykorzystane w celu wyświetlania reklam, które są bliższe Twoim zainteresowaniom.";
        
        public int callbackOrder => 0;
        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                var plistPath = report.summary.outputPath + "/Info.plist";
                
                var plistDocument  = new PlistDocument();
                
                plistDocument.ReadFromString(File.ReadAllText(plistPath));

                var elementDict = plistDocument.root;

                elementDict.SetString(USER_TRACKING_USAGE_FIELD, USER_TRACKING_USAGE_DESCRIPTION);
                
                File.WriteAllText(plistPath, plistDocument.WriteToString());
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;
        }
    }
}
#endif
