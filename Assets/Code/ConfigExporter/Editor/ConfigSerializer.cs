using System;
using System.Collections.Generic;
using System.Text;

namespace Code.ConfigExporter.Editor
{
    public class ConfigSerializer
    {
        private readonly List<byte> m_Bytes = new List<byte>(256);

        public void Reset()
        {
            m_Bytes.Clear();
        }

        public void SerializeInt(int v)
        {
            var bytes = BitConverter.GetBytes(v);
            for (var i = 0; i < bytes.Length; i++)
                m_Bytes.Add(bytes[i]);
        }

        public void SerializeBoolean(bool v)
        {
            var bytes = BitConverter.GetBytes(v);
            for (var i = 0; i < bytes.Length; i++)
                m_Bytes.Add(bytes[i]);
        }

        public void SerializeFloat(float v)
        {
            var bytes = BitConverter.GetBytes(v);
            for (var i = 0; i < bytes.Length; i++)
                m_Bytes.Add(bytes[i]);
        }

        public void SerializeString(string s)
        {
            var bytes = Encoding.Default.GetBytes(s);
            SerializeInt(bytes.Length);
            for (var i = 0; i < bytes.Length; i++)
                m_Bytes.Add(bytes[i]);
        }

        public void WriteHead(int v)
        {
            var bytes = BitConverter.GetBytes(v);
            for (int i = 0; i < bytes.Length; i++)
                m_Bytes[i] = bytes[i];
        }

        public byte[] ToArray()
        {
            return m_Bytes.ToArray();
        }
    }
}