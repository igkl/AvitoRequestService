using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using System.Threading;
using System.Text;
//using Selectors;

namespace avitoRequestService
{
    class Program
    {

        private static VkApi api = new VkApi();



        static char[] charsToTrim = { '*', ' ', '\n' };

        static HttpClientHandler handler = new HttpClientHandler() { MaxConnectionsPerServer = 2 };

        private static HttpClient client = new HttpClient(handler);

        private static List<Product> products = new List<Product>();

        static void Main(string[] args)
        {
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = 6613950,
                Login = "79185584046",
                Password = "1v3.4c@&$",
                Settings = Settings.All
            });

            while (true)
            {
                ProcessRequestToAvito().Wait();
                Thread.Sleep(50000);
            }

        }

        static List<Product> SortDescending(List<Product> list)
        {
            list.Sort((a, b) => b.DataTime.CompareTo(a.DataTime));
            return list;
        }
        private static async Task ProcessRequestToAvito()
        {

            // DateTime dateTimeFromString;
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.avito.ru/rostov-na-donu/telefony/iphone?s=104");
            request.Headers.Clear();
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.13; rv:60.0) Gecko/20100101 Firefox/60.0");
            var response = new HttpResponseMessage();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    response = await client.GetAsync(request.RequestUri);
                    break;
                }
                catch (HttpRequestException)
                {
                    Thread.Sleep(30000);

                    //response = await client.GetAsync(request.RequestUri);

                }

            }



            if (response.IsSuccessStatusCode)
            {
                var htmlDoc = new HtmlDocument(); //htmlAgilityPack

                htmlDoc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                HtmlNodeCollection divCollectionFromAvitoBodyHtml = htmlDoc.DocumentNode.SelectNodes(@"//div[contains(@class, 'item item_table clearfix js-catalog-item-enum')]");

                if (divCollectionFromAvitoBodyHtml != null)
                {
                    Console.WriteLine(divCollectionFromAvitoBodyHtml.Count);

                    if (products.Count == 0)
                    {
                        foreach (var node in divCollectionFromAvitoBodyHtml)
                        {
                            var dataType = node.GetAttributeValue("data-type", "1");

                            var id = node.Id;
                            var link = node.SelectSingleNode(Selectors.link).GetAttributeValue("href", null);//.ChildNodes[1].ChildNodes[1].GetAttributeValue("href", null);
                            var name = node.SelectSingleNode(Selectors.name).InnerText;
                            var price = node.SelectSingleNode(Selectors.price).GetAttributeValue("content", "Цена не указанна"); ;

                            var position = node.GetAttributeValue("data-position", null);
                            var dateTime = node.SelectSingleNode(Selectors.dateTime).GetAttributeValue("data-absolute-date", null).dateTimeFromAvitoString(); // метод расширение для работы со строками даты время из авито

                            products.Add(new Product { IdFromAvito = id, Link = link, Name = name.Trim(charsToTrim), Price = price, Position = position, DataTime = dateTime, Age = "new", DataType = dataType });

                        }
                        products = SortDescending(products);

                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        DateTime checkTime = products[0].DataTime;

                        foreach (var node in divCollectionFromAvitoBodyHtml)
                        {
                            var dateTime = node.SelectSingleNode(Selectors.dateTime).GetAttributeValue("data-absolute-date", null).dateTimeFromAvitoString(); // метод расширение для работы со строками даты время из авито
                            var id = node.Id;
                            var dataType = node.GetAttributeValue("data-type", "1");

                            if (!dataType.Equals("1")) continue; // убираем рекламные объявления, в avito html любые не равные data-type=1
                            if (products.Exists(x => dateTime.CompareTo(x.DataTime) == 0 && x.IdFromAvito == id)) continue; // не пропускаем объявления с не измененным временем и с существующим id


                            if (dateTime.CompareTo(checkTime) >= 0)
                            {
                                var link = node.SelectSingleNode(Selectors.link).GetAttributeValue("href", null);
                                var name = node.SelectSingleNode(Selectors.name).InnerText;
                                var price = node.SelectSingleNode(Selectors.price).GetAttributeValue("content", "Цена не указанна"); ;

                                var position = node.GetAttributeValue("data-position", null);

                                products.Insert(0, new Product { IdFromAvito = id, Link = link, Name = name.Trim(charsToTrim), Price = price, Position = position, DataTime = dateTime, Age = "new", DataType = dataType });

                                sb.Append($"{products[0].Name}\t Цена: {products[0].Price}\n ID: {products[0].IdFromAvito}; Time: {products[0].DataTime.ToString()}\n https://www.avito.ru{products[0].Link}\n\n");

                            }


                        }
                        if (sb.Length > 0)
                        {
                            products = SortDescending(products);

                            try
                            {
                                var send = api.Messages.Send(new MessagesSendParams { UserId = 170726879, Message = sb.ToString() });

                            }
                            catch (VkNet.Exception.UserAuthorizationFailException)
                            {
                                Console.WriteLine("TOKEN EXEPTION");
                                api.Authorize(new ApiAuthParams
                                {
                                    ApplicationId = 6613950,
                                    Login = "79185584046",
                                    Password = "1v3.4c@&$",
                                    Settings = Settings.All
                                });

                                var send = api.Messages.Send(new MessagesSendParams { UserId = 170726879, Message = sb.ToString() });

                            }
                            catch (VkNet.Exception.MessageIsTooLongException)
                            {
                                products.Clear();
                                Console.WriteLine("LONG MESSAGE !!!!!!!!!!!!");
                            }
                            finally
                            {
                                sb.Clear();
                            }


                            //Console.WriteLine($"Send Message: {send}");


                        }
                    }

                }
            }
            else
            {
                Console.WriteLine($"Request StatusCode: {response.StatusCode}");
            }

        }
    }
}

