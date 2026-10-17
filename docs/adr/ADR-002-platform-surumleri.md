# ADR-002: Platform ve sürümler

Durum: Kabul, doğrulama CI'da (16 Ekim 2026)

Not: Veritabanı kararı ADR-005 ile SQLite olarak değiştirildi.

## Karar
- .NET 10 (LTS), C# `latest`. SDK sürümü `global.json` ile sabitlenir.
- Veritabanı: PostgreSQL. Hedef 17 ve üstü; kesin sürüm Dilim 1'de veri erişimi kurulurken CI'da gerçek bir PostgreSQL ile denenerek sabitlenir.
- Çalışma ortamı: Windows Server (2022 ve 2025 hedef; müşteri BT'siyle teyit edilecek). Geliştirme ve CI hem Linux hem Windows'ta çalışır.
- Üretim bağımlılığı eklemek ADR + lisans kontrolü gerektirir. Şu an üretim kodunda paket yoktur (yalnızca .NET çerçevesi); test araçları açık kaynaktır.

## Açık nokta
- NuGet kilit dosyaları (`packages.lock.json`) şu an kapalı; ilk güvenilir restore sonrası açılıp depoya eklenecek.
