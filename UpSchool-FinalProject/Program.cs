using FinalProject.Domain.Dtos;
using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using FinalProject.Domain.Enums;
using FinalProject.Domain.Entities;
using System.Collections.Generic;
using HtmlAgilityPack;
using FinalProject.Application.Features.Products.Commands.Add;
using FinalProject.Application.Features.Orders.Commands.Add;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using FinalProject.Application.Features.Products.Queries.GetAll;
using Newtonsoft.Json.Linq;
using FinalProject.Application.Features.Products.Queries.GetNonDiscount;
using FinalProject.Application.Features.Orders.Commands.Update;
using FinalProject.Application.Features.OrderEvents.Commands.Add;

Console.WriteLine("UpSchool Crawler");
Console.WriteLine("Kazıma işlemine başlamak için lütfen bir tuşa basınız.");

Console.ReadKey();

new DriverManager().SetUpDriver(new ChromeConfig());

IWebDriver driver = new ChromeDriver();

//HubConnection nesnesini oluşturmak ve yapılandırmak için HubConnectionBuilder kullanıldı. 
var hubConnection = new HubConnectionBuilder()
    .WithUrl($"https://localhost:7220/Hubs/SeleniumLogHub")
    .WithAutomaticReconnect()
    .Build();

//HubConnection'u başlattık.
await hubConnection.StartAsync();

try
{
    bool continueProcessing = true;

    do
    {

        using var client = new HttpClient();

        //ilk default bir başta order oluşturulur.
        OrderAddCommand orderAdd = new OrderAddCommand();
        orderAdd.Id = Guid.NewGuid();

        var jsonData = JsonSerializer.Serialize(orderAdd);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        await client.PostAsync("https://localhost:7220/api/Orders", content);

        OrderUpdateCommand order = new OrderUpdateCommand();
        order.Id = orderAdd.Id;

        OrderEventAddCommand orderBotStarted = new OrderEventAddCommand();

        orderBotStarted.OrderId = order.Id;
        orderBotStarted.Status = (OrderStatus)1;

        var orderBotStartedJson = JsonSerializer.Serialize(orderBotStarted);
        var orderBotStartedContent = new StringContent(orderBotStartedJson, Encoding.UTF8, "application/json");

        await client.PostAsync("https://localhost:7220/api/OrderEvents", orderBotStartedContent);
        Thread.Sleep(5000);

        //Bot'un başladığını logladık
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot başlatıldı."));

        //İstenen siteye yönlendirme işlemi yapıldı
        driver.Navigate().GoToUrl("https://finalproject.dotnet.gg");
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Websiteye giriş yapıldı"));


        OrderEventAddCommand orderCrawlingStarted = new OrderEventAddCommand();

        orderCrawlingStarted.OrderId = order.Id;
        orderCrawlingStarted.Status = (OrderStatus)2;

        var orderCrawlingStartedJson = JsonSerializer.Serialize(orderCrawlingStarted);
        var orderCrawlingStartedContent = new StringContent(orderCrawlingStartedJson, Encoding.UTF8, "application/json");

        HttpResponseMessage asdsdasd = await client.PostAsync("https://localhost:7220/api/OrderEvents", orderCrawlingStartedContent);

        Thread.Sleep(1500);

        List<ProductAddCommand> products = new List<ProductAddCommand>();

        try
        {
            //Kaç sayfa ürün bulunduğu tespit edildi
            ReadOnlyCollection<IWebElement> pageCount = driver.FindElements(By.XPath("/html/body/section/div/nav/ul/li"));
            var pageItems = pageCount.Count() - 1;
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Toplam : {pageItems} sayfa ürün bulunduğu tespit edildi"));
            // We are waiting for the results to load.
            Thread.Sleep(3000);


            // Tüm sayfalardaki ürinlerin hepsi taranır ve kaç adet ürün olduğu tespit edilir. Aynı zamanda database e kayıt edilir.
            int totalCount = 0;



            for (int i = 1; i <= pageItems; i++)
            {
                string currentPage = i.ToString();
                string url = $"https://finalproject.dotnet.gg/?currentPage={currentPage}";
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(1000);
                ReadOnlyCollection<IWebElement> divElements = driver.FindElements(By.XPath("//div[@class='col mb-5']"));

                string html = driver.PageSource;

                // HtmlAgilityPack ile ayrıştırma
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                HtmlNodeCollection divs = doc.DocumentNode.SelectNodes("//div[@class='col mb-5']");

                int productCountInPage = 0;
                foreach (HtmlNode div in divs)
                {
                    productCountInPage++;
                    totalCount++;
                    ProductAddCommand product = new ProductAddCommand();
                    product.OrderId = orderAdd.Id;

                    HtmlNode salePriceNode = div.SelectSingleNode(".//span[@class='sale-price']");
                    // Ürün adını al
                    HtmlNode productNameNode = div.SelectSingleNode(".//h5[@class='fw-bolder product-name']");
                    if (productNameNode != null)
                    {
                        string productName = productNameNode.InnerText;
                        product.Name = productName;
                    }

                    // Ürün resim src'sini al
                    HtmlNode imgNode = div.SelectSingleNode(".//img[@class='card-img-top']");
                    if (imgNode != null)
                    {
                        string src = imgNode.GetAttributeValue("src", "");
                        product.Picture = src;
                    }
                    if (salePriceNode != null)
                    {
                        product.IsOnSale = true;

                        // İndirimli fiyatı al
                        string salePrice = salePriceNode.InnerText;
                        product.SalePrice = ConvertToDecimal(salePrice);

                        // İndirimsiz fiyatı al
                        HtmlNode originalPriceNode = div.SelectSingleNode(".//span[@class='text-muted text-decoration-line-through price']");
                        if (originalPriceNode != null)
                        {
                            string originalPrice = originalPriceNode.InnerText;
                            product.Price = ConvertToDecimal(originalPrice);
                        }
                    }
                    else
                    {
                        product.IsOnSale = false;
                        // İndirimli değilse fiyatı al
                        HtmlNode priceNode = div.SelectSingleNode(".//span[@class='price']");
                        if (priceNode != null)
                        {
                            string price = priceNode.InnerText;
                            product.Price = ConvertToDecimal(price);
                        }
                    }
                    products.Add(product);

                }
                await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Sayfa {currentPage} için ürün sayısı: " + productCountInPage));
            }
        }
        catch (Exception)
        {
            OrderEventAddCommand orderCrawlingFailed = new OrderEventAddCommand();

            orderCrawlingFailed.OrderId = order.Id;
            orderCrawlingFailed.Status = (OrderStatus)4;

            var orderCrawlingFailedJson = JsonSerializer.Serialize(orderCrawlingFailed);
            var orderCrawlingFailedContent = new StringContent(orderCrawlingFailedJson, Encoding.UTF8, "application/json");

            await client.PostAsync("https://localhost:7220/api/OrderEvents", orderCrawlingFailedContent);
        }

        foreach (var product in products)
        {
            var jsonDataProduct = JsonSerializer.Serialize(product);
            var contentProduct = new StringContent(jsonDataProduct, Encoding.UTF8, "application/json");


            HttpResponseMessage productAddResponse = await client.PostAsync("https://localhost:7220/api/Products", contentProduct);
        }

        OrderEventAddCommand orderCrawlingCompleted = new OrderEventAddCommand();

        orderCrawlingCompleted.OrderId = order.Id;
        orderCrawlingCompleted.Status = (OrderStatus)3;

        var orderCrawlingCompletedJson = JsonSerializer.Serialize(orderCrawlingCompleted);
        var orderCrawlingCompletedContent = new StringContent(orderCrawlingCompletedJson, Encoding.UTF8, "application/json");

        await client.PostAsync("https://localhost:7220/api/OrderEvents", orderCrawlingCompletedContent);

        Console.WriteLine("Kaç ürün kazımak istiyorsun (hepsi ya da belirtildiği kadarı )");
        

        string result = Console.ReadLine();

        if (result == "hepsi")
        {
            order.RequestedAmount = products.Count;
        }
        else if( Convert.ToInt32(result) >= products.Count)
        {
            order.RequestedAmount = products.Count;
        }
        else
        {
            order.RequestedAmount = Convert.ToInt32(result);
        }

        Console.WriteLine("Hangi ürünleri kazımak istiyorsun ?");
        Console.WriteLine("1-Tümü");
        Console.WriteLine("2-İndirimdekiler");
        Console.WriteLine("3-Normal Fiyatlı Ürünler");

        string crawlingData = Console.ReadLine();
        int _crawlingData = int.Parse(crawlingData);
        order.ProductCrowlType = (ProductCrowlType)_crawlingData;

        if (_crawlingData == 1)
        {
            var query = new ProductGetAllQuery(order.Id, order.RequestedAmount);

            // HttpClient ile API'ye POST isteği gönderin ve sonucu alın
            HttpResponseMessage getAllResponse = await client.PostAsJsonAsync("https://localhost:7220/api/Products/GetAll", query);


            // İsteğin başarı durumu kontrol edilir
            if (getAllResponse.IsSuccessStatusCode)
            {
                // İsteğin başarılı olduğu durumda işlemler burada yapılır
                var responseContent = await getAllResponse.Content.ReadAsStringAsync();

                var jsonResponse = JArray.Parse(responseContent);

                int found = 0;
                foreach (JObject product in jsonResponse)
                {
                    //****** Burada verileri Dto dan almak istedik ama veriler json dan ProductGetAllDto ya çevrildiğinde verilerimiz hep null döndü.

                    string name = (string)product["name"];
                    string picture = (string)product["picture"];
                    bool isOnSale = (bool)product["isOnSale"];
                    decimal price = (decimal)product["price"];
                    decimal salePrice = (decimal)product["salePrice"];

                    Console.WriteLine($"Adı: {name}");
                    Console.WriteLine($"Resim Url: {picture}");
                    Console.WriteLine($"İndirimde mi: {isOnSale}");
                    Console.WriteLine($"Fiyatı: {price}");
                    Console.WriteLine($"İndirimli Fiyatı: {salePrice}");
                    Console.WriteLine();
                    found++;

                }

                order.TotalFoundedAmount = found;
                Console.WriteLine(found + " ürün bulunmuştur.");
            }
            else
            {
                // İsteğin başarısız olduğu durumda işlemler burada yapılır
                Console.WriteLine("Hata! Kodu: " + getAllResponse.StatusCode);
            }
        }
        else if (_crawlingData == 2)
        {
            var query = new ProductGetNonDiscountQuery(order.Id, order.RequestedAmount);

            // HttpClient ile API'ye POST isteği gönderin ve sonucu alın
            HttpResponseMessage getResponse = await client.PostAsJsonAsync("https://localhost:7220/api/Products/GetDiscount", query);


            // İsteğin başarı durumu kontrol edilir
            if (getResponse.IsSuccessStatusCode)
            {
                // İsteğin başarılı olduğu durumda işlemler burada yapılır
                var responseContent = await getResponse.Content.ReadAsStringAsync();

                var jsonResponse = JArray.Parse(responseContent);

                int found = 0;
                foreach (JObject product in jsonResponse)
                {
                    //****** Burada verileri Dto dan almak istedik ama veriler json dan ProductGetAllDto ya çevrildiğinde verilerimiz hep null döndü.

                    string name = (string)product["name"];
                    string picture = (string)product["picture"];
                    decimal price = (decimal)product["price"];
                    decimal salePrice = (decimal)product["salePrice"];

                    Console.WriteLine($"Adı: {name}");
                    Console.WriteLine($"Resim Url: {picture}");
                    Console.WriteLine($"Fiyatı: {price}");
                    Console.WriteLine($"İndirmli fiyatı: {salePrice}");
                    Console.WriteLine();

                    found++;
                }

                order.TotalFoundedAmount = found;
                Console.WriteLine(found + " ürün bulunmuştur.");

            }
            else
            {
                // İsteğin başarısız olduğu durumda işlemler burada yapılır
                Console.WriteLine("Hata! Kodu: " + getResponse.StatusCode);
            }
        }
        else if (_crawlingData == 3)
        {
            var query = new ProductGetNonDiscountQuery(order.Id, order.RequestedAmount);

            // HttpClient ile API'ye POST isteği gönderin ve sonucu alın
            HttpResponseMessage getResponse = await client.PostAsJsonAsync("https://localhost:7220/api/Products/GetNonDiscount", query);


            // İsteğin başarı durumu kontrol edilir
            if (getResponse.IsSuccessStatusCode)
            {
                // İsteğin başarılı olduğu durumda işlemler burada yapılır
                var responseContent = await getResponse.Content.ReadAsStringAsync();

                var jsonResponse = JArray.Parse(responseContent);

                int found = 0;
                foreach (JObject product in jsonResponse)
                {
                    //****** Burada verileri Dto dan almak istedik ama veriler json dan ProductGetAllDto ya çevrildiğinde verilerimiz hep null döndü.

                    string name = (string)product["name"];
                    string picture = (string)product["picture"];
                    decimal price = (decimal)product["price"];

                    Console.WriteLine($"Adı: {name}");
                    Console.WriteLine($"Resim Url: {picture}");
                    Console.WriteLine($"Fiyatı: {price}");
                    Console.WriteLine();

                    found++;
                }

                order.TotalFoundedAmount = found;
                Console.WriteLine(found + " ürün bulunmuştur.");

            }
            else
            {
                // İsteğin başarısız olduğu durumda işlemler burada yapılır
                Console.WriteLine("Hata! Kodu: " + getResponse.StatusCode);
            }
        }
        else
        {
            Console.WriteLine("Yanlış girdiniz!");
        }


        var orderUpdateJson = JsonSerializer.Serialize(order);
        var orderUpdateContent = new StringContent(orderUpdateJson, Encoding.UTF8, "application/json");

        await client.PutAsync("https://localhost:7220/api/Orders", orderUpdateContent);

        OrderEventAddCommand orderOrderCompleted = new OrderEventAddCommand();

        orderOrderCompleted.OrderId = order.Id;
        orderOrderCompleted.Status = (OrderStatus)5;

        var orderOrderCompletedJson = JsonSerializer.Serialize(orderOrderCompleted);
        var orderOrderCompletedContent = new StringContent(orderOrderCompletedJson, Encoding.UTF8, "application/json");

        await client.PostAsync("https://localhost:7220/api/OrderEvents", orderOrderCompletedContent);

        // Yeni bir sipariş oluşturmak isteyip istemediği sorulur
        Console.WriteLine("Yeni bir sipariş oluşturmak istiyor musunuz? (E/H)");
        string continueResponse = Console.ReadLine();

        if (continueResponse.ToUpper() == "H")
        {
            continueProcessing = false;
        }
    } while (continueProcessing);

    driver.Quit();

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot durduruldu."));
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    driver.Quit();
}
finally
{
    Console.ReadKey();
}

SeleniumLodDto CreateLog(string message) => new SeleniumLodDto(message);



decimal ConvertToDecimal(string str)
{
    str = str.Replace("$", "").Replace(",", ".");

    decimal decValue;
    if (decimal.TryParse(str, out decValue))
    {
        return decValue;
    }

    return decimal.MinValue;
}