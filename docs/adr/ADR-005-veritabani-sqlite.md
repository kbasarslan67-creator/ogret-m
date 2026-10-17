# ADR-005: Veritabanı olarak SQLite

Durum: Kabul (17 Ekim 2026). ADR-002'deki PostgreSQL kararının yerini alır.

## Karar
İlk sürümlerde veritabanı SQLite'tır (`Microsoft.Data.Sqlite`, MIT). Kurulum, ayrı sunucu ve bağlantı yönetimi gerektirmez; veri tek dosyadır (WAL kipi açık).

## Gerekçe
Kurulum ve geliştirme hızı. Müşteri BT'sine ek yazılım kurdurmak gerekmez; yedekleme dosya kopyalamaktır.

## Sınırlar ve risk
- Tek yazar: eşzamanlı yoğun yazma (yoklama anı) için ölçülecek (Dilim 6/22 yük testi). Yetmezse veri erişimi arayüzü sayesinde PostgreSQL'e geçilir; bu yüzden SQL yalnızca Infrastructure katmanında yazılır, SQLite'a özgü özellik kullanılmaz.
- Zaman damgaları UTC metin (ISO 8601); GUID'ler metin.
- Silme yok, denetim kaydı yalnızca eklemeli (Standart: Veri erişimi).
- Şema değişikliği: sürümlü geçiş betikleri (Dilim 1'de, ilk gerçek tabloyla).
