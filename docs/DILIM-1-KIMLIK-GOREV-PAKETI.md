# Dilim 1 · Kimlik (asgari): görev paketi

Önkoşul: `DILIM-0-BULGULAR-KARAR.md` içindeki "ÖNCE" maddeleri birleşmiş ve build/test yeşil. Girdi belgeleri: `GEREKSINIMLER-MUFREDAT.md` (roller, S8), Dilim Kataloğu. Çıkış ölçütü: yetki matrisi testleri geçiyor; M1 dış incelemesinde kritik bulgu yok.

**Amaç:** her ekranın "kim, neyi, nerede yapabilir" sorusunu yanıtlayan yetki çekirdeği. Hata tüm modüllere yayıldığı için dilim M1 dış incelemesiyle kapanır.
**Kapsam dışı:** LDAP/AD (Dilim 9), oturum sıkılaştırma ve OIDC (Dilim 26), müfredat varlıkları (Dilim 2+).

## Kısıtlar
- Katman: Web → Infrastructure → Application → Domain/SharedKernel. SQL yalnız Infrastructure; SQLite'a özgü özellik yok (ADR-005 istisnası: sequence). Veritabanı değişikliği ileri yönlü numaralı migration (Dilim 1'de migration altyapısı kurulur; yeni tablolar buradan).
- Hata `Result` ile; boş catch, `#region`, `dynamic` yasak. Yeni paket yok (parola özeti: yerleşik `Rfc2898DeriveBytes.Pbkdf2`, SHA-256, ≥600 000 yineleme, 16 bayt rastgele tuz, `CryptographicOperations.FixedTimeEquals`); paket gerekirse ADR-006 + lisans.
- Varsayılan "reddet": açık izin yoksa erişim yok. Varsayılan parola yok. Parola, özet ve belirteçler asla loglanmaz ve denetim kaydına yazılmaz.
- Kullanıcı sayımı sızdırma: hatalı kullanıcı adı ile hatalı parola aynı mesaj ve benzer süre.
- Yapılandırılabilir varsayılanlar (kullanıcı onayı bekler, `Platform:Kimlik` bölümü, ValidateOnStart): kilit eşiği 5 başarısız deneme, kilit süresi 15 dk, boşta oturum 30 dk, mutlak oturum 8 saat, parola uzunluğu ≥12.
- Zaman `TimeProvider` ile; testlerde sahte saat. Cookie: HttpOnly, Secure, SameSite=Strict, yetkili POST'lar anti-forgery'li.
- Denetim yazılamazsa yetkiyle ilgili durum değiştiren işlem (rol atama) reddedilir (fail closed); giriş başarısız denemesinde audit hatası girişi yine reddeder, başarıyla geçirmez.
- Test adı `S_<MODÜL>_<NN>_<Davranış>`; modül kodları: KIM (kimlik/giriş), YET (yetki), ROL (rol atama), KUR (ilk kurulum), DEN (denetim), MIM (mimari), MUF (S-MUF). Önce kırmızı test.
- Rol listesi `GEREKSINIMLER-MUFREDAT.md` §4'tekidir (9 rol); rol adları yapılandırılabilir görünen ad, kod sabit anahtardır.

## Alan modeli (Domain/Application)
- `Kullanici` (Id, KullaniciAdi benzersiz-büyük/küçük harf duyarsız, AdSoyad, ParolaOzeti, Aktif, BasarisizDenemeSayisi, KilitBitisUtc?).
- `Rol` sabit anahtar kümesi: EgitimKoordinatoru, DonemKoordinatoru, BirimSorumlusu, AnabilimDaliSorumlusu, OgretimUyesi, EgitimKomisyonuUyesi, KaliteAkreditasyon, Ogrenci, SistemYoneticisi.
- `Kapsam` (Tur: Program|Donem|Birim|Ders|AnabilimDali, KapsamId: string; hedef varlıklar henüz yok, FK yok). Program kapsamı için KapsamId boş değil sabit program kimliği.
- `RolAtamasi` (KullaniciId, Rol, Kapsam, AtayanId, AtamaZamaniUtc). Aynı (kullanıcı, rol, kapsam) tekrarlanamaz. Bir kullanıcı birden çok atama taşır.
- `Izin` (sabit): KullaniciYonet, RolAta, MufredatGor, MufredatDuzenle, HedefYaz, BirimOnOnayla, DonemIncelemeyeGonder, SurumKabulIade, SurumYayinla, DegisiklikGecmisiGor, TeknikAyarDuzenle. Rol→izin→kapsam-türü eşlemesi tek tabloda (Domain), matristen üretilir:
  - EgitimKoordinatoru(Program): MufredatGor, MufredatDuzenle, SurumYayinla
  - DonemKoordinatoru(Donem): MufredatGor, MufredatDuzenle, DonemIncelemeyeGonder
  - BirimSorumlusu(Birim): MufredatGor, MufredatDuzenle, BirimOnOnayla
  - AnabilimDaliSorumlusu(AnabilimDali): MufredatGor (+ eğitici ataması izni sonra eklenir)
  - OgretimUyesi(Ders): MufredatGor, HedefYaz
  - EgitimKomisyonuUyesi(Program): MufredatGor, SurumKabulIade
  - KaliteAkreditasyon(Program): MufredatGor, DegisiklikGecmisiGor
  - Ogrenci(Donem): MufredatGor (yalnız yayınlanmış; görünürlük kuralı Dilim 2+)
  - SistemYoneticisi(Kurulum): KullaniciYonet, RolAta, TeknikAyarDuzenle; **MufredatDuzenle/HedefYaz/Onay izinleri YOK**.

## Katman beklentileri
**Domain/SharedKernel:** `Kapsam` kapsama ilişkisi (üst kapsam alt kapsamı içerir mi: şimdilik yalnız eşitlik ve Program⊇hepsi); `YetkiPolitikasi.Izinli(atamalar, izin, hedefKapsam)` saf fonksiyon, varsayılan reddet; parola kuralı doğrulama; kilit hesabı saf mantığı (sayaç, eşik, süre).
**Application:** komutlar `Giris`, `Cikis`, `IlkYoneticiOlustur`, `RolAta`, `RolKaldir`, `KullaniciOlustur`; hepsi `Result` döner, hata kodları `identity.*` / `authz.*` (örn. identity.invalid_credentials, identity.locked, identity.weak_password, authz.forbidden); denetim `AuditRecorder` ile; portlar `IKullaniciDeposu`, `IRolAtamaDeposu`, `IParolaOzeti`, `ISimdi` (TimeProvider).
**Infrastructure:** SQLite depoları (migration'lı `kullanici`, `rol_atamasi`), PBKDF2 uygulaması, parametreli sabit SQL, benzersiz indeks (kullanici_adi nocase, atama üçlüsü).
**Web:** cookie kimlik doğrulama, `/giris`, `/cikis`, ilk kurulum sayfası, BT yöneticisi için kullanıcı ve rol atama ekranları, her sayfa/işleyici `[Authorize]`+izin politikası (varsayılan: tüm uç noktalar kimlik ister, anonim yalnız /giris ve /kurulum), 403 sayfası, htmx yerel, Türkçe metinler `UiStrings`'te.

## Kabul senaryoları ve test adları
**Giriş / parola / kilit (KIM)**
- S_KIM_01_ParolaOzetiDuzMetinIcermezVeHerKullaniciIcinFarklidir
- S_KIM_02_DogruParolaIleGirisBasarilidirVeSayacSifirlanir
- S_KIM_03_YanlisParolaVeBilinmeyenKullaniciAyniHatayiDoner
- S_KIM_04_BesBasarisizDenemedenSonraHesapKilitlenir
- S_KIM_05_KilitSuresiDolunca GirisYenidenMumkundur (sahte saat)
- S_KIM_06_KilitliHesapDogruParolaylaDaGirisYapamaz
- S_KIM_07_ZayifParolaReddedilir (uzunluk <12, kullanıcı adıyla aynı)
- S_KIM_08_BostaOturumSuresiDolunca KimlikGecersizOlur (30 dk) ve S_KIM_09_MutlakOturumSuresiAsilirsaYenidenGirisGerekir
- S_KIM_10_CikisCerezinIptalEder
- S_KIM_11_CerezHttpOnlySecureSameSiteStrictOlarakKurulur
**Yetki politikası (YET)**
- S_YET_01_AtamasizKullaniciHerSeyiReddedilir (varsayılan reddet)
- S_YET_02_RolKapsamIcindeIzinVerir ve S_YET_03_AynıRolBaskaKapsamdaReddedilir (örn. dönem koordinatörü başka dönemde düzenleyemez)
- S_YET_04_ProgramKapsamiAltKapsamlariKapsar
- S_YET_05_BirdenCokRolunIzinleriBirlesir (hem dönem koordinatörü hem öğretim üyesi)
- S_YET_06_YetkiMatrisiHerRolXHerIzinBeklenenSonuc (veri güdümlü teori testi: 9 rol × 11 izin × uygun/uygunsuz kapsam; beklenenler yukarıdaki eşlemeden elle doldurulur, koddan üretilmez)
- S_YET_07_SistemYoneticisiMufredatDuzenleyemez (S8 çekirdeği: MufredatDuzenle, HedefYaz, BirimOnOnayla, SurumYayinla hepsi reddedilir; KullaniciYonet, RolAta izinli)
- S_YET_08_ReddedilenIslemAuthzForbiddenDonerVeDenetimeYazilir
**Rol atama (ROL)**
- S_ROL_01_SadeceKullaniciYonetIzniOlanRolAtayabilir
- S_ROL_02_AyniUclununTekrarAtanmasiReddedilir
- S_ROL_03_RolKaldirmaKaydiSilmezDenetimeYazar (atama geçmişi korunur)
- S_ROL_04_SonSistemYoneticisiKaldirilamaz
**İlk kurulum (KUR)**
- S_KUR_01_KullaniciYokkenKurulumSayfasiAcilirDigerleriGiriseYonlendirir
- S_KUR_02_YoneticiOlusturulduktanSonraKurulumSayfasiKapanir (ikinci çağrı 404/403)
- S_KUR_03_VarsayilanParolaYokYoneticiParolasiKurulumdaGirilir
- S_KUR_04_KurulumYarisiTekYoneticiOlusur (eşzamanlı iki istek)
**Denetim (DEN)**
- S_DEN_01_BasariliGirisDenetimeYazilir; S_DEN_02_BasarisizGirisDenetimeYazilirParolaYoktur; S_DEN_03_RolDegisikligiKimKimeHangiRolHangiKapsamGerekce YazilirDenetimdeParolaVeOzetYoktur
- S_DEN_04_DenetimYazilamazsaRolAtamaGeriAlinir/Reddedilir (fail closed)
**Mimari (MIM)**
- S_MIM_10_TumSayfalarVeUcNoktalarYetkiPolitikasiTasir (yansıma ile: anonim yalnız izinli listede)
- S_MIM_11_ParolaOzetiVeKimlikSinifiLogaYazilmaz (ILogger çağrılarında hassas alan adı taraması)
- S_MIM_12_SqlSadeceInfrastructurede (var olan kuralı genişlet)
**Web / uçtan uca**
- S_MUF_08_BtYoneticisiHedefDuzenlemeyiDenerKullaniciVeRolEkranlariCalisir (katalogdaki S8)
- S_MUF_14_KapsamDisiIslemHttp403DonerVeDenetimeGirer (kapsam dışı dönem/ders; yanıtta iç ayrıntı yok)
- S_MUF_15_AnonimIstekGiriseYonlendirilir
- S_MUF_16_AntiForgeryOlmayanPostReddedilir

## Ek teslimatlar
- Yetki matrisi belgesi `docs/YETKI-MATRISI.md` (rol × izin × kapsam, testlerle aynı veri).
- Kimlik tehdit modeli `docs/TEHDIT-MODELI-KIMLIK.md` (STRIDE: kimlik taklidi, kaba kuvvet/kilit-DoS, oturum çalma, yetki yükseltme, denetim silme/atlatma, kurulum yarışı; her tehdit için önlem ve ilgili test kimliği).
- ADR-006 Kimlik (parola özeti algoritması/parametreleri, oturum süreleri, çerçeve başvurusu kararı = Bulgu 5b, yeni paket varsa lisans).
- Kullanıcıdan beklenenler: rol adları/listesinin onayı; kilit/oturum süreleri varsayılanlarının onayı; M1 dış uzman incelemesi (kimlik kodu).

## Çalışma sırası önerisi (küçük PR'lar)
1. Migration altyapısı + ADR-006 + Bulgu 5b. 2. Domain: kapsam, izin eşlemesi, YetkiPolitikasi + S_YET_01–07. 3. Parola özeti + kilit + S_KIM. 4. Depolar + ilk kurulum + S_KUR. 5. Web giriş/cookie/politika + S_MUF_15/16, S_MIM_10. 6. Rol atama + denetim + S_ROL, S_DEN, S_MUF_08/14. 7. Matris belgesi + tehdit modeli. Her PR sonunda tam build/test çıktısı kanıttır.
