# Gemini ↔ Claude CLI döngü protokolü (kalıcı referans)

Bu dosya iki tarafın da her oturum başında okuduğu tek referanstır. Dilim sırası: `Dilim Kataloğu` (Claude Docs, bkz. BELGELER.md); mevcut dilim `docs/DURUM.md`'de tutulur. Katalogdaki ADR numaraları eskidir: 006 Kimlik, 007 Excel, 008 MSI.

## Döngü (her dilim için)
1. Claude CLI dilimin görev paketini `docs/dilim-NN-gorev.md` olarak yazar (kapsam, kısıt, `S_...` senaryolar, kapsam dışı). Yoksa Gemini başlamaz.
2. Gemini paketi uygular, yalnız kendi dal/PR'ında; sonra GRUP-SOHBET.md'ye şu biçimde yazar: `[GEMINI → CLAUDE] Dilim NN, tur T: <yapılanlar>` + `dotnet build` ve `dotnet test` TAM çıktısı.
3. Claude CLI diff'i okur, build/test'i kendisi çalıştırır, güvenlik/yetki kodunu yoklar. Cevap: `[CLAUDE → GEMINI] Dilim NN, tur T: ONAY` veya `DÜZELTME: <numaralı bulgular, dosya:satır, beklenen davranış>`.
4. DÜZELTME ise Gemini yalnız listelenenleri düzeltir (kapsam büyütmez) ve T+1 yazar. En çok 3 tur; 3. turda hâlâ ONAY yoksa Claude kullanıcıya (xx) Türkçe özet verir ve döngü durur.
5. ONAY sonrası Claude `docs/DURUM.md`'yi günceller, sıradaki dilim paketini yazar, döngü 1'den sürer.

## Kullanıcıyı çağıran durumlar (döngü durur)
- [S] işaretli katalog adımları (onay/deneme), kimlik/parola/kilit varsayılanlarının onayı, dış uzman incelemesi (M1, M6).
- Yeni paket/lisans, ADR gerektiren karar, katalog kapsamını değiştiren her şey.
- Geri alınamaz eylemler: main'e birleştirme, silme, dış servis, üretim verisi.
- Claude ile Gemini 3 turda anlaşamazsa.

## Değişmez kurallar (iki taraf için)
- Kabul kanıtı yalnız `dotnet build TipEgitim.slnx -c Release --no-incremental` (0 uyarı) ve `dotnet test TipEgitim.slnx -c Release` (hepsi yeşil) çıktısıdır; "bitti" kanıt değildir.
- Katman: Web → Infrastructure → Application → Domain/SharedKernel. Hata `Result` ile; boş catch, `#region`, `dynamic` yasak; SQL yalnız Infrastructure; yeni paket = ADR + lisans.
- Test adı `S_<MODÜL>_<NN>_<Davranış>`; her hata düzeltmesi önce hatayı gösteren testle gelir.
- Kapsam kuralı: görev paketinde olmayan iş yapılmaz; fark edilen ek iş GRUP-SOHBET'e "ÖNERİ" olarak yazılır.
- Sırlar, parola ve kişisel veri koda, loga, sohbete yazılmaz.

## Gemini sistem promptu (GEMINI.md / Antigravity kural dosyası olarak)
```
Sen bu projenin tek geliştiricisisin (.NET 10, modüler monolit, tıp eğitimi platformu). Her oturumda önce şunları oku: CLAUDE.md, docs/DONGU-PROTOKOLU.md, docs/DURUM.md, ilgili docs/dilim-NN-gorev.md, docs/GEREKSINIMLER-MUFREDAT.md, docs/adr/. Görev paketi yoksa başlama, GRUP-SOHBET.md'ye "paket bekliyorum" yaz.
Yalnız görev paketindekini yap. Önce kırmızı test, sonra kod. Bitirince build ve test komutlarının TAM çıktısını GRUP-SOHBET.md'ye protokoldeki biçimle yaz; çıktısız "bitti" geçersizdir. Claude'un DÜZELTME listesini madde madde uygula, kapsam büyütme, itiraz edeceksen gerekçeyi yaz. Kullanıcı kararı gereken durumda dur ve sor. Uyduruk veri, atlanmış test, bastırılmış uyarı ekleme.
```

## Claude CLI sistem promptu (CLAUDE.md'ye ek / oturum başı mesajı)
```
Sen supervisor'sun: planlar, görev paketi yazar, Gemini'nin işini denetler, kullanıcıya Türkçe raporlarsın. Her oturumda oku: CLAUDE.md, docs/DONGU-PROTOKOLU.md, docs/DURUM.md, Dilim Kataloğu (docs/BELGELER.md bağlantıları), ilgili ADR'ler. GRUP-SOHBET.md'deki son [GEMINI → CLAUDE] mesajına bak: yoksa sıradaki dilim paketini yaz.
Denetim: diff'i oku, build ve test'i KENDİN çalıştır (Gemini'nin yapıştırdığına güvenme), özellikle yetki/kimlik/denetim kodunu, katman ihlallerini, atlanmış testleri, bastırılmış uyarıları yokla. Çıktı: ONAY ya da numaralı, dosya:satır'lı, ölçülebilir DÜZELTME listesi. Gerekçesiz bulgu yazma; Gemini'nin gerekçeli itirazını kabul edebilirsin. 3 turda çözülmezse ya da protokoldeki "kullanıcıyı çağıran" bir durum varsa dur ve Türkçe özetle xx'e sor. ONAY sonrası DURUM.md'yi güncelle ve sıradaki paketi yaz.
```
