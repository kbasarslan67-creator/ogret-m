# ADR-004: Windows Hizmeti barındırma paketi

Durum: Kabul (17 Ekim 2026)

## Karar
Uygulama Windows Server'da hizmet olarak çalışacağı için `Microsoft.Extensions.Hosting.WindowsServices` üretim bağımlılığı olarak eklenir (ADR-002 ile uyumlu).

## Gerekçe ve alternatifler
- Microsoft paketi, .NET ile aynı sürüm çizgisinde; hizmet olarak çalışmayı tek satırda (`UseWindowsService`) açar.
- Alternatif: elle Windows hizmeti sarmalayıcısı veya üçüncü taraf araç. Bakım yükü ve lisans riski daha yüksek.

## Lisans
MIT (Microsoft.Extensions ailesi). Sürüm 10.0.13. Lisans metni nuspec'ten teyit edilecek (açık nokta).

## Doğrulama
Linux'ta konsol uygulaması olarak çalıştığı denendi. Gerçek Windows hizmeti kurulumu Dilim 9 kurulum senaryolarında ve Windows CI'da doğrulanır.
