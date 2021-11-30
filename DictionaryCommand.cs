using Discord.WebSocket;
using Newtonsoft.Json;
using Nursery.Plugins;
using Nursery.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Septim.DictPlugin {
    public class DictionaryCommand : IPlugin {

        public string Name { get; } = "Septim.DictPlugin.DictionaryCommand";
        public string HelpText { get; } =  "Tiber's Dictionary Opeartion Commands"+"```$learn key value :keyの読みをvalueとして登録 \n $forget key :keyの読みを消去```";
        Nursery.Plugins.Type IPlugin.Type => Nursery.Plugins.Type.Command;

        public void Initialize(IPluginManager loader, IPlugin[] plugins) {  }

        public bool Execute(IBot bot, IMessage message) {
            string[] args = message.Original.Content.Split(' ');
            if (args[0].StartsWith("$learn")) {
                if (args.Length == 3) {
                    AddDictFile(args[1], args[2]);
                    bot.SendMessageAsync(message.Original.Channel, $"Learned:{args[1]} / {args[2]}", true);
                    message.Content = "";
                    message.AppliedPlugins.Add(this.Name);
                    message.Terminated = true;
                    return true;
                } else {
                    bot.SendMessageAsync(message.Original.Channel, "引数が過剰もしくは不足しています。", true);
                    message.Content = "";
                    message.AppliedPlugins.Add(this.Name);
                    message.Terminated = true;
                    return true;
                }
            } else if (args[0].StartsWith("$forget")){
                if (args.Length == 2){
                    RemoveFromDict(args[1]);
                    bot.SendMessageAsync(message.Original.Channel, $"Removed:{args[1]}", true);
                    message.Content = "";
                    message.AppliedPlugins.Add(this.Name);
                    message.Terminated = true;
                } else {
                    bot.SendMessageAsync(message.Original.Channel, "引数が過剰もしくは不足しています。", true);
                    message.Content = "";
                    message.AppliedPlugins.Add(this.Name);
                    message.Terminated = true;
                    return true;
                }
            } else if (args[0].StartsWith("$enableDictionary")) {
                Global.status = true;
                bot.SendMessageAsync(message.Original.Channel, "辞書機能を有効にしました。", true);
                message.Content = "";
                message.AppliedPlugins.Add(this.Name);
                message.Terminated = true;

            } else if (args[0].StartsWith("$unableDictionary")) {
                Global.status = false;
                bot.SendMessageAsync(message.Original.Channel, "辞書機能を無効にしました。", true);
                message.Content = "";
                message.AppliedPlugins.Add(this.Name);
                message.Terminated = true;
            }
                return false;
        }

        public void AddDictFile(string key, string value) {
            Global.Dict.Add(key, value);
            XDocument xmlDic = XDocument.Load("./dictionary.xml");
            var newelem = new XElement("word",
                new XElement("key", key),
                new XElement("value", value)
            );
            xmlDic.Elements().First().Add(newelem);
            xmlDic.Save("./dictionary.xml");
            ReloadDictFile();
        }

        public void RemoveFromDict(string key) {
            XDocument xmlDic = XDocument.Load("./dictionary.xml");
            XElement xmlElement = xmlDic.Element("words");
            IEnumerable<XElement> elList =
                from el in xmlElement.Elements("word")
                where el.Element("key").Value.Equals(key)
                select el;
            elList.Remove();
            xmlDic.Save("./dictionary.xml");
            ReloadDictFile();      
        }
        public static void ReloadDictFile() {
            Global.Dict = Dictionary.LoadDictFile();
        }

    }
}