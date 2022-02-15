using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BicycleRepairManagement
{
    public class MyStorage
    {
        #pragma warning disable SYSLIB0011
        public static void WriteBin<T>(T data, string fileName)
        {
            if (data == null) return;
            try
            {
                using FileStream stream = new FileStream(fileName, FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        public static T? ReadBin<T>(string fileName)
        {
            try
            {
                using FileStream stream = new FileStream(fileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                T data = (T)binaryFormatter.Deserialize(stream);
                if (data == null) return default(T);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return default(T);
            }
        }
        public static void WriteXml<T>(T data, string fileName)
        {
            XmlSerializer sr = new XmlSerializer(typeof(T));

            FileStream stream = new FileStream(fileName, FileMode.Create);
            sr.Serialize(stream, data);
        }

        public static T? ReadXml<T>(string fileName)
        {
            try
            {
                using StreamReader stream = new StreamReader(fileName);
                XmlSerializer sr = new XmlSerializer(typeof(T));
                var deserialised = sr.Deserialize(stream);
                if (deserialised == null) return default(T);

                T data = (T) deserialised;

                return data;
            } catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return default(T);
            }
        }

        public static void WriteObjectAsXMLStringToFS<T>(string fileName, T obj)
        {
            if (obj == null) return;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using TextWriter textWriter = new StreamWriter(fileName);
            serializer.Serialize(textWriter, obj);
        }
    }
}