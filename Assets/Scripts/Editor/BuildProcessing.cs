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
        public int callbackOrder => 0;
        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                var plistPath = report.summary.outputPath + "/Info.plist";
                
                var plistDocument  = new PlistDocument();
                
                plistDocument.ReadFromString(File.ReadAllText(plistPath));

                var elementDict = plistDocument.root;

                elementDict.SetString("NSUserTrackingUsageDescription", "Want to see some ads kid?");
                
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
