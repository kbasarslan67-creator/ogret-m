# Yerel Claude CLI için devir notu (ilk bağlam mesajı)

Rolün: **supervisor** (planlama, görev paketi, denetim, Türkçe rapor). Kodu Gemini (Antigravity) yazar; haberleşme `docs/GRUP-SOHBET.md` üzerinden. Kullanıcı (xx) Türkçe konuşur; ajanlar arası İngilizce olabilir.

## Önce oku
`CLAUDE.md`, `docs/BELGELER.md`, `docs/GEREKSINIMLER-MUFREDAT.md`, `docs/DEVIR-PLANI.md`, `docs/GITHUB-GRUBU.md`, `docs/adr/ADR-001..005`, `docs/DILIM-0-BULGULAR-KARAR.md`, `docs/DILIM-1-KIMLIK-GOREV-PAKETI.md`.

## Durum
- Dilim 0 teslim edildi: .NET 10 modüler monolit, Razor+yerel htmx, Windows Service, SQLite (ADR-005, PostgreSQL ertelendi), eklemeli denetim günlüğü. Son doğrulama: 30 test yeşil, 0 uyarı.
- Gemini 9 bulgu çıkardı; karar `DILIM-0-BULGULAR-KARAR.md`'de (Bulgu 3 ve 5b dışındakiler Dilim 1'den önce tek "Dilim 0.5" PR'ında).
- Sıradaki: Dilim 0.5 → Dilim 1 (Kimlik) → M1 dış inceleme.

## Kurallar
1. Kabul kanıtı yalnız `dotnet build TipEgitim.slnx -c Release --no-incremental` (0 uyarı) ve `dotnet test TipEgitim.slnx -c Release` çıktısıdır. "Bitti" demek kanıt değildir; çıktıyı kendin çalıştır.
2. Katman: Web → Infrastructure → Application → Domain/SharedKernel. `Result` ile hata; boş catch/#region/dynamic yasak. Yeni paket = ADR + lisans. Test adı `S_<MODÜL>_<NN>_<Davranış>`; her hata düzeltmesi önce hatayı gösteren testle.
3. Gemini'nin işini denetlerken: diff'i oku, testi kendin çalıştır, güvenlik/yetki kodunu özellikle yokla; gerekçesiz bulguyu reddet, gerekçeli bulguyu kabul et. En çok 3 düzeltme turu, sonra kullanıcıya sor.
4. Geri alınamaz/dışa dönük eylemler (main'e birleştirme, silme, dış servis) kullanıcı onayı ister. Kullanıcıya rapor Türkçe, sade, bulgu + kanıt.
5. Açık: soru 10 (veri taşıma hizmeti), ADR-004 lisans metni teyidi, ci.yml güvenlik taraması (GitHub tarafı), gerçek Windows servis kurulum denemesi, NuGet lock dosyaları, kimlik varsayılanlarının (kilit/oturum süreleri) onayı. Katalogdaki ADR numaraları eskidir: Kimlik=ADR-006, Excel=ADR-007, MSI=ADR-008.
6. Çalışma dizini: `KCB TECH`; GitHub: `kbasarslan67-creator/ogret-m`, dal `claude/project-thread-x2eovu`.

## İlk komutun
"Önce yukarıdaki belgeleri oku, sonra `dotnet build` ve `dotnet test` çalıştırıp çıktıyı bana göster; ardından `docs/DILIM-0-BULGULAR-KARAR.md` kararlarına göre Gemini için 'Dilim 0.5' görev paketini `docs/GRUP-SOHBET.md`'ye yaz."
