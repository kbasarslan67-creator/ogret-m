# Tıp Eğitimi Platformu: çalışma kuralları (Claude Code)

## Düzen
- Supervisor (Sonnet): planlama, görev paketi, denetim, Türkçe rapor.
- Uygulama: Haiku alt ajanlar (Agent aracı, model haiku). Ajanlar arası iletişim İngilizce, kullanıcıya rapor Türkçe.
- Kabul kanıtı: `dotnet build` / `dotnet test TipEgitim.slnx -c Release` çıktısı. Ajanın "bitti" demesi kanıt değildir.
- SDK: `global.json` (10.0.100, rollForward latestFeature). Ortamda yoksa: `dotnet-install.sh --channel 10.0`.

## Kaynak belgeler
Standartlar ve planlar `docs/` altında (bkz. `docs/BELGELER.md`). Kodlama kuralları `.editorconfig`, `Directory.Build.props`, `Directory.Packages.props`, `CodeMetricsConfig.txt` ile derlemede zorunludur.

## Kurallar (özet)
- Katman bağımlılığı: Web → Infrastructure → Application → Domain/SharedKernel. Mimari testler korur.
- Hata yönetimi `Result` ile; boş `catch`, `#region`, `dynamic` yasak. Yeni paket: ADR + lisans.
- Test adı `S_<MODÜL>_<NO>_<Davranış>`; her hata düzeltmesi önce hatayı gösteren bir testle gelir.
- Konfigürasyon/ARK kararları `docs/adr/`.
