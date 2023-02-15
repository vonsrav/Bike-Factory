using MyBikesFactoy.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyBikesFactoy.Data
{
    public  static class BikesXMLData
    {
        private static string GetFilePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "bikes.xml";
        }
        public static List<Bikes> Load()
        {
            string filepath=GetFilePath();
            if(!File.Exists(filepath))
                return new List<Bikes>();
            string fileContent = File.ReadAllText(filepath);
            if (fileContent == "")
                return new List<Bikes>();
            using (var reader = new StringReader(fileContent))
            {
                var serializer = new XmlSerializer(typeof(List<Bikes>));
                return (List<Bikes>)serializer.Deserialize(reader)!;
            }
        }
        public static void Save(List<Bikes> list)
        {
            string filePath=GetFilePath();
            using (var writer = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(List<Bikes>));
                serializer.Serialize(writer, list);

            }

        }


    }
}
