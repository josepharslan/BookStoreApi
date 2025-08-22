# BookStore

**BookStore**, e-ticaret odaklı bir kitap yönetim sistemi için geliştirilen örnek bir çözüm setidir. Proje; REST API (WebApi), İş Katmanı (BusinessLayer), Veri Erişim Katmanı (DataAccessLayer) ve yönetim arayüzü (AdminUI) bileşenlerinden oluşur.

## Mimari Kısa Bakış
- **EntityLayer**: `Product`, `Subscriber`, `Category` gibi domain varlıkları
- **DataAccessLayer (EF Core)**: Repository implementasyonları (örn. `ISubscriberDal`, `IProductDal`)
- **BusinessLayer**: İş kuralları ve orkestrasyon (örn. `DashboardManager`, `NewsletterManager`)
- **WebApi**: Dışa açık REST uçları (Subscribers, Newsletters, Dashboard)
- **AdminUI**: Yönetim ekranları (ApexCharts, SweetAlert2 entegrasyonu)

## Öne Çıkan Yetkinlikler
- Abonelik yönetimi (kayıt, çakışma kontrolü)
- Aylık yeni abone raporu (12 ay dolu seri)
- Kampanya/test e-postalarını kuyruklayıp arka planda teslim
- Dashboard metrikleri: toplamlar, son eklenenler, en pahalı ürün, en düşük stok, kategori/yazar analizleri

## API Yüzeyi (Özet)
- `POST /api/Subscribers` — **Abone ol**
  - Gövde: `{ "email": "user@example.com" }` | Yanıt: `201 Created` / `409 Conflict`
- `GET /api/Subscribers/monthly?year=YYYY` — **Aylık yeni aboneler**
  - Yanıt: `year`, `items[]: { month, monthName, count }`
- `POST /api/Newsletters/send` — **Kampanya kuyruğa at**
  - Gövde: `{ "subject": "...", "htmlBody": "..." }`
- `POST /api/Newsletters/test?to=mail@ornek.com` — **Test gönderim**
- `GET /api/Dashboard` — **Toplu istatistik** (gösterge paneli verileri)
  
## Ekran Görüntüleri

### 1) Dashboard
<img width="1494" height="1292" alt="localhost_7191_Admin_Dashboard" src="https://github.com/user-attachments/assets/9e104d3c-e8db-4144-80a6-e1c3743d7021" />

### 2) Ana Sayfa
<img width="2107" height="5886" alt="localhost_7191_Default_Index (1)" src="https://github.com/user-attachments/assets/ab806f6b-8463-494a-8b35-ed821255e8cc" />

### 3) Api Client
<img width="1494" height="2460" alt="localhost_7287_swagger_index html" src="https://github.com/user-attachments/assets/f2b3e7b6-ecdb-4beb-8af2-1ff2b3aef19f" />
