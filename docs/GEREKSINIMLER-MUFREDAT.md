# Müfredat modülü gereksinim özeti

Kaynak: Claude Docs "Müfredat Yapısı: Modül Gereksinim Taslağı" (16 Ekim 2026, rev 22), "Kurumsal Tıp Eğitimi Platformu: Proje Planı ve İş Paketi Kırılımı" ve "Dilim Kataloğu". Bu dosya kaynakların yerel kopyasıdır; çelişkide Claude Docs belgesi geçerlidir. Kimlikler (K, S, WP, M) kaynaktaki gibidir ve test adlarında/izlenebilirlikte kullanılır.

## 1. Yapı ve terimler
Program → Müfredat sürümü (akademik yıl) → Dönem → Eğitim birimi (ders kurulu, klinik staj, intörnlük rotasyonu, seçmeli) → Ders → Öğrenim hedefi. Anabilim dalı ve program çıktıları ağaca yandan bağlanır.

| Terim | Tanım |
|---|---|
| Program | Fakültenin verdiği lisans programı (ilk sürümde tek program) |
| Müfredat sürümü | Bir programın bir akademik yıldaki yapısının tamamı; yayınlanınca donar |
| Dönem | Programın yıl kademesi; sayısı programda tanımlı (varsayılan 6) |
| Eğitim birimi | Dönem içindeki ders kurulu, klinik staj, intörnlük rotasyonu veya seçmeli |
| Birim türü | Birimin cinsi; kurum kendi türlerini ekleyebilir |
| Ders | Birim içinde konu başlığı olan tek öğretim etkinliği; sorumlu anabilim dalı vardır ("ders" kelimesi yalnızca bunu anlatır; üst düzey "eğitim birimi") |
| Öğrenim hedefi | Dersin sonunda öğrencinin yapabileceği ölçülebilir tek davranış |
| Oturum | Dersin takvimdeki gerçekleşmesi (takvim modülünün) |
| Anabilim/Bilim dalı | Dersi veren akademik birim |
| Program çıktısı | Mezunun kazanması beklenen yeterlik; programa ve sürüme bağlı |
| Kalıcı kimlik | Birim/ders/hedefin yıllar boyu aynı kalan kimliği (adı değişse de) |

## 2. İş kuralları
| # | Kural | Neden / kaynak |
|---|---|---|
| K1 | Kimlik ad değil, kalıcı kimliktir; aynı adlı birden çok ders olabilir | Aynı konu başlığı farklı kurullarda tekrar ediyor |
| K2 | Birim kodu ve sırası serbest biçimli, ayrı alanlardır | "1.1 …" ve "4- …" iki numaralama birlikte kullanılıyor |
| K3 | Dönem sayısı programda yapılandırılır | Örnek raporlarda Dönem VI yok |
| K4 | Bir dersin tek sorumlu anabilim dalı vardır; katkı veren dallar ayrıca eklenebilir | Ortak verilen dersler |
| K5 | Yayınlanmış sürümde hiçbir kayıt silinmez, yalnızca sonraki yılda kaldırılır | Geçmiş raporlar/akreditasyon kanıtı değişmemeli |
| K6 | Soru veya belirtke bağlantısı olan hedef taslakta da silinemez; pasifleştirilir | Soru bankası/sınav geçmişi kopmamalı |
| K7 | Taşınan birim veya ders kimliğini ve hedeflerini korur | Yıllar arası karşılaştırma |
| K8 | Öğrenim hedefi tek davranış içerir; ölçülemeyen fiiller ("bilir", "anlar") uyarı üretir, engellemez | Belirtke ve soru yazımı hedefe dayanır |
| K9 | Bir kişi ancak atandığı dersin hedefini yazar | "Eğitici yalnızca kendi dersini görür" |
| K10 | Ders başına planlanan saat ve yöntem (teorik, pratik, klinik) isteğe bağlıdır; takvimdeki gerçekleşenle farkı raporlanır | Raporlarda saat ağırlığı eksik |
| K11 | Eski yıl verisi içeri alınırken çelişkili düzey kodları ("T-TT" gibi) uyarı olarak listelenir | Örnek veride 6 kez geçiyor |
| K12 | Yapı değişiklikleri kim, ne, ne zaman ve gerekçe ile kaydedilir | TEPDAD 2025 standart 6.3 (denetim izi ve belge arşivi); eski kod MÖTE TS.5.2 |

Diğer kurallar (belgenin gövdesinden):
- Yayındaki yılda birim/ders/hedef ekleme-kaldırma-taşıma: gerekçeli ve denetim kayıtlı düzeltme; silme yerine pasifleştirme. Yazım düzeltmesi: gerekçe + denetim kaydı. Eğitici ataması serbestçe değişir (yapı değil görevlendirme). UÇEP/program çıktısı eşlemesi ayrı onayla güncellenir, geçmişi tutulur.
- Aynı adlı derse uyarı verilir, engellenmez. Sürüm durumları: Taslak → İnceleme → Yayınlanmış (iade edilince sorumlusuna döner).
- Yeni yıl açma: kaynak yayınlanmış son yıl; önizleme (birim/ders/hedef/atama sayıları); onaydan sonra "Taslak"; kalıcı kimlikler korunur; eğitici atamaları isteğe bağlı boş bırakılır.
- Onay zinciri iki basamaklı: birim sorumlusu ön onayı + komisyon onayı (kurum yapılandırmayla basamak kapatabilir). Yıllar arası karşılaştırma: eklenen/kaldırılan/taşınan/adı değişen.
- İçe aktarma: tek standart Excel şablonu; yükle-önizle (hiçbir şey yazılmadan) → hata listesi (satır no, bozuk karakter kodlaması, birleşik hücre, eksik anabilim dalı, bilinmeyen dönem, çelişkili düzey) → onay (denetim kaydı, geri alınabilir) → dışa aktar (aynı şablon).
- İlk sürüme alınmayanlar: staj rotasyon sırası (takvim modülü), yatay/dikey entegrasyon görünümü.

## 3. Kararlar (16 Ekim 2026)
Kabul (önerim): 1 eşleme hem hedef hem ders düzeyi (ders düzeyi "eksik ayrıntı" görünür); 5 hedefi öğretim üyesi yazar, birim sorumlusu onaylar; 6 planlanan saat modülde tutulur, takvim farkı uyarı; 7 program çıktıları müfredattan bağımsız ayrı sürümlü liste; 8 öğrenciler yayınlanmış hedefleri görür.
Diğerleri: Dönem VI ilk sürümde yok; yayındaki yılda revizyon değil gerekçeli+denetim kayıtlı düzeltme; onay iki basamaklı; ilk sürüm tek program (veri modeli çoklu programa açık); soru 10 (veri taşıma hizmeti mi şablon mu) AÇIK.

Güncelleme (17 Ekim 2026, Nihai Fonksiyon Listesi kararları, ayrıntı `FONKSIYON-LISTESI-ESLEME.md`): birim kazanımları K1…Kn ve ders hedefleri birlikte tutulur, ders hedefleri birim kazanımlarına bağlanır; ders × program çıktısı ilişkisi 0–5 güçle tutulur; hedef yazım denetimi K8'i genişletir (ders başına 5–7 hedef, ölçülebilir fiil, Bloom düzeyi, yöntem ve süre doluluğu; uyarı, engel değil); UÇEP eşlemesinde yapay zekâ önerisi 2. sürümde (öneri kaydedilmez, insan karar verir).

## 4. Onaylı roller ve kapsam
Yetki = rol (ne yapılabilir) + kapsam (nerede). Bir kişi birden çok rol taşıyabilir. Rol adları ekranda yapılandırılabilir.

| Rol | Kapsam | Görür | Düzenler | Onaylar/yayınlar |
|---|---|---|---|---|
| Eğitim koordinatörü (baş) | Program | Tüm sürümler | Dönem ve birim yapısı, program çıktıları | Yeni yıl açar; komisyon kararından sonra yayınlar |
| Dönem koordinatörü | Bir dönem | Kendi dönemi + diğerleri salt okunur | Dönemindeki birimler ve ders listeleri | Dönemini incelemeye gönderir |
| Birim sorumlusu (kurul/staj) | Bir eğitim birimi | Kendi birimi | Birimin dersleri, ders–anabilim dalı ataması | Birimin hedeflerini ön onaylar |
| Anabilim dalı sorumlusu | Bir anabilim dalı | Dalın verdiği tüm dersler | Eğitici ataması | — |
| Öğretim üyesi | Atandığı dersler | Atandığı dersler + birimin genel yapısı | Kendi derslerinin öğrenim hedefleri | — |
| Eğitim komisyonu üyesi | Program | Tüm sürüm | — | İncelemedeki sürümü kabul/iade eder |
| Kalite/akreditasyon birimi | Program | Tüm sürümler + değişiklik geçmişi | — | — |
| Öğrenci | Kayıtlı olduğu dönem | Yayınlanmış yapı ve hedefler | — | — |
| Sistem yöneticisi (BT) | Kurulum | Teknik ayarlar | Rol atamaları, LDAP eşlemesi | —; müfredat içeriğini düzenleyemez |

İlke: akademik yetki ile teknik yetki ayrıdır; BT yöneticisi kullanıcı/rol yönetir, müfredata dokunamaz.

## 5. Kabul senaryoları (tam metin)
| # | Durum | Eylem | Beklenen sonuç |
|---|---|---|---|
| S1 | 2025–2026 sürümü yayınlanmış | Eğitim koordinatörü 2026–2027'yi bu yıldan açar | Önizlemede kopyalanacak birim, ders ve hedef sayıları görünür; onaydan sonra yeni sürüm "Taslak" olur, eski yıl değişmez |
| S2 | Öğretim üyesi A yalnızca "Prenatal genetik tanı" dersine atanmış | A giriş yapar | Yalnızca bu dersin hedeflerini düzenleyebilir; aynı kuruldaki başka dersi düzenle düğmesi görünmez |
| S3 | Bir hedefe bağlı 3 soru var | Birim sorumlusu hedefi silmeye çalışır | Silme engellenir, "pasifleştir" seçeneği sunulur ve 3 sorunun listesi gösterilir |
| S4 | 2026–2027 yayınlanmış | Dönem koordinatörü bir dersin adındaki yazım hatasını düzeltir | Düzeltme gerekçe ister; denetim kaydında eski ve yeni ad görünür |
| S5 | Bir ders 3.2'den 3.3 kuruluna taşınır | İki yıl karşılaştırılır | Ders "taşındı" olarak görünür, "silindi + eklendi" olarak değil; hedefleri korunur |
| S6 | Hazırlanmış şablonda 2 satırda bozuk karakter, 1 satırda eksik anabilim dalı var | Dosya yüklenir | Hiçbir şey yazılmadan 3 hata satır numarasıyla listelenir |
| S7 | Taslak sürümde birim sorumlusu ön onayı verilmemiş bir birim var | Koordinatör yayınlamaya çalışır | Yayınlama engellenir, eksik onaylar listelenir |
| S8 | BT yöneticisi giriş yapmış | Bir dersin hedefini düzenlemeye çalışır | Düzenleme yetkisi yoktur; kullanıcı ve rol ekranları çalışır |
| S9 | Bir hedef "bilir" fiiliyle yazılır | Kaydedilir | Kayıt yapılır ama "ölçülebilir fiil önerisi" uyarısı görünür |

Katalogdaki yeni senaryo: S-MUF-14 — kapsam dışı işlem 403 döner ve denetim kaydına girer (Dilim 1).

## 6. Proje planı: iş paketleri (WP0–WP14) ve kilometre taşları (M0–M7)
Not: WP1..WP12 / M1..M7 değil; gerçek plan WP0–WP14 ve M0–M7'dir.

| Paket | Ad | Ana çıktı | Bağımlılık | Bitti ölçütü |
|---|---|---|---|---|
| WP0 | Proje yönetimi ve yönetişim | Plan, risk kaydı, değişiklik yönetimi, aylık rapor | — | Plan onaylı, risk kaydı haftalık güncel |
| WP1 | Gereksinim ve alan analizi | Kullanıcı hikâyeleri, alan sözlüğü, kapsama metrikleri, düzey karşılaştırma kuralları | WP0 | Fakülte temsilcisi onaylı gereksinim seti |
| WP2 | Mimari ve yürüyen iskelet | ADR kılavuzu, çözüm yapısı, CI, katman testleri, uca uca boş dilim | WP1 | Windows Server'a kurulan iskelet CI'dan geçiyor |
| WP3 | Kimlik ve yetki | Giriş, roller, kaynak bazlı yetki, LDAP/AD, denetim izi | WP2 | Yetki matrisi testleri geçiyor |
| WP4 | Müfredat ve akademik takvim | Program/dönem/ders yapısı, yıla göre değişmez sürümler | WP3 | İki akademik yıl sürümü yan yana tutulabiliyor |
| WP5 | UÇEP içerik yönetimi | UÇEP 2020 paketi, düzey modelleri, imza doğrulamalı yükleme | WP4 | Paket yüklenip sürüm olarak görüntülenebiliyor |
| WP6 | Belirtke tablosu | Ağaç, soru sayacı, Excel alım/dağıtım, etkileşimli matris | WP4 | Binlerce satırda sayfalama, JS bütçesi aşılmıyor |
| WP7 | Yoklama | Oturum, QR, alternatif giriş, devamsızlık hesabı | WP4 | Hesap kuralları yapılandırılabilir ve test edilmiş |
| WP8 | Soru bankası | Soru türleri, sürüm/onay akışı, belirtkeye bağlama | WP6 | Onaylı soru belirtke sayacını güncelliyor |
| WP9 | Eşleştirme ve raporlama | UÇEP–belirtke eşleştirme, kapsama kural motoru, paneller, kanıt paketi | WP5, WP6, WP8 | Dört kapsama sorusu örnek veride yanıtlanıyor |
| WP10 | Arayüz, erişilebilirlik, zayıf istemci | Ortak bileşenler, klavye/ekran okuyucu, performans bütçesi | WP2 | Hedef istemcide ölçüm raporu |
| WP11 | Kurulum, güncelleme, yedekleme | MSI, çevrimdışı güncelleme, yedek/geri alma, tanı paketi | WP2 | Temiz Windows Server'da kurulum/güncelleme/geri yükleme |
| WP12 | Kalite, performans, yük testleri | Test piramidi, mutasyon testi, yük senaryoları | WP3–WP9 | Boyutlandırma hedefleri ölçümle doğrulanmış |
| WP13 | Güvenlik, KVKK, lisans | Tehdit modeli, sızma testi, veri envanteri, SBOM, lisans anahtarı | WP2 | Kritik bulgu yok, lisans taraması temiz |
| WP14 | Pilot, eğitim, destek | Pilot kurulum, kılavuzlar, eğitim, destek süreci | WP11, WP12, WP13 | Pilot fakülte üretimde |

| Taş | Ad | Paketler | Çıkış ölçütü |
|---|---|---|---|
| M0 | Başlangıç | WP0, WP1 | Gereksinimler onaylı |
| M1 | Yürüyen iskelet | WP2 | Windows Server'da iskelet CI'dan geçer (+ dış uzman iskelet ve kimlik kodu incelemesi) |
| M2 | Kimlik, müfredat | WP3, WP4 | Yetki testleri geçer, iki yıl sürümü var |
| M3 | UÇEP, belirtke | WP5, WP6 | Paket yüklenir, matris bütçede |
| M4 | Soru, yoklama | WP7, WP8 | Onaylı soru belirtke sayacını günceller |
| M5 | Eşleştirme, rapor | WP9 | Eksik listesi ve kanıt paketi üretilir |
| M6 | Sertleşme | WP10–WP13 | Yük ve güvenlik kapısı geçilir (dış denetim, sızma testi) |
| M7 | Pilot | WP14 | Pilot fakülte üretimde |

Kural: çıkış ölçütü karşılanmadan sonraki aşamaya geçilmez.
