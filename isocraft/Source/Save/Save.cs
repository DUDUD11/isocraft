using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text.Json;











namespace isocraft
{
    public class Save
    {
        private static Save _instance;
        public static string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string gameName = "IsoCraft";
        public static string baseFolder = localAppDataPath + "\\" + gameName + "";
        public static string MapbaseFolder = localAppDataPath + "\\" + gameName + "\\Map";
        public static string DatabaseFolder = localAppDataPath + "\\" + gameName + "\\Data";


        private Save()
        {

        }

        public static Save Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Save();
                    CreateBaseFolders();
                }
                return _instance;
            }
        }


        public static void CreateBaseFolders()
        {
            CreateFolder(baseFolder);
            CreateFolder(MapbaseFolder);
            CreateFolder(DatabaseFolder);
        }

        public static void CreateFolder(string s)
        {
            DirectoryInfo CreateSiteDirectory = new DirectoryInfo(s);
            if (!CreateSiteDirectory.Exists)
            {
                CreateSiteDirectory.Create();
            }
        }

        public bool CheckIfFileExists(string PATH)
        {
            bool fileExists = File.Exists(localAppDataPath + "\\" + gameName + "\\" + PATH);
            return fileExists;
            //return true;
        }

        public void DeleteFile(string PATH)
        {
            File.Delete(PATH);
        }
        public void DeleteMapFile(string PATH)
        {
            PATH = MapbaseFolder + PATH;

            File.Delete(PATH);
        }

        public void DeleteDataFile(string PATH)
        {
            PATH = DatabaseFolder + PATH;

            File.Delete(PATH);
        }


        public void SaveMapData(Map map, string filename)
        {
            filename = MapbaseFolder + "\\" + filename;

            string json = JsonSerializer.Serialize(map, new JsonSerializerOptions()
            {
                IncludeFields = true
            });


            File.WriteAllText(filename, json);
        }


        public void SaveUpgradeData(UpgradeData upgradeData, string filename)
        {
            filename = DatabaseFolder + "\\" + filename;

            string json = JsonSerializer.Serialize(upgradeData, new JsonSerializerOptions()
            {
                IncludeFields = true
            });


            File.WriteAllText(filename, json);
        }




        public Map LoadMapData(string filepath)
        {
            filepath = MapbaseFolder + "\\" + filepath;


            if (!File.Exists(filepath))
            {
                return null;
            }
            var jsonOptions = new JsonSerializerOptions()
            {
                IncludeFields = true
            };

            string json = File.ReadAllText(filepath);
            Map map = JsonSerializer.Deserialize<Map>(json, jsonOptions);

            return map;
        }

        public UpgradeData LoadUpgradeData(string filepath)
        {
            filepath = DatabaseFolder + "\\" + filepath;


            if (!File.Exists(filepath))
            {
                return null;
            }
            var jsonOptions = new JsonSerializerOptions()
            {
                IncludeFields = true
            };

            string json = File.ReadAllText(filepath);
            UpgradeData upgradeData = JsonSerializer.Deserialize<UpgradeData>(json, jsonOptions);

            return upgradeData;
        }


    }
}

