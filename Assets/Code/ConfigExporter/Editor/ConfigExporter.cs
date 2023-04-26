using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugins.Excel4Unity;
using UnityEditor;
using UnityEngine;

namespace Code.ConfigExporter.Editor
{
    public class ConfigExporterLogger
    {
        public void Log(string msg)
        {
            Debug.Log("<ConfigExport> " + msg);
        }

        public void LogWarning(string msg)
        {
            Debug.LogWarning("<ConfigExport> " + msg);
        }

        public void LogError(string msg)
        {
            Debug.LogError("<ConfigExport> " + msg);
        }
    }

    public class ConfigExporter
    {
        //下面这些常量注意要全小写
        private const string TypeInt = "int";
        private const string TypeFloat = "float";
        private const string TypeString = "string";
        private const string TypeVector2 = "vector2";
        private const string TypeVector3 = "vector3";
        private const string TypeVector2Int = "vector2int";
        private const string TypeVector2IntArray = "vector2int[]";
        private const string TypeBoolean = "bool";
        private const string TypeIntArray = "int[]";

        private readonly string m_ConfigCsExportDirectory;
        private readonly string m_ConfigCsInitFile;
        private readonly string m_ConfigCsTemplate;
        private readonly string m_CopyPath;

        private readonly string m_ExcelPath;

        private readonly Dictionary<string, PropertyExporter> m_Exporters = new Dictionary<string, PropertyExporter>
        {
            {TypeInt, new PropertyExporter(PropertyExportHandler.DoSerializeForInt, "int", "ReadInt")},
            {TypeFloat, new PropertyExporter(PropertyExportHandler.DoSerializeForFloat, "float", "ReadFloat")},
            {TypeString, new PropertyExporter(PropertyExportHandler.DoSerializeForString, "string", "ReadString")},
            {TypeVector2, new PropertyExporter(PropertyExportHandler.DoSerializeForVector2, "Vector2", "ReadVector2")},
            {TypeVector3, new PropertyExporter(PropertyExportHandler.DoSerializeForVector3, "Vector3", "ReadVector3")},
            {
                TypeVector2Int,
                new PropertyExporter(PropertyExportHandler.DoSerializeForVector2Int, "Vector2Int", "ReadVector2Int")
            },
            {TypeBoolean, new PropertyExporter(PropertyExportHandler.DoSerializeForBoolean, "bool", "ReadBoolean")},
            {
                TypeVector2IntArray,
                new PropertyExporter(PropertyExportHandler.DoSerializeForVector2IntArray, "Vector2Int[]",
                    "ReadVector2IntArray")
            },
            {TypeIntArray, new PropertyExporter(PropertyExportHandler.DoSerializeForIntArray, "int[]", "ReadIntArray")},
        };

        private readonly Action<string> m_ShowNotification;
        private readonly string m_TargetFileFormat;

        private readonly string m_TargetPath;

        public ConfigExporter(string excelPath, string copyPath, string targetPath, string targetFileFormat,
            string configCsTemplate, string configCsExportDirectory, string configCsInitFile,
            Action<string> showNotification)
        {
            m_TargetFileFormat = targetFileFormat;
            m_TargetPath = targetPath;
            m_ShowNotification = showNotification;
            m_ExcelPath = excelPath;
            m_CopyPath = copyPath;
            m_ConfigCsTemplate = configCsTemplate;
            m_ConfigCsInitFile = configCsInitFile;
            m_ConfigCsExportDirectory = configCsExportDirectory;
        }

        public void Export()
        {
            if (!CopyExcels()) return;
            ExportAllConfigs();

            Debug.Log("导表完成");
            m_ShowNotification?.Invoke("导表完成");
        }

        private bool CopyExcels()
        {
            if (!Directory.Exists(m_ExcelPath))
            {
                Debug.LogError("Fail to find target path : " + m_ExcelPath);
                return false;
            }

            MakeDirectory(m_CopyPath);
            var allFiles = Directory.GetFiles(m_ExcelPath, "*.xlsx", SearchOption.AllDirectories);
            foreach (var f in allFiles)
            {
                var fileName = Path.GetFileName(f);
                if (fileName.StartsWith("~")) continue; //WPS临时数据文件
                var tPath = m_CopyPath + "\\" + fileName;
                File.Copy(f, tPath);
            }

            return true;
        }

        private void ExportAllConfigs()
        {
            var logger = new ConfigExporterLogger();
            var sb = new StringBuilder();
            var context = new ConfigExportContext
            {
                Logger = logger,
                AllConfigNames = new List<string>(10),
                IdCache = new HashSet<int>(100),
            };
            var allFiles = Directory.GetFiles(m_CopyPath, "*.xlsx", SearchOption.AllDirectories);
            var configSerializer = new ConfigSerializer();

            if (!File.Exists(m_ConfigCsTemplate))
            {
                Debug.LogError("Fail to load config Template! " + m_ConfigCsTemplate);
                return;
            }

            MakeDirectory(m_TargetPath); //清空所有原来的表数据
            MakeDirectory(m_ConfigCsExportDirectory); //清空所有原来的表代码

            //加载代码模板
            var configTemplate = File.ReadAllText(m_ConfigCsTemplate);

            foreach (var f in allFiles)
            {
                Debug.Log("Load : " + f);
                var excel = ExcelHelper.LoadExcel(f);
                if (excel == null)
                {
                    Debug.LogError("Fail to load excel : " + f);
                    continue;
                }

                context.FileName = f;
                ExportExcel(excel, f, configSerializer, configTemplate, m_ConfigCsExportDirectory, ref context, sb);
            }

            //初始化代码
            if (File.Exists(m_ConfigCsInitFile))
            {
                var originFileText = File.ReadAllText(m_ConfigCsInitFile);
                var initCode = "\n";
                var resetCode = "\n";
                foreach (var name in context.AllConfigNames)
                {
                    initCode += "\t\t\t\t" + name + ".Init,\n";
                    resetCode += "\t\t\t\t" + name + ".Reset,\n";
                }

                var head = originFileText[
                    ..(originFileText.IndexOf("//INIT_CODE_GENERATE_START", StringComparison.Ordinal) +
                       "//INIT_CODE_GENERATE_START".Length)];
                var tail = originFileText[
                    originFileText.IndexOf("//INIT_CODE_GENERATE_END", StringComparison.Ordinal)..];
                {
                    var tailHead = tail[
                        ..(tail.IndexOf("//RESET_CODE_GENERATE_START", StringComparison.Ordinal) +
                           "//RESET_CODE_GENERATE_START".Length)];
                    var tailTail = tail[
                        tail.IndexOf("//RESET_CODE_GENERATE_END", StringComparison.Ordinal)..];
                    tail = tailHead + resetCode + tailTail;
                }

                File.WriteAllText(m_ConfigCsInitFile, head + initCode + tail);
            }
            else
                context.Logger.LogError("Fail to find the config init cs file! " + m_ConfigCsInitFile);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void ExportExcel(Excel excel, string filePath, ConfigSerializer cs, string configTemplate,
            string exportCsFileDir, ref ConfigExportContext context, StringBuilder sb)
        {
            context.IdCache.Clear();
            foreach (var table in excel.Tables)
            {
                if (!CheckTableIsValid(table))
                {
                    Debug.LogWarning("Table is not valid so would not be exported : " + filePath + " # " +
                                     table.TableName);
                    continue;
                }

                cs.Reset();
                var configInfo = GetConfigInfo(table);
                cs.SerializeInt(0); //写入一个整型占位置，未来这个会用来存储行数

                context.TableName = table.TableName;
                context.ConfigName = configInfo.Name;

                var validRowCount = 0;
                for (var row = 5; row <= table.NumberOfRows; row++)
                {
                    var firstCol = table.GetCell(row, 1);
                    var s = firstCol.Value.Replace(" ", "");
                    if (s.StartsWith("//")) continue; //注释行

                    context.Row = row;
                    context.Col = 2;
                    var idStr = table.GetCell(row, 2).Value;
                    if (!int.TryParse(idStr, out var id))
                    {
                        context.LogCellError("Fail to parse id");
                        continue; //其他的写错了没关系，但是如果Id读不出来整一行都不能要
                    }

                    if (context.IdCache.Contains(id)) //检查重名ID
                    {
                        context.LogCellError("Repeated id");
                        continue;
                    }

                    context.IdCache.Add(id);
                    validRowCount++;

                    foreach (var p in configInfo.PropertyInfos)
                    {
                        var exporter = m_Exporters[p.TypeStr]; //GetConfigInfo里面已经做了判断了，这里肯定拿得到
                        context.Col = p.ColumnIndex;
                        exporter.Serialize(table.GetCell(row, p.ColumnIndex).Value, cs, context);
                    }
                }

                cs.WriteHead(validRowCount);

                var exportFile = m_TargetPath + string.Format(m_TargetFileFormat, configInfo.Name);
                Debug.Log("ExportFile : " + exportFile);

                File.WriteAllBytes(exportFile, cs.ToArray());
                cs.Reset();

                //导出代码
                var csCode = configTemplate.Replace("/*", "").Replace("*/", "");
                var propertiesLoad = "";
                sb.Clear();
                foreach (var p in configInfo.PropertyInfos)
                {
                    var exporter = m_Exporters[p.TypeStr];
                    if (!string.IsNullOrEmpty(p.DescName) || !string.IsNullOrEmpty(p.Desc))
                    {
                        sb.AppendLine("\t\t/*");
                        if (!string.IsNullOrEmpty(p.DescName))
                        {
                            var lines = p.DescName.Split('\n');
                            foreach (var l in lines)
                                sb.Append("\t\t").AppendLine(l);
                        }

                        if (!string.IsNullOrEmpty(p.Desc))
                        {
                            var lines = p.Desc.Split('\n');
                            foreach (var l in lines)
                                sb.Append("\t\t").AppendLine(l);
                        }

                        sb.AppendLine("\t\t*/");
                    }

                    sb.Append("\t\tpublic ").Append(exporter.TypeStrInCs).Append(" ").Append(p.Name)
                        .AppendLine(" { get; private set; }");
                    if (p.ColumnIndex == 2) continue; //Id那一列不需要
                    propertiesLoad += "\t\t\t\t\t" + p.Name + " = configDeserializer." + exporter.ReadFunctionName +
                                      "(),\n";
                }

                csCode = csCode.Replace("#CONFIG_NAME#", configInfo.Name)
                    .Replace("#PROPERTIES_LOAD#", propertiesLoad).Replace("#PROPERTIES#", sb.ToString());
                sb.Clear();
                var exportCsFile = exportCsFileDir + configInfo.Name + ".cs";
                File.WriteAllText(exportCsFile, csCode);

                context.AllConfigNames.Add(configInfo.Name);
            }

            context.IdCache.Clear();
        }

        private ConfigInfo GetConfigInfo(ExcelTable t)
        {
            var configInfo = new ConfigInfo
            {
                Name = t.GetCell(1, 1).Value,
                ConfigDesc = t.GetCell(2, 1).Value,
                PropertyInfos = new List<PropertyInfo>(10),
            };

            for (var c = 1; c <= t.NumberOfColumns; c++)
            {
                var nameStr = t.GetCell(3, c).Value;
                if (string.IsNullOrEmpty(nameStr))
                    continue;
                var typeStr = t.GetCell(4, c).Value;
                typeStr = typeStr.ToLower(); //用全小写判断，懒得写错
                if (string.IsNullOrEmpty(typeStr) || !m_Exporters.ContainsKey(typeStr))
                    continue;
                configInfo.PropertyInfos.Add(new PropertyInfo
                {
                    ColumnIndex = c,
                    DescName = t.GetCell(1, c).Value,
                    Desc = t.GetCell(2, c).Value,
                    Name = nameStr,
                    TypeStr = typeStr,
                });
            }

            return configInfo;
        }

        private static bool CheckTableIsValid(ExcelTable t)
        {
            var title = t.GetCell(1, 1);
            if (string.IsNullOrEmpty(title.Value)) //没有表名
                return false;
            var t32 = t.GetCell(3, 2);
            if (string.IsNullOrEmpty(t32.Value)) //没有Id
                return false;
            var t42 = t.GetCell(4, 2);
            if (string.IsNullOrEmpty(t42.Value) || t42.Value != TypeInt) //没有Id类型或者Id不是整型
                return false;

            return true;
        }

        private static void MakeDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                var allFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (var f in allFiles)
                    File.Delete(f);
            }
            else
                Directory.CreateDirectory(path);
        }

        public class ConfigExportContext
        {
            public List<string> AllConfigNames;
            public int Col;
            public string ConfigName;
            public string FileName;
            public HashSet<int> IdCache;
            public ConfigExporterLogger Logger;
            public int Row;
            public string TableName;

            public void LogCellError(string msg)
            {
                Logger.LogError(msg + "(" + ConfigName + " in row = " + Row + ", col = " + Col + ")");
            }
        }

        public class PropertyExporter
        {
            public readonly string ReadFunctionName;
            public readonly Action<string, ConfigSerializer, ConfigExportContext> Serialize;
            public readonly string TypeStrInCs;

            public PropertyExporter(Action<string, ConfigSerializer, ConfigExportContext> serialize, string typeStrInCs,
                string readFunctionName)
            {
                Serialize = serialize;
                TypeStrInCs = typeStrInCs;
                ReadFunctionName = readFunctionName;
            }
        }

        public class ConfigInfo
        {
            public string ConfigDesc;
            public string Name;
            public List<PropertyInfo> PropertyInfos;
        }

        public class PropertyInfo
        {
            public int ColumnIndex;
            public string Desc;
            public string DescName;
            public string Name;
            public string TypeStr;
        }
    }
}