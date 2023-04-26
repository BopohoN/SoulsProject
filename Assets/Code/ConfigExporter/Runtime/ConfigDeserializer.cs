using System;
using System.Text;
using UnityEngine;

namespace Code.ConfigExporter.Runtime
{
    public class ConfigDeserializer
    {
        private byte[] m_Bytes;
        private int m_Index;

        public void Init(byte[] bytes)
        {
            m_Bytes = bytes;
            m_Index = 0;
        }

        public int ReadInt()
        {
            var ret = BitConverter.ToInt32(m_Bytes, m_Index);
            m_Index += sizeof(int);
            return ret;
        }

        public int[] ReadIntArray()
        {
            var len = ReadInt();
            var ret = new int[len];
            for (var i = 0; i < len; i++)
                ret[i] = ReadInt();
            return ret;
        }

        public float ReadFloat()
        {
            var ret = BitConverter.ToSingle(m_Bytes, m_Index);
            m_Index += sizeof(float);
            return ret;
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(ReadFloat(), ReadFloat());
        }

        public bool ReadBoolean()
        {
            var ret = BitConverter.ToBoolean(m_Bytes, m_Index);
            m_Index += sizeof(bool);
            return ret;
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
        }

        public Vector2Int ReadVector2Int()
        {
            return new Vector2Int(ReadInt(), ReadInt());
        }

        public Vector2Int[] ReadVector2IntArray()
        {
            var len = ReadInt();
            var result = new Vector2Int[len];
            for (var i = 0; i < len; i++)
                result[i] = ReadVector2Int();
            return result;
        }

        public string ReadString()
        {
            var len = ReadInt();
            var ret = Encoding.Default.GetString(m_Bytes, m_Index, len);
            m_Index += len;
            return ret;
        }

        public void Reset()
        {
            m_Bytes = null;
            m_Index = 0;
        }
    }
}