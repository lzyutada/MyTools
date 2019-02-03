using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Forms;

namespace MangoPublishPackage.Configs
{
    class MyConfig : IDisposable
    {
        public static string ConfigFile { get { return @"./Configs/settings.json"; } }
        public static MyConfig Instance { get { return (null == _instance) ? (_instance = new MyConfig()) : _instance; } }
        public static MyConfig _instance = null;

        public TConfigEnntity Setting { get; protected set; }

        MyConfig()
        {
            try
            {
                using (FileStream fs = new FileStream(ConfigFile,  FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        Setting = JsonConvert.DeserializeObject< TConfigEnntity>(sr.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载配置发生异常.\r\n" + ex.Message);
            }
        }

        public void Dispose()
        {
            // TODO: 
        }

        public void Save()
        {
            try
            {
                using (FileStream fs = new FileStream(ConfigFile, FileMode.Open))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(JsonConvert.SerializeObject(Setting));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存配置信息发生异常.\r\n" + ex.Message);
            }
        }

        public void SetTfsMapping(string value) { Setting.CHECKPROJ_TFSMAPPING = value; }
        public void SetTeamDeveloper(string value) { Setting.CHECKPRO_TEAMDEVELOPER = value; }
        public void SetBeyondComparePath(string value) { Setting.CHECKPRO_BEYONDCOMPARE_PATH = value; }
    }

    sealed class TConfigEnntity
    {
        public string CHECKPROJ_TFSMAPPING { get; set; }
        public string CHECKPRO_TEAMDEVELOPER { get; set; }
        public string CHECKPRO_BEYONDCOMPARE_PATH { get; set; }
    }
}
