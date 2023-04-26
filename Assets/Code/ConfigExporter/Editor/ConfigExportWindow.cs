using System.IO;
using UnityEditor;
using UnityEngine;

namespace Code.ConfigExporter.Editor
{
    public class ConfigExportWindow : EditorWindow
    {
        private const string CopyPathSaveKey = "ConfigExportCopyPathSaveKey";
        private const string OriginPathSaveKey = "ConfigExportOriginPathSaveKey";
        private string m_ConfigCsExportDirectory;
        private string m_ConfigCsInitFile;
        private string m_ConfigCsTemplate;
        private string m_CopyPath;
        private string m_OriginExcelPath;
        private string m_TargetPath;

        private void OnEnable()
        {
            m_OriginExcelPath = EditorPrefs.GetString(OriginPathSaveKey, "D:\\Config");
            m_CopyPath = EditorPrefs.GetString(CopyPathSaveKey, "D:\\TempConfig");
            //Modify the following path per project
            m_TargetPath = Application.dataPath + "/Config/";
            m_ConfigCsTemplate = Application.dataPath + "/Code/ConfigExporter/Editor/ConfigTemplate.cs";
            m_ConfigCsExportDirectory = Application.dataPath + "/Code/ConfigExporter/Runtime/Configuration/";
            m_ConfigCsInitFile = Application.dataPath + "/Code/ConfigExporter/Runtime/ConfigInitHandler.cs";
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Excel路径： " + m_OriginExcelPath, MessageType.Info);
            EditorGUILayout.HelpBox("临时拷贝路径： " + m_CopyPath, MessageType.Info);
            EditorGUILayout.HelpBox("目标路径： " + m_TargetPath, MessageType.Info);
            EditorGUILayout.HelpBox("配置表模板路径： " + m_ConfigCsTemplate, MessageType.Info);
            EditorGUILayout.HelpBox("配置表导出路径： " + m_ConfigCsExportDirectory, MessageType.Info);
            EditorGUILayout.HelpBox("配置表初始化代码： " + m_ConfigCsInitFile, MessageType.Info);
            {
	            var newPath = EditorGUILayout.TextField("Excel配置表源路径", m_OriginExcelPath);
	            if (newPath != m_OriginExcelPath)
	            {
		            m_OriginExcelPath = newPath;
		            EditorPrefs.SetString(OriginPathSaveKey, newPath);
	            }

	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("打开以指定"))
	            {
		            newPath = EditorUtility.OpenFolderPanel("选择配置表源路径", m_OriginExcelPath, "");

		            if (!string.IsNullOrEmpty(newPath) && Directory.Exists(newPath))
		            {
			            m_OriginExcelPath = newPath;
			            EditorPrefs.SetString(OriginPathSaveKey, m_OriginExcelPath);
		            }
	            }
            
	            if (GUILayout.Button("打开目标目录"))
	            {
		            if (Directory.Exists(m_OriginExcelPath))
			            Application.OpenURL(m_OriginExcelPath);
		            else
			            Debug.LogError("Target directory is not exist! : " + m_OriginExcelPath);
	            }
	            EditorGUILayout.EndHorizontal();
            }
            {
	            var newPath = EditorGUILayout.TextField("Excel配置表拷贝路径", m_CopyPath);
	            if (newPath != m_CopyPath)
	            {
		            m_CopyPath = newPath;
		            EditorPrefs.SetString(CopyPathSaveKey, newPath);
	            }

            
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("打开以指定"))
	            {
		            newPath = EditorUtility.OpenFolderPanel("选择配置表拷贝路径", m_CopyPath, "");

		            if (!string.IsNullOrEmpty(newPath) && Directory.Exists(newPath))
		            {
			            m_CopyPath = newPath;
			            EditorPrefs.SetString(CopyPathSaveKey, m_CopyPath);
		            }
	            }
            
	            if (GUILayout.Button("打开目标目录"))
	            {
		            if (Directory.Exists(m_CopyPath))
			            Application.OpenURL(m_CopyPath);
		            else
			            Debug.LogError("Target directory is not exist! : " + m_CopyPath);
	            }
	            EditorGUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button("导表"))
            {
                new Code.ConfigExporter.Editor.ConfigExporter(m_OriginExcelPath, m_CopyPath, m_TargetPath, "{0}.bytes",
                        m_ConfigCsTemplate, m_ConfigCsExportDirectory, m_ConfigCsInitFile,
                        msg => { ShowNotification(new GUIContent(msg)); })
                    .Export();
            }
        }

        [MenuItem("QGame/Export config")]
        public static void OpenWindow()
        {
            var window = GetWindow<ConfigExportWindow>("导表工具");
            window.Show();
        }
    }
}