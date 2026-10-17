# ADR-003: Kullanıcı arayüzü yaklaşımı

Durum: Kabul (16 Ekim 2026)

## Karar
- Sunucu tarafı işleme: ASP.NET Core + Razor, etkileşim için htmx. Zayıf istemci bilgisayarlar ve kurum içi kullanım için hafif sayfa hedeflenir.
- Yalnızca belirtke matrisi gibi yoğun etkileşimli ekranlarda küçük bir JavaScript adası kullanılır (sıkıştırılmış ~300 KB bütçe).
- Erişilebilirlik hedefi WCAG 2.1 AA. Kültür: kullanıcıya dönük sıralama/biçimlendirme `tr-TR`, kimlik ve karşılaştırmalar Ordinal.
- Dilim 0'da yalnızca boş kabuk ve `/health` vardır; asıl ekranlar Dilim 1'den başlar.

## Ek (17 Ekim 2026): htmx yerel dosya
htmx 2.0.4 (`dist/htmx.min.js`, npm paketi `htmx.org`, lisans 0BSD) `wwwroot/js/` altına yerel olarak konuldu; CDN kullanılmaz. SHA-256 değeri `htmx.LICENSE.txt` içinde. Sürüm güncellemesi ADR notuyla yapılır.
