# Devir planı: durum, kalanlar, Gemini için çalışma kuralları

Tarih: 17 Ekim 2026. Depo dalı: `claude/project-thread-x2eovu`. Yerel kopya: KCB TECH klasörü.

## 1. Ne yaptık (kanıtlı)
Derleme 0 uyarı / 0 hata, 30 test yeşil (Linux, .NET 10.0.402).
- Çözüm iskeleti: SharedKernel, Application, Infrastructure, Web + 4 test projesi (çekirdek, uygulama, altyapı, web) + mimari testler (katman bağımlılığı).
- Çekirdek: `Result` / `ResultError` / `ErrorType`, komut-sorgu dağıtıcısı (Dispatcher).
- Windows Service olarak çalışma (`UseWindowsService`), `/health` ucu. Linux'ta konsol olarak denendi.
- Ayar sınıfı `PlatformOptions` + açılışta doğrulama (kurum adı, veritabanı yolu, kültür tr-TR).
- Denetim kaydı: `AuditEntry`, `IAuditLog`, `AuditRecorder`; SQLite'ta `SqliteAuditLog` (güncelleme/silme tetikleyiciyle engelli).
- Arayüz kabuğu: Razor Pages, Türkçe, erişilebilir düzen, tek yerden menü, yerel htmx.
- Kararlar: ADR-001 modüler monolit + Clean Architecture; ADR-002 .NET 10 / Windows Server; ADR-003 Razor + htmx; ADR-004 Windows Hizmeti paketi; ADR-005 SQLite (PostgreSQL'in yerine).

## 2. Nerede kaldık
Dilim 0 (Temel) neredeyse bitti. Kalan:
1. CI'a bağımlılık ve gizli anahtar taraması (`.github/workflows/ci.yml`; yalnızca GitHub tarafında düzenlenebiliyor).
2. Windows'ta gerçek hizmet kurulumu denemesi (Windows makine veya Windows CI).
3. Arayüzün gerçek tarayıcıda görsel kontrolü (renk karşıtlığı, dar ekran).
4. NuGet kilit dosyalarının açılması (ADR-002 açık noktası).
Bekleyen onaylar: Dilim 9 kurulum paketi sonraya ertelendi; yoklama için SQLite eşzamanlı yazma yükü Dilim 6/22'de ölçülecek.

## 3. Sıradaki işler (Dilim Kataloğu, Claude Docs)
Dilim 1 Kimlik (kullanıcı, rol, kapsamlı rol, yerel giriş, yetki politikası, denetim) → 2 Sürüm ve birimler → 3 Ders ve anabilim dalı → 4 Hedef ve program çıktıları → 5 Eşleme ve kapsama → 6 Onay ve yayın → 7 Yeni yıl ve karşılaştırma → 8 İçe/dışa aktarma → 9 Kurulum paketi → UÇEP/Belirtke/Soru bankası/Takvim/Yoklama fazları (10–30). Ayrıntı ve kabul senaryoları kataloğdadır (docs/BELGELER.md bağlantıları).

## 4. Çalışma düzeni
- Claude (supervisor): gereksinim, kabul senaryosu (S-…) ve test iskeleti yazar; Gemini'nin ürettiği kodu derleme/test çıktısıyla denetler; bulguları numaralı liste olarak verir.
- Gemini (uygulayıcı): her dilim için aşağıdaki kuralları uygulayarak kodu yazar.
- Kabul kanıtı yalnızca `dotnet build TipEgitim.slnx -c Release --no-incremental` (0 uyarı) ve `dotnet test TipEgitim.slnx -c Release` (hepsi yeşil) çıktısıdır. "Bitti" demek kanıt değildir.
- Dilim sırası: kabul senaryoları onaylanmadan kod yazılmaz.

## 5. Gemini'ye verilecek kurallar (kopyalanabilir)
- .NET 10, C# latest, `global.json` sürümü. Uyarılar hata sayılır (`Directory.Build.props`, `.editorconfig`).
- Katmanlar: Web → Infrastructure → Application → Domain/SharedKernel; ters bağımlılık yok (mimari testler denetler). Modüller yalnızca birbirinin `Contracts` projesine bağlanır.
- Beklenen hata: `Result` döner; boş `catch`, `#region`, `dynamic`, `DateTime.Now` (TimeProvider kullan) yasak. Async: baştan sona, `CancellationToken` al.
- Veri: yalnızca Infrastructure'da, parametreli SQL, SQLite'a özgü özellik yok (ileride PostgreSQL'e geçilebilsin). Silme yok (pasifleştirme); denetim kaydı yalnızca eklemeli.
- Yeni paket: ADR + lisans (MIT/Apache-2.0/BSD serbest; GPL/AGPL yasak). Sürümler `Directory.Packages.props`'ta.
- Türkçe karakter: karşılaştırma `StringComparison.Ordinal*`; kullanıcıya gösterilen metin tek kaynaktan (`UiStrings`).
- Test adı `S_<MODÜL>_<NN>_<Davranış>`; her hata düzeltmesi önce hatayı gösteren testle gelir; kabul senaryosu sayısı kadar test.
- Kapsam dışına çıkma; yeni özellik ekleme; belirsizlikte dur ve soru yaz.
- Her teslimde: değişen dosyalar, eklenen paketler, build/test çıktısı (birebir), doğrulanmayanlar.

## 6. Açık sorular
Windows Server sürümü ve müşteri BT'siyle teyit; hedef müşteri devlet mi vakıf mı (KVKK/erişilebilirlik yükümlülükleri); yerli yazılım belgesi; yoklama eşzamanlılık yükü; harici kimlik (LDAP/AD, OIDC) Dilim 9/26.
