using AngleSharp.Dom;
using FinalProject.Domain.Dtos;
using FinalProject.Domain.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.SignalR.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;



new DriverManager().SetUpDriver(new ChromeConfig());

IWebDriver driver = new ChromeDriver();


// Divleri seçme

try
{

    driver.Navigate().GoToUrl("https://finalproject.dotnet.gg");

    Thread.Sleep(1500);

    //Kaç sayfa ürün bulunduğu tespit edildi
    ReadOnlyCollection<IWebElement> pageCount = driver.FindElements(By.XPath("/html/body/section/div/nav/ul/li"));
    var pageItems = pageCount.Count() - 1;


    Thread.Sleep(3000);
    Console.WriteLine("Sayfa Sayısı : " + pageItems);


    //for (int i = 1; i <= pageItems; i++)
    //{
    //    string currentPage = i.ToString();
    //    string url = $"https://finalproject.dotnet.gg/?currentPage={currentPage}";
    //    Thread.Sleep(1000);
    //    driver.Navigate().GoToUrl(url);

    //    ReadOnlyCollection<IWebElement> productsAmount = driver.FindElements(By.XPath("//div[@class='col mb-5']"));
    //    int productsCount = productsAmount.Count;
    //    Console.WriteLine($"Sayfa {currentPage} için ürün sayısı: {productsCount}");
    //}


    Console.WriteLine("Kaç ürün kazımak istiyorsun (hepsi ya da belirtildiği kadarı )");
    string result = Console.ReadLine();

    Console.WriteLine("Hangi ürünleri kazımak istiyorsun ?");
    Console.WriteLine("1-Hepsi");
    Console.WriteLine("2-İndirimdekiler");
    Console.WriteLine("3-Normal Fiyatlı Ürünler");

    string crawlingData = Console.ReadLine();

    int _crawlingData=int .Parse(crawlingData);
   // int _result=int .Parse(result);


    //İndirimdeki ürün sayısını bulmak istiyoruz
    int saleCount = 0;
    int totalCount = 0;


    if (_crawlingData ==1 && result == "hepsi")

    {
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


            foreach (HtmlNode div in divs)
            {
                totalCount++;
                HtmlNode salePriceNode = div.SelectSingleNode(".//span[@class='sale-price']");
                // Ürün adını al
                HtmlNode productNameNode = div.SelectSingleNode(".//h5[@class='fw-bolder product-name']");
                if (productNameNode != null)
                {
                    string productName = productNameNode.InnerText;
                    Console.WriteLine("Ürün Adı: " + productName);
                }

                // Ürün resim src'sini al
                HtmlNode imgNode = div.SelectSingleNode(".//img[@class='card-img-top']");
                if (imgNode != null)
                {
                    string src = imgNode.GetAttributeValue("src", "");
                    Console.WriteLine("Resim Src: " + src);
                }
                if (salePriceNode != null)
                {
                    // İndirimli fiyatı al
                    string salePrice = salePriceNode.InnerText;
                    Console.WriteLine("İndirimli Fiyat: " + salePrice);

                    // İndirimsiz fiyatı al
                    HtmlNode originalPriceNode = div.SelectSingleNode(".//span[@class='text-muted text-decoration-line-through price']");
                    if (originalPriceNode != null)
                    {
                        string originalPrice = originalPriceNode.InnerText;
                        Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                    }
                }
                else
                {
                    // İndirimli değilse fiyatı al
                    HtmlNode priceNode = div.SelectSingleNode(".//span[@class='price']");
                    if (priceNode != null)
                    {
                        string price = priceNode.InnerText;
                        Console.WriteLine("Fiyat: " + price);
                    }
                }


            }

        }

    }
    else
    {
        Console.WriteLine("Hata");
    }

    Console.WriteLine(totalCount);

    //İndirimde olan tüm ürünleri al
    if (_crawlingData == 2 && result == "hepsi")
    {


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

            foreach (HtmlNode div in divs)
            {
                // Sadece Sale etiketine sahip ürünleri kontrol etme
                HtmlNode saleBadge = div.SelectSingleNode(".//div[@class='badge bg-dark text-white position-absolute onsale']");
                if (saleBadge != null)
                {
                    saleCount++;

                    // Ürün adını al
                    HtmlNode productNameNode = div.SelectSingleNode(".//h5[@class='fw-bolder product-name']");
                    if (productNameNode != null)
                    {
                        string productName = productNameNode.InnerText;
                        Console.WriteLine("Ürün Adı: " + productName);
                    }

                    // Ürün resim src'sini al
                    HtmlNode imgNode = div.SelectSingleNode(".//img[@class='card-img-top']");
                    if (imgNode != null)
                    {
                        string src = imgNode.GetAttributeValue("src", "");
                        Console.WriteLine("Resim Src: " + src);
                    }

                    // İndirimli ürün mü kontrolü
                    HtmlNode salePriceNode = div.SelectSingleNode(".//span[@class='sale-price']");
                    if (salePriceNode != null)
                    {
                        // İndirimli fiyatı al
                        string salePrice = salePriceNode.InnerText;
                        Console.WriteLine("İndirimli Fiyat: " + salePrice);

                        // İndirimsiz fiyatı al
                        HtmlNode originalPriceNode = div.SelectSingleNode(".//span[@class='text-muted text-decoration-line-through price']");
                        if (originalPriceNode != null)
                        {
                            string originalPrice = originalPriceNode.InnerText;
                            Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                        }
                    }
                    else
                    {
                        // İndirimli değilse fiyatı al
                        HtmlNode priceNode = div.SelectSingleNode(".//span[@class='price']");
                        if (priceNode != null)
                        {
                            string price = priceNode.InnerText;
                            Console.WriteLine("Fiyat: " + price);
                        }
                    }
                }
            }



        }

    }
    else
    {
        Console.WriteLine("Hata");
    }
    Console.WriteLine(saleCount);
    if (_crawlingData == 3 && result == "hepsi")
    {
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

            foreach (HtmlNode div in divs)
            {
                // Sadece Sale etiketine sahip olmayan ürünleri kontrol etme
                HtmlNode saleBadge = div.SelectSingleNode(".//div[@class='badge bg-dark text-white position-absolute onsale']");
                if (saleBadge == null)
                {
                    // Ürün adını al
                    HtmlNode productNameNode = div.SelectSingleNode(".//h5[@class='fw-bolder product-name']");
                    if (productNameNode != null)
                    {
                        string productName = productNameNode.InnerText;
                        Console.WriteLine("Ürün Adı: " + productName);
                    }

                    // Ürün resim src'sini al
                    HtmlNode imgNode = div.SelectSingleNode(".//img[@class='card-img-top']");
                    if (imgNode != null)
                    {
                        string src = imgNode.GetAttributeValue("src", "");
                        Console.WriteLine("Resim Src: " + src);
                    }

                    // İndirimli ürün mü kontrolü
                    HtmlNode salePriceNode = div.SelectSingleNode(".//span[@class='sale-price']");
                    if (salePriceNode != null)
                    {
                        // İndirimli fiyatı al
                        string salePrice = salePriceNode.InnerText;
                        Console.WriteLine("İndirimli Fiyat: " + salePrice);

                        // İndirimsiz fiyatı al
                        HtmlNode originalPriceNode = div.SelectSingleNode(".//span[@class='text-muted text-decoration-line-through price']");
                        if (originalPriceNode != null)
                        {
                            string originalPrice = originalPriceNode.InnerText;
                            Console.WriteLine("İndirimsiz Fiyat: " + originalPrice);
                        }
                    }
                    else
                    {
                        // İndirimli değilse fiyatı al
                        HtmlNode priceNode = div.SelectSingleNode(".//span[@class='price']");
                        if (priceNode != null)
                        {
                            string price = priceNode.InnerText;
                            Console.WriteLine("Fiyat: " + price);
                        }
                    }
                }
            }
        }
    }
    else
    {
        Console.WriteLine("Hata");
    }

    //Kullanıcının istediği ürün sayısı kadar indirimde olanları getir.
    if (result != "hepsi" && result != "Hepsi" && result != "HEPSİ" && _crawlingData == 2)
    {

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


            foreach (HtmlNode div in divs)
            {
                // Sadece Sale etiketine sahip olan ürünleri kontrol etme
                HtmlNode saleBadge = div.SelectSingleNode(".//div[@class='badge bg-dark text-white position-absolute onsale']");
                if (saleBadge != null)
                {
                    saleCount++; // Sale etiketine sahip ürün sayısını artır

                    if (saleCount <= Convert.ToInt32(result))
                    {
                        // Ürün adını al
                        HtmlNode productNameNode = div.SelectSingleNode(".//h5[@class='fw-bolder product-name']");
                        if (productNameNode != null)
                        {
                            string productName = productNameNode.InnerText;
                            Console.WriteLine("Ürün Adı: " + productName);
                        }

                        // Ürün resim src'sini al
                        HtmlNode imgNode = div.SelectSingleNode(".//img[@class='card-img-top']");
                        if (imgNode != null)
                        {
                            string src = imgNode.GetAttributeValue("src", "");
                            Console.WriteLine("Resim Src: " + src);
                        }

                        // Diğer işlemler...
                    }
                    else
                    {
                        break; // İlk 5 ürünü aldıktan sonra döngüyü sonlandır
                    }
                }
            }
        }
    }
    Console.ReadKey();

    Thread.Sleep(1000);


    driver.Quit();
}
catch (Exception exception)
{
    driver.Quit();
}

//SeleniumLodDto CreateLog(string message) => new SeleniumLodDto(message);
