# 🚀 UpSchool Full Stack Developer Bitirme Projesi
## ⚙️ Kullanılan Teknolojiler
- ASP.Net 7 ile Entity Framework Core
- Blazor ile Loglama Ekranı
- CQRS Pattern
- Selenium ile Kazıma
- SignalR ile Loglama
- SMTP ile Email
- CodeFirst yaklaşımı
- Clean Architecture

### Proje 6 katmandan oluşur:
- 
  - Domain
  - Infrastructure
  - Application
  - WebApi
  - Wasm (Blazor ile Loglama)
  - Console (Verileri Kazıma)

## 📝 İsterler

### Uygulamamızın Hikayesi
Hep birlikte bir yazılım evi (Software House) kurduğumuzu düşünelim. Bu süreçte bir çok yazılım geliştirme işi alıyor ve başarıyla teslim ediyoruz. Tabii zamanla adımız duyuluyor ve bir e-ticaret sitesinin yöneticisi bizlere ulaşıyor. Büyük bir rakibinin önüne geçebilmek için rakibini ve rakibinin verdiği fiyatları sürekli takip etmek istiyor. Rakibi fiyat kırıyor ise o da kırmak, rakibi indirim yapıyor ise o da yapmak istiyor. Kendisi bizlere kısaca aşağıdaki açıklamaları iletiyor.

### Müşteri İhtiyaçları
Müşteri:Rakibimin ürünlerini kazıyabilecek bir bot uygulamasına ihtiyacım var. Bu bot uygulamasını başlattığımda istersem tüm ürünleri, istersem verdiğim sayı kadarını kazımak istiyorum. Bu ürünleri kazırken istersem sadece indirimde olanları, indirimde olmayanları ya da hepsini kazıyabilmeliyim. Bot çalışırken bot’un hangi durumda olduğunu ve neler yaptığını web uygulamanızdaki bir konsol (log) ekranından takip etmek istiyorum. Ayrıca; Ürünler kazındıktan sonra onları sizlerin oluşturacağı bir panelde görüntüleyebilmek ve kendi sistemime aktarabilmem adına bir excel dosyası olarak kaydedebilmek istiyorum. Bir de bot kazıma işlemini bitirince hem uygulama içi bir notifikasyon, hem de e-posta gönderirse süper olur. Hatta bu kısımı uygulamadaki ayarlar kısmına ekleyin ki ben istemezsem kapatabileyim lütfen.

Rakibimin Website Adresi: https://finalproject.dotnet.gg

### Alper Hocanın Yorumları
Arkadaşlar bu nokta da uygulamamızı 3 ana parçaya bölebileceğimizi düşünüyorum.

#### Crawler App (.NET - Console Application)
Bu uygulamamız bizim için veri kazıma işlemlerini gerçekleştirecektir. Veri kazıma işlemleri için Selenium Framework’ü kullanabiliriz. Uygulamamız başladığında kullanıcıya bazı sorular yöneltecektir.
#### 1-) Kaç tane ürün kazımak istiyorsunuz?
Kullanıcı hepsi yazabilir veya bir sayı verebilir. Biz gelen veriye göre işlem yapacak şekilde kodlamalıyız. Yani şöyle düşünmelisiniz gelen verinin hangi tipte olduğunu anlayıp, ona göre parse ettikten sonra işlemlerimizde değerlendirmeliyiz. Text de gelebilir, number’da gelebilir…
#### 2-) Hangi ürünleri kazımak istiyorsunuz?
A-) Hepsi, B-) İndirimdekiler, C-) Normal Fiyatlı Ürünler

Kullanıcıdan gerekli geri dönüşleri aldıktan sonra bu bilgiler ile API’ye istek yapıp, yeni bir “Order” oluşturuyoruz. Sonrasında ise rakip siteye konumlanıp, sizlere html elementlerindeki bıraktığım ekmek kırıntılarını (Hansel ve Gratel) takip ederek ürünlerin çekilip, parse edilmesini sağlamalıyız. Yani HTML üzerinden text’leri alıp, bir “Product” class’ına dönüştürmeliyiz.

#### Product Örneği
```c#
public  class  Product:EntityBase<Guid>
{  

public  Guid  OrderId { get; set; }

public  Order  Order { get; set; }

public  string  Name { get; set; }

public  string  Picture { get; set; }

public  bool  IsOnSale { get; set; }

public  decimal  Price { get; set; }

public  decimal  SalePrice { get; set; }   

}
```
Bu parse edilen ürünleri kullanıcının istediği gibi filtreleyip, API’ye bir istek yapıyoruz. Bu süreçte tabii ki işlemleri web uygulamasındaki konsol ekranına logluyor olmalıyız. (SignalR)
Örn;
- Websiteye giriş yapıldı. – 01.05.2023 : 12.30
- Toplam 10 sayfa ürün bulunduğu tespit edildi. 01.05.2023 : 12.30
- Birinci sayfa tarandı. Toplam 4 ürün - 01.05.2023 : 12.31
- İkinci sayfa tarandı. Toplam 8 ürün - 01.05.2023 : 12.33
Mesela uygulama bu süreçte bir hata ile karşılaşırsa aynı şekilde hatayı konsol’a göndermeli ve siparişi hatalı olarak güncellemelidir. Belki kullanıcının notifikasyon ayarlarına göre bir notifikasyon göndermeli ve/veya e-posta göndermelidir.

Örn: “#1094 numaralı sipariş beklenmedik bir hata ile karşılaştığı için tamamlanamamıştır.”

#### Web API (Clean Architecture - ASPNET Core 7 Application)
Bu uygulamamız bizim için uygulamalar arası haberleşmeyi ve veritabanı işlemlerimizi gerçekleştirecektir. Upstorage projemizdeki yapımızı bir-e-bir örnek alabilirsiniz. Bu nokta da isterseniz tüm işlemleri API Requestleri & SignalR üzerinden veya tamamen SignalR üzerinden de halledebilirsiniz. Bilgi, birikim ve yöntem seçiminize göre iki yolda tercih edilebilir. Clean architecture üzerinde “Application katmanından SignalR hub metodları nasıl tetiklenir?” başlığıyla ilgili örnekleri derslerimizde sizlere göstereceğim. O yüzden bu nokta da endişelenmenize gerek yoktur.

![Diagram](https://i.hizliresim.com/q0pqpkf.png)

Bu diagram üzerinde Api Request göndererek veya SignalR kullanarak gerçekleştirebileceğimiz çözüm yollarını görüyoruz.
#### Order Event Örneği
```c#
  public  class  OrderEvent:EntityBase<Guid>

{

public  Guid  OrderId { get; set; }

public  Order  Order { get; set; }   

public  OrderStatus  Status { get; set; }             

}
```

#### OrderStatus Örneği
```c#
  public  class  OrderEvent:EntityBase<Guid>

public  enum  OrderStatus

{

BotStarted=1,

CrawlingStarted=2,

CrawlingCompleted=3,

CrawlingFailed = 4,

OrderCompleted =5,

}
```

Order event’ler bizim sipariş ile ilgili checkpoint’lerimiz gibi düşünülebilir. Sipariş ne zaman başladı, ne zaman bitti vb. bilgileri tutmamızı sağlar. Kargo şirketlerindeki hareket dökümleri gibi düşünebilirsiniz.

Order (Sipariş) class’ını sizlere bırakıyorum arkadaşlar. Bu noktada bir siparişte müşteri hangi bilgileri bilmek ister diye hikayeye uygun olarak düşünmeliyiz. Mesela; kazınmak istenen ürün sayısı 100 ama biz 60 tane bulduk. Bunları order class’ı içinde bulundurmalıyız.

Bu uygulamamızda “İletişim Ayarları” kısımıda olması gerekiyor. Kullanıcının bu ayarlar ile ilgili seçim yapacağı küçük bir sayfamız olacaktır.
Örn: Uygulama içi bilgilendirme, E-Posta ile bilgilendirme seçeneklerinden hangileri seçili ise, bir sipariş tamamlandığında veya hataya düştüğünde bu ayarları kontrol edip, kullanıcıya bilgi vereceğiz. Uygulama içi bilgilendirme açık ise SignalR ile, e-posta seçili ise e-posta göndererek kullanıcıyı bilgilendireceğiz.

#### Blazor / React Client Application
Bu uygulamamızda kullanıcının siparişlerini, sipariş detaylarını, siparişe ait ürünlerini görüntüleyeceği sayfalar oluşturmamız gerekiyor. Yapacağımız işlemi daha basit anlatmam gerekirse Bootstrap Table’lar üzerinde verileri görüntüleyeceğiz.

Örn: https://getbootstrap.com/docs/5.0/content/tables/
#### Console Sayfamız
En basit haliyle bu sayfamızın görevi SignalR üzerinden OrderHub tarafından gönderilen “EventLog” vb. isimli bir metodu dinleyerek oradan gelen tüm verileri (yazıları, mesajları) sayfadaki terminal görüntüsünde şekillendirilmiş kısıma yazdırmaktır.
#### Örnekler:
- https://codepen.io/Xaptor/pen/XYVvGG
- https://codepen.io/addyosmani/pen/avxmvN
- https://codeeverywhere.ca/post.php?id=10&title=building-a-terminal-window-using-CSS

Bizim bu tarz bir terminal ekranımız olduğunda buraya düşen yazılar gerçek bir console/terminal ekranında çıkıyormuş gibi gözükecektir.

Devamı gelecektir…
