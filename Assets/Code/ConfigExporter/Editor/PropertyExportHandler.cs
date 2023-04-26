using UnityEngine;

namespace Code.ConfigExporter.Editor
{
    public static class PropertyExportHandler
    {
        public static void DoSerializeForInt(string s, ConfigSerializer cs, Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            if (!int.TryParse(s, out var intVal))
            {
                context.LogCellError("Fail to parse value to int");
                cs.SerializeInt(0);
                return;
            }

            cs.SerializeInt(intVal);
        }

        public static void DoSerializeForBoolean(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            s = s.ToLower();
            var boolVal = s is "1" or "true";
            cs.SerializeBoolean(boolVal);
        }

        public static void DoSerializeForVector2IntArray(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            var ss = s.Split('|');
            if (ss == null || ss.Length <= 0)
            {
                cs.SerializeInt(0);
                return;
            }

            var len = ss.Length;
            cs.SerializeInt(len);
            for (var i = 0; i < ss.Length; i++)
                DoSerializeForVector2Int(ss[i], cs, context);
        }

        public static void DoSerializeForFloat(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            if (!float.TryParse(s, out var floatVal))
            {
                context.LogCellError("Fail to parse value to float");
                cs.SerializeFloat(0f);
                return;
            }

            cs.SerializeFloat(floatVal);
        }

        public static void DoSerializeForString(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            cs.SerializeString(s);
        }

        public static void DoSerializeForText(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            cs.SerializeString(s);
            cs.SerializeInt(Animator.StringToHash(s));
        }

        public static void DoSerializeForIntArray(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            var ss = s.Split(',');
            if (ss == null || ss.Length <= 0)
            {
                cs.SerializeInt(0);
                return;
            }

            var len = ss.Length;
            cs.SerializeInt(len);
            for (var i = 0; i < ss.Length; i++)
                DoSerializeForInt(ss[i], cs, context);
        }

        public static void DoSerializeForVector2(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            if (string.IsNullOrEmpty(s))
            {
                cs.SerializeFloat(0f);
                cs.SerializeFloat(0f);
                return;
            }

            var ss = s.Split(',');
            if (ss.Length != 2 || !float.TryParse(ss[0], out var x) || !float.TryParse(ss[1], out var y))
            {
                context.LogCellError("Fail to parse to vector2");
                cs.SerializeFloat(0f);
                cs.SerializeFloat(0f);
                return;
            }

            cs.SerializeFloat(x);
            cs.SerializeFloat(y);
        }

        public static void DoSerializeForVector3(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            if (string.IsNullOrEmpty(s))
            {
                cs.SerializeFloat(0f);
                cs.SerializeFloat(0f);
                cs.SerializeFloat(0f);
                return;
            }

            var ss = s.Split(',');
            if (ss.Length != 3 || !float.TryParse(ss[0], out var x) || !float.TryParse(ss[1], out var y) ||
                !float.TryParse(ss[2], out var z))
            {
                context.LogCellError("Fail to parse to vector3");
                cs.SerializeFloat(0f);
                cs.SerializeFloat(0f);
                cs.SerializeFloat(0f);
                return;
            }

            cs.SerializeFloat(x);
            cs.SerializeFloat(y);
            cs.SerializeFloat(z);
        }

        public static void DoSerializeForVector2Int(string s, ConfigSerializer cs,
            Code.ConfigExporter.Editor.ConfigExporter.ConfigExportContext context)
        {
            if (string.IsNullOrEmpty(s))
            {
                cs.SerializeInt(0);
                cs.SerializeInt(0);
                return;
            }

            var ss = s.Split(',');
            if (ss.Length != 2 || !int.TryParse(ss[0], out var x) || !int.TryParse(ss[1], out var y))
            {
                context.LogCellError("Fail to parse to vector2Int");
                cs.SerializeInt(0);
                cs.SerializeInt(0);
                return;
            }

            cs.SerializeInt(x);
            cs.SerializeInt(y);
        }
    }
}