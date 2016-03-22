using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

namespace NendUnityPlugin
{
	public class NendAndroidSetup : EditorWindow
	{
		static bool debuggable = false;

		[MenuItem ("NendSDK/Android Setup", false, 1)]
		public static void MenuItemAndroidSetup ()
		{
			NendAndroidSetup window = (NendAndroidSetup)EditorWindow.GetWindow (typeof(NendAndroidSetup));
			Vector2 vec2 = new Vector2 (200, 100);
			window.minSize = vec2;
			window.maxSize = vec2;
			window.Show ();
		}

		void OnGUI ()
		{
			debuggable = EditorGUILayout.Toggle ("Debuggable", debuggable);
			if (GUILayout.Button ("Configure")) {
				DoSetup ();
			}
		}

		public static void DoSetup ()
		{
			Debug.Log ("Processing...");
			if (PrecheckForSetup ()) {
				string androidSDKPath = EditorPrefs.GetString ("AndroidSdkRoot");
				Debug.Log ("AndroidSDK path: " + androidSDKPath);
				CreateAndroidLibraryDirectoryIfNeeded ();
				AddGooglePlayServicesLibrary (androidSDKPath);
				ConfigureAndroidManifest (androidSDKPath);
				AssetDatabase.Refresh ();
			}
			Debug.Log ("Done!");
		}

		private static bool PrecheckForSetup ()
		{
			string androidSDKPath = EditorPrefs.GetString ("AndroidSdkRoot", null);
			if (string.IsNullOrEmpty (androidSDKPath) || !Directory.Exists (androidSDKPath)) {
				Debug.LogWarning ("AndroidSdkRoot is not setup.");
				return false;
			}
			string libraryPath = Path.Combine (androidSDKPath, ToPlatformDirectorySeparator ("extras/google/google_play_services/libproject/google-play-services_lib"));
			if (!Directory.Exists (libraryPath)) {
				Debug.LogWarning ("The Google Play services library project is not installed.");
				return false;
			}
			return true;
		}

		private static string ToPlatformDirectorySeparator (string path)
		{
			return path.Replace ("/", Path.DirectorySeparatorChar.ToString ());
		}

		private static void CreateAndroidLibraryDirectoryIfNeeded ()
		{
			string path = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator ("Plugins/Android"));
			if (!Directory.Exists (path)) {
				Directory.CreateDirectory (path);
				Debug.Log ("Created: 'Plugins/Android' directory.");
			} else {
				Debug.Log ("'Plugins/Android' directory is already exist.");
			}
		}

		private static void AddGooglePlayServicesLibrary (string androidSDKPath)
		{
			string libraryPath = Path.Combine (androidSDKPath, ToPlatformDirectorySeparator ("extras/google/google_play_services/libproject/google-play-services_lib/libs/google-play-services.jar"));
			if (File.Exists (libraryPath)) {
				string libraryPathDest = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator ("Plugins/Android/google-play-services.jar"));
				if (File.Exists (libraryPathDest)) {
					FileUtil.DeleteFileOrDirectory (libraryPathDest);
					Debug.Log ("Delete: old google-play-services.jar");
				}
				FileUtil.CopyFileOrDirectory (libraryPath, libraryPathDest);
				Debug.Log ("Added: google-play-services.jar");
			} else {
				Debug.LogWarning ("The Google Play services library is not installed.");
			}
		}

		private static void ConfigureAndroidManifest (string androidSDKPath)
		{
			string manifestPathDest = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator ("Plugins/Android/AndroidManifest.xml"));
			if (!File.Exists (manifestPathDest)) {
				string[] UnityAndroidManifestPathList = {
					Path.Combine (EditorApplication.applicationPath, ToPlatformDirectorySeparator ("../PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml")),
					Path.Combine (EditorApplication.applicationContentsPath, ToPlatformDirectorySeparator ("PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml")),
					Path.Combine (EditorApplication.applicationContentsPath, ToPlatformDirectorySeparator ("PlaybackEngines/AndroidPlayer/AndroidManifest.xml"))
				};
					
				string defaultManifestPath = null;
				foreach (string path in UnityAndroidManifestPathList) {
					if (File.Exists (path)) {
						defaultManifestPath = path;
						Debug.Log ("Found AndroidManifest at " + path);
						break;
					}
				}
				if (null == defaultManifestPath) {
					Debug.LogWarning ("Couldn't find default AndroidManifest.");
					return;
				}
				FileUtil.CopyFileOrDirectory (defaultManifestPath, manifestPathDest);
			} else {
				Debug.Log ("The AndroidManifest is already exist.");
			}
		
			XmlDocument doc = new XmlDocument ();
			doc.Load (manifestPathDest);
		
			XmlNode applicationNode = doc.SelectSingleNode ("manifest/application");
			if (null == applicationNode) {
				Debug.LogWarning ("The application tag is not found.");
				return;
			}
		
			string ns = applicationNode.GetNamespaceOfPrefix ("android");
			XmlNamespaceManager nsManager = new XmlNamespaceManager (doc.NameTable);
			nsManager.AddNamespace ("android", ns);
		
			XmlElement gmsVersionElement = CreateGooglePlayServicesVersionElement (doc, ns, androidSDKPath);
			if (null != gmsVersionElement) {
				XmlNode gmsVersionNode = GetChildNode (applicationNode, @"//meta-data[@android:name='com.google.android.gms.version']", nsManager);
				if (null != gmsVersionNode) {
					applicationNode.RemoveChild (gmsVersionNode);
					Debug.Log ("Removed: " + gmsVersionNode.OuterXml);
				}
				applicationNode.AppendChild (gmsVersionElement);
				Debug.Log ("Added: " + gmsVersionElement.OuterXml);
			}
		
			if (!SearchChildNode (doc, @"/manifest/uses-permission[@android:name='android.permission.INTERNET']", nsManager)) {
				XmlElement element = CreateInternetPermissionElement (doc, ns);
				doc.DocumentElement.AppendChild (element);
				Debug.Log ("Added: " + element.OuterXml);
			} else {
				Debug.Log ("Found: 'android.permission.INTERNET'");
			}

			XmlNode nendDebuggableNode = GetChildNode (applicationNode, @"//meta-data[@android:name='NendDebuggable']", nsManager);
			if (null != nendDebuggableNode) {
				XmlElement element = (XmlElement)nendDebuggableNode;
				element.SetAttribute ("value", ns, debuggable.ToString ().ToLower ());
				Debug.Log ("Modified: " + element.OuterXml);
			} else if (debuggable) { 
				XmlElement element = CreateNendDebuggableElement (doc, ns);
				applicationNode.AppendChild (element);
				Debug.Log ("Added: " + element.OuterXml);
			} else {
				Debug.Log ("No need to create a NendDebuggable element.");
			}

			EnableForwardNativeEventsToDalvikIfNeeded (applicationNode, nsManager, ns);
		
			doc.Save (manifestPathDest);
		}

		private static bool SearchChildNode (XmlNode parentNode, string xpath, XmlNamespaceManager nsManager)
		{
			XmlNodeList nodes = parentNode.SelectNodes (xpath, nsManager);
			return null != nodes && 0 < nodes.Count;
		}

		private static XmlNode GetChildNode (XmlNode parentNode, string xpath, XmlNamespaceManager nsManager)
		{
			return parentNode.SelectSingleNode (xpath, nsManager);
		}

		private static XmlElement CreateGooglePlayServicesVersionElement (XmlDocument doc, string ns, string androidSDKPath)
		{
			string version = GetGooglePlayServicesVersion (androidSDKPath);
			if (!string.IsNullOrEmpty (version)) {
				XmlElement element = doc.CreateElement ("meta-data");
				element.SetAttribute ("name", ns, "com.google.android.gms.version");
				element.SetAttribute ("value", ns, GetGooglePlayServicesVersion (androidSDKPath));
				return element;
			} else {
				return null;
			}
		}

		private static XmlElement CreateInternetPermissionElement (XmlDocument doc, string ns)
		{
			XmlElement element = doc.CreateElement ("uses-permission");
			element.SetAttribute ("name", ns, "android.permission.INTERNET");
			return element;
		}

		private static XmlElement CreateNendDebuggableElement (XmlDocument doc, string ns)
		{
			XmlElement element = doc.CreateElement ("meta-data");
			element.SetAttribute ("name", ns, "NendDebuggable");
			element.SetAttribute ("value", ns, "true");
			return element;
		}

		private static string GetGooglePlayServicesVersion (string androidSDKPath)
		{
			string xmlPath = Path.Combine (androidSDKPath, ToPlatformDirectorySeparator ("extras/google/google_play_services/libproject/google-play-services_lib/res/values/version.xml"));
			if (File.Exists (xmlPath)) {
				XmlDocument doc = new XmlDocument ();
				doc.Load (xmlPath);
				XmlNode node = doc.SelectSingleNode (@"/resources/integer[@name='google_play_services_version']");
				if (null != node) {
					return node.InnerText;
				} else {
					Debug.LogWarning ("Couldn't find 'google_play_services_version'.");
				}
			} else {
				Debug.LogWarning ("Couldn't find 'version.xml'.");
			}
			return "";
		}

		private static void EnableForwardNativeEventsToDalvikIfNeeded (XmlNode applicationNode, XmlNamespaceManager nsManager, string ns)
		{
#if UNITY_4_6
			XmlNodeList nodes = applicationNode.SelectNodes(@"//activity/meta-data[@android:name='unityplayer.ForwardNativeEventsToDalvik']", nsManager);
			foreach (XmlNode node in nodes) {
				if (node is XmlElement) {
					XmlElement element = (XmlElement)node;
					if (!"true".Equals(element.GetAttribute("value", ns))) {
						Debug.Log("ForwardNativeEventsToDalvik >> true");
						element.SetAttribute("value", ns, "true");
					}
				}
			}
#endif
		}
	}
}
