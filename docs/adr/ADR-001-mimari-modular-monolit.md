# ADR-001: Modüler monolit + Clean Architecture

Durum: Kabul (16 Ekim 2026) · Karar veren: Claude (teknik), kullanıcı bilgilendirildi

## Bağlam
Ürün, üniversite tıp fakültelerinin kendi sunucusuna (Windows Server) kurulacak. Zayıf istemci bilgisayarlar var; ekip küçük. Birden çok bağımsız dağıtım birimi işletmek müşteri BT'si için yük olur.

## Karar
- Tek dağıtım birimi (monolit), içte modüller. Her modül kendi şemasını (PostgreSQL schema) ve `Contracts` projesini taşır; modüller yalnızca birbirinin `Contracts` projesine bağımlı olabilir.
- Katmanlar: `SharedKernel` ← `Application` ← `Infrastructure` ← `Web`. Bağımlılıklar yalnızca içe doğru akar; kural `ArchitectureTests` ile her derlemede doğrulanır.
- Komut/sorgu yönlendirmesi kendi küçük `Dispatcher` sınıfımızla yapılır; MediatR ve AutoMapper kullanılmaz (ticari lisans değişiklikleri; paket bağımlılığı azaltma).
- Beklenen hatalar `Result` ile döner; istisna yalnızca programlama hatası ve altyapı arızası içindir.

## Sonuçlar
- Olumlu: tek kurulum, tek yedek, modül sınırları test edilebilir, ileride bir modül ayrılabilir.
- Olumsuz: modül sınırlarını disiplinle korumak gerekir (mimari testler bunun için var).

## Standart notu
Hiçbir standart n-katmanlı ya da Clean Architecture dayatmaz; seçim bu ADR ile kayıt altındadır.
