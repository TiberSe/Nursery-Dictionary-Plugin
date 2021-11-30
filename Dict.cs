using Discord.WebSocket;
using Newtonsoft.Json;
using Nursery.Plugins;
using Nursery.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Septim.DictPlugin
{
    [JsonObject("Septim.DictPlugin.DictionaryConfig")]
    public class DictionaryConfig {
        [JsonProperty("inService")]
        public bool InService { get; set; } = true;
    }
    public static class Global {
        public static Hashtable Dict;
        public static bool status;
    }

    public class Dictionary : IPlugin {
        public string Name { get; } = "Septim.DictPlugin.Dictionary";
        public string HelpText { get; } = "Dictionaly by Tiber Septim";
        Nursery.Plugins.Type IPlugin.Type => Nursery.Plugins.Type.Filter;


        private DictionaryConfig config = null;
        public DictionaryConfig Config { get => config; }
        public void Initialize(IPluginManager loader, IPlugin[] plugins) {
            try {
                this.config = loader.GetPluginSetting<DictionaryConfig>(this.Name);
            } catch (System.Exception e) {
                Logger.DebugLog(e.ToString());
                this.config = null;
            }
            if (this.config == null) {  
                this.config = new DictionaryConfig();
            }
            
            Global.status = this.config.InService;
            Global.Dict = Dictionary.LoadDictFile();

        }

        public bool Execute(IBot bot, IMessage message) {
            if (message.Content.Length == 0) { return false; }
            if (!Global.status) { return false; }
            foreach (DictionaryEntry dc in Global.Dict) {
                message.Content = Regex.Replace(message.Content, (string)dc.Key, (string)dc.Value, RegexOptions.IgnoreCase);
            }
            message.AppliedPlugins.Add(this.Name);
            return true;

        }
        public static Hashtable LoadDictFile() {
            XDocument xmlDic = XDocument.Load("./dictionary.xml");
            XElement xmlElement = xmlDic.Element("words");
            IEnumerable<XElement> wordsRow = xmlElement.Elements("word");
            var kvs = new Hashtable();
            foreach (XElement el in wordsRow)  {
                XElement name = el.Element("key");
                XElement value = el.Element("value");
                kvs.Add(name.Value, value.Value);
            }        
            return kvs;
        }
    }
}
