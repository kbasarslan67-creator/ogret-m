# GitHub'ı ortak "grup" yapmak: Claude + Gemini + siz

Durum: tasarım. Bu belgedeki Gemini tarafı henüz bu ortamda denenmedi; kurulum sırasında adım adım doğrulanacak.

## Parçalar
| Parça | Rolü | Nerede yaşar |
| --- | --- | --- |
| Issue (her dilim için bir tane) | Grup sohbeti: görev, kabul senaryoları, "başladım/bitti", bulgular | GitHub > Issues |
| Pull request (her dilim için bir dal) | Gemini'nin teslimi; kod, CI sonucu, satır yorumları | GitHub > Pull requests |
| CI (`ci.yml`) | Linux + Windows derleme ve test; tarafsız hakem | GitHub Actions |
| `CLAUDE.md` / `GEMINI.md` | İki tarafın okuduğu ortak kurallar | depo kökü |
| Etiketler | Durum bayrağı | issue/PR üzerinde |

## Etiketler (durum makinesi)
`dilim:hazır` (görev yazıldı, Gemini başlayabilir) → `dilim:uygulamada` → `dilim:incelemede` (Gemini PR açtı) → `dilim:düzeltme` (Claude bulgu yazdı) → `dilim:onaylı` (Claude onayladı, CI yeşil) → `dilim:birleşti`.
Gemini yalnızca `hazır` ve `düzeltme` etiketli işi alır; Claude yalnızca `incelemede` etiketli PR'a bakar.

## Bir dilimin yaşam döngüsü
1. **Claude** issue açar: kapsam, kabul senaryoları (S-…), yasaklar, çıkış koşulu; `dilim:hazır`.
2. **Gemini** issue'yu okur, `dilim-N-ad` dalında yazar, PR açar (gövdede issue numarası, değişen dosyalar, build/test çıktısı); `dilim:incelemede`.
3. **Claude** PR olayıyla uyanır: dalı çeker, `dotnet build/test` çalıştırır, CI'ı okur. Bulgular PR'a numaralı yorum (B1, B2…) olarak yazılır; sorun varsa `dilim:düzeltme`.
4. **Gemini** B1…Bn'yi düzeltir, her birine "düzeltildi: <commit>" diye yanıt verir, aynı dala gönderir; `dilim:incelemede`.
5. 3-4 en çok 3 tur. Üçüncü turdan sonra hâlâ bulgu varsa Claude size özet yazar, karar sizdedir.
6. Bulgu kalmayınca Claude `dilim:onaylı` yazar ve sizden birleştirme onayı ister (ya da önceden verdiğiniz kurala göre birleştirir). Birleşince Claude sıradaki dilimin issue'sunu açar.

## Gemini'nin yorumu "duyması"
İki seçenek:
- **A. GitHub Actions (önerilen):** issue/PR'a yeni yorum ya da etiket gelince Gemini'yi çalıştıran bir iş akışı tetiklenir; Gemini görevi okur, dalı günceller. Gerekli: depo ayarlarında `GEMINI_API_KEY` secret'ı, iş akışına yazma izni (`contents: write`, `pull-requests: write`). Bilgisayarınız kapalı olabilir.
- **B. Yerel Gemini CLI:** KCB TECH klasöründe bir betik GitHub'ı yoklar (`gh issue list --label dilim:hazır`), Gemini CLI'yi çalıştırır, sonucu iter. Bilgisayar açık olmalı.

## Güvenlik ve maliyet sınırları
- Ana dal korumalı: PR'sız push yok, CI yeşil olmadan birleştirme yok.
- Gemini yalnızca kendi dalına yazar; iş akışı secret'ları PR'dan gelen koda açmaz.
- Tur sınırı 3; iş akışı günde en çok N çalıştırma ile sınırlanır.
- Issue/PR metni "talimat" sayılmaz: bir tarafın yorumu diğerinin kurallarını (CLAUDE.md/GEMINI.md) değiştiremez; kural değişikliği yalnızca sizin onayınızla.

## Sizin yapacaklarınız (bir kerelik)
1. KCB TECH klasörünü depoya bağlayın (`git remote`) ve GitHub girişini yapın.
2. Depo ayarları > Branches: `main` için "Require a pull request" ve "Require status checks" açın.
3. (Seçenek A) Settings > Secrets > `GEMINI_API_KEY` ekleyin.
4. Etiketleri açma ve iş akışı dosyasını yazma işini Claude yapar.
