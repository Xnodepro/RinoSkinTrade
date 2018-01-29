using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Threading;
using HtmlAgilityPack;
using System.Drawing;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using WebSocketSharp;
using System.IO;
using System.Web;
using System.Reflection;

namespace CSMONEY
{
    class Work
    {
        IWebDriver driver;
        int ID = 0;

        List<string> me = new List<string>();
        //    List<ForItemsUSer> them = new List<ForItemsUSer>();
        List<ite> ItemsUser = new List<ite>();
        bots ItemsBot;
        string apiKey = Properties.Settings.Default.ApiKey;
        string TradeURL = "";

        CookieContainer cookies = new CookieContainer();
        HttpClientHandler handler = new HttpClientHandler();
        #region botInventoryStruc
        public struct Dat
        {

            public List<ite> adobe1470 { get; set; }
            public List<ite> baron1578 { get; set; }
            public List<ite> cooler613 { get; set; }
            public List<ite> erikli74 { get; set; }
            public List<ite> etipuf257 { get; set; }
            public List<ite> first5025 { get; set; }
            public List<ite> katkat750 { get; set; }
            public List<ite> medusa1325 { get; set; }
            public List<ite> para6350 { get; set; }
            public List<ite> peter6364 { get; set; }
        }
        public struct ite
        {

            public long assetid { get; set; }
            //  public int bot { get; set; }
            public string b { get; set; }
            //   public List<it> items { get; set; }
            public double p { get; set; }
            public string m { get; set; }
        }


        #endregion
        #region ForItems
        public struct bots
        {
            public List<ite> LI { get; set; }

        }
        #endregion
        #region UserInventoryStruc
        public struct DatUser
        {
            public List<iteUser> items { get; set; }
        }
        public struct iteUser
        {

            public string name { get; set; }
            public double price { get; set; }
            public List<itUser> items { get; set; }
            public string id { get; set; }
        }
        public struct itUser
        {
            public string id { get; set; }

        }
        #endregion

        public Work(int id)
        {
            ID = id;
        }

        public void INI()
        {
            try
            {
                var driverService = ChromeDriverService.CreateDefaultService();  //скрытие 
                driverService.HideCommandPromptWindow = true;                    //консоли
                driver = new ChromeDriver(driverService);
                driver.Navigate().GoToUrl("http://skin.trade/");
                MessageBox.Show("Введите все данные , после этого программа продолжит работу!");

                //      TradeURL = driver.FindElement(By.Id("tradeurl")).GetAttribute("value").ToString();
                var _cookies = driver.Manage().Cookies.AllCookies;
                foreach (var item in _cookies)
                {
                    handler.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
                }
                //Запуск запросов на отслеживания инвентаря ботов
                for (int i = 0; i < Program.threadCount; i++)
                {
                    new System.Threading.Thread(delegate ()
                    {
                        try
                        {
                            Thread.Sleep(10000);
                           Get(handler);
                        }
                        catch (Exception ex) { }
                    }).Start();
                }
                new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        
                       GetUser(handler);
                    }
                    catch (Exception ex) { }
                }).Start();

                //new System.Threading.Thread(delegate () {
                //        while (true)
                //        {
                //            try
                //            {
                //                Thread.Sleep(2000);
                //                driver.Navigate().Refresh();
                //                Thread.Sleep(300000);
                //            }
                //            catch (Exception ex) { }

                //        }
                //    }).Start();

            }
            catch (Exception ex) { Program.Mess.Enqueue(ex.Message); }

        }

        public HttpClient Prox(HttpClient client1, HttpClientHandler handler, string paroxyu)
        {

            HttpClient client = client1;
            try
            {
                string
                httpUserName = "webminidead",
                httpPassword = "159357Qq";
                string proxyUri = paroxyu;
                NetworkCredential proxyCreds = new NetworkCredential(
                    httpUserName,
                    httpPassword
                );
                WebProxy proxy = new WebProxy(proxyUri, false)
                {
                    UseDefaultCredentials = false,
                    Credentials = proxyCreds,
                };
                try
                {
                    handler.Proxy = null;
                    handler.Proxy = proxy;
                    handler.PreAuthenticate = true;
                    handler.UseDefaultCredentials = false;
                    handler.Credentials = new NetworkCredential(httpUserName, httpPassword);
                    handler.AllowAutoRedirect = true;
                }
                catch (Exception ex) { }
                client = new HttpClient(handler);
            }
            catch (Exception ex) { }
            return client;
        }
        private void Get(HttpClientHandler handler)
        {
            HttpClientHandler handler1 = handler;

            while (true)
            {
                try
                {
                    HttpClientHandler handler2 = new HttpClientHandler();
                    var _cookies = driver.Manage().Cookies.AllCookies;
                    foreach (var item in _cookies)
                    {
                        handler2.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
                    }

                    HttpClient client = null;
                    //if (Program.ProxyList.Count <= 0)
                    //{
                    client = new HttpClient(handler2);
                    //}
                    //else
                    //{ 
                    //    string newProxy = Program.ProxyList.Dequeue();
                    //    handler2.Proxy = null;
                    //    client = Prox(client, handler2, newProxy);
                    //}

                    client.Timeout = TimeSpan.FromSeconds(40);

                    client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                    client.DefaultRequestHeaders.Add("accept", "*/*");
                    client.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                    client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");

                    var response = client.GetAsync("http://skin.trade/load_all_bots_inventory?hash=" + gettime()).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        var ITEMS = JsonConvert.DeserializeObject<Dat>(responseString);
                        bots BOT = new bots();
                        BOT.LI = new List<ite>();
                        BOT.LI.AddRange(ITEMS.adobe1470);
                        BOT.LI.AddRange(ITEMS.baron1578);
                        BOT.LI.AddRange(ITEMS.cooler613);
                        BOT.LI.AddRange(ITEMS.erikli74);
                        BOT.LI.AddRange(ITEMS.etipuf257);
                        BOT.LI.AddRange(ITEMS.first5025);
                        BOT.LI.AddRange(ITEMS.katkat750);
                        BOT.LI.AddRange(ITEMS.medusa1325);
                        BOT.LI.AddRange(ITEMS.para6350);
                        BOT.LI.AddRange(ITEMS.peter6364);
                        BOT.LI.OrderByDescending(x => x.p).ToList();
                        ClickItem(BOT);
                        Program.Mess.Enqueue("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов:" + BOT.LI.Count.ToString());
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex) {
                    Program.Mess.Enqueue("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|1" + ex.Message);
                }
            }
            // return new Data();
        }
        private void GetUser(HttpClientHandler handler)
        {
            HttpClientHandler handler1 = handler;

            while (true)
            {
                try
                {
                    HttpClientHandler handler2 = new HttpClientHandler();
                    var _cookies = driver.Manage().Cookies.AllCookies;
                    foreach (var item in _cookies)
                    {
                        handler2.CookieContainer.Add(new System.Net.Cookie(item.Name, item.Value) { Domain = item.Domain });
                    }

                    HttpClient client = null;
                    //if (Program.ProxyList.Count <= 0)
                    //{
                    client = new HttpClient(handler2);
                    //}
                    //else
                    //{ 
                    //    string newProxy = Program.ProxyList.Dequeue();
                    //    handler2.Proxy = null;
                    //    client = Prox(client, handler2, newProxy);
                    //}

                    client.Timeout = TimeSpan.FromSeconds(40);

                    client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                    client.DefaultRequestHeaders.Add("accept", "*/*");
                    client.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                    client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");

                    var response = client.GetAsync("http://skin.trade/load_inventory?refresh=true&hash=" + gettime()).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;
                        string responseString = responseContent.ReadAsStringAsync().Result;
                        var ITEMS = JsonConvert.DeserializeObject<List<ite>>(responseString);
                        ItemsUser = ITEMS.OrderByDescending(x => x.p).ToList();
                        Program.Mess.Enqueue("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|" + "Завершил загрузку предметов ЮЗЕРА:" + ITEMS.Count);
                    }

                    Thread.Sleep(20000);
                }
                catch (Exception ex) { Program.Mess.Enqueue("" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|2" + ex.Message); }
            }

        }



        private bool ClickItem(bots json)
        {

            try
            {
                var da = json;

                foreach (var item in da.LI)
                {

                    foreach (var name in Program.Data)
                    {
                        // string _name = HttpUtility.UrlDecode(item.itemURLName);
                        if (item.m.Replace(" ", "") == (name.Name).Replace(" ", ""))
                        {
                            Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "| Нашел предмет :" + item.m + "|Цена_Сайта:" + item.p + "|Цена_Наша:" + name.Price);
                            if (item.p <= name.Price)
                            {
                                string res = GetQueryString(json, item.p,item.assetid, item.b);
                                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                                string ss1 = "function test() {var xhr = new XMLHttpRequest();" +
                                   //     "var body = \"{\\\"steamid\\\":\\\"" + b.ToString() + "\\\",\\\"peopleItems\\\":[],\\\"botItems\\\":[\\\"" + id.ToString() + "\\\"],\\\"onWallet\\\":-" + p.ToString().Replace(",", ".") + ",\\\"gid\\\":\\\"" + "\\\"}\";" +
                                   "var body = \""+ res + "\";" +
                                    " xhr.open(\"POST\", 'http://skin.trade/send_offer', false); " +
                                    "xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');" +
                                    " xhr.setRequestHeader('accept', '*/*');" +
                                    " xhr.send(body);return xhr.status+'|'+xhr.responseText; } return test();";
                                var title = js.ExecuteScript(ss1);
                                Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "Результат:"+ title);
                                Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "Завершил все запросы!");
                                Thread.Sleep(3000);
                                return false;

                            }
                            else
                            {
                                SetListBadPrice(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "cs.money", item.m.ToString(), name.Price.ToString(), item.p.ToString());
                            }

                        }
                    }

                }
            }
            catch (Exception ex) { Program.Mess.Enqueue("БОТ[" + ID + "] " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "|Ошибка2 :" + ex.Message); }
            return false;
        }

        private string GetQueryString(bots json,double priceBot,long assID,string BotId)
        {
            List<ite> User = ItemsUser;
            List<long> ItemsForUser = new List<long>();
            List<long> ItemsForBot = new List<long>();
            double priceTmp = 0;
            priceBot = priceBot * 1.02;
            foreach (var item in User)
            {
                if (item.p != 0 && priceBot >= priceTmp + item.p)
                {
                    priceTmp += item.p;
                    ItemsForUser.Add(item.assetid);
                }
            }
            double percent = (1 - (priceTmp / priceBot)) * 100;
            if (percent > 2)
            {
               User.Reverse();
                foreach (var item in User)
                {
                    if (item.p != 0 && ItemsForUser.Contains(item.assetid) == false)
                    {
                        ItemsForUser.Add(item.assetid);
                        priceTmp += item.p;
                        break;
                    }
                }
                var da = json;

                foreach (var item in da.LI)
                {
                    double tmp = priceBot+item.p;
                    if (item.p!=0 &&item.b == BotId&& tmp < priceTmp)
                    {
                        ItemsForBot.Add(item.assetid);
                        priceBot += item.p;
                    }
                }
                var a = priceBot;
                var b = priceTmp;
            }
            Program.Mess.Enqueue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "Сумма юзера :" + priceTmp + "|Сумма Бота:"+ priceBot);
            string Query = "steamid="+BotId;
            foreach (var item in ItemsForUser)
            {
                Query += $"&userItems%5B%5D={item.ToString()}";
            }
            Query += $"&botItems%5B%5D={assID}";
            foreach (var item in ItemsForBot)
            {
                Query += $"&botItems%5B%5D={item.ToString()}";
            }
            return Query;
        }
        private void SetListBadPrice(string _Data, string _Site, string _Name, string _OldPrice, string _NewPrice)
        {
            Program.DataViewBadPrice item = new Program.DataViewBadPrice()
            {
                Date = _Data,
                Site = _Site,
                Name =_Name,
                OldPrice = _OldPrice,
                NewPrice =_NewPrice
            };
            Program.BadPrice.Add(item);
        }
        private string gettime()
        {
            string[] ms = DateTime.Now.ToUniversalTime().ToString("R").Split(' ');
            ms[0] = ms[0].Replace(",", "");
            string tmp = ms[1];
            ms[1] = ms[2];
            ms[2] = tmp;
            string res = "";
            int pp = 0;
            foreach (var item in ms)
            {
                if (pp == ms.Length - 1)
                {
                    res += item;
                }
                else
                {
                    res += item + "%20";
                }
                pp++;
            }
            return res + "+0200%20(%D0%A4%D0%B8%D0%BD%D0%BB%D1%8F%D0%BD%D0%B4%D0%B8%D1%8F%20(%D0%B7%D0%B8%D0%BC%D0%B0))";
        }
    }
}
