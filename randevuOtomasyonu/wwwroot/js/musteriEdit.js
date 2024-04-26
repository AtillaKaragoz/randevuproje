
const duzenleButonlari = document.querySelectorAll(".btn-duzenle");
const silButonlari = document.querySelectorAll(".btn-sil");

duzenleButonlari.forEach(buton => {
  buton.addEventListener("click", duzenleClickiIsle);
});

silButonlari.forEach(buton => {
  buton.addEventListener("click", silClickiIsle);
});

function duzenleClickiIsle(event) {
  // Düzenleme işlemini ele alan kod (isteğe bağlı)
  // Örneğin, düzenleme formunu açabilir veya satırın içeriğini düzenlenebilir hale getirebilirsiniz.
  alert("Düzenle seçildi!"); // Geçici olarak bir uyarı görüntüleyin
}

function silClickiIsle(event) {
  // Silme işlemini ele alan kod (isteğe bağlı)
  // Örneğin, satırı silmek için bir onay kutusu gösterebilir veya sunucuya silme isteği gönderebilirsiniz.
  alert("Silme seçildi!"); // Geçici olarak bir uyarı görüntüleyin
}

function duzenleClickiIsle(event) {
  const satir = event.target.closest("tr"); // Düğmeyi içeren tablo satırını al
  const tabloVerileri = satir.querySelectorAll("td"); // Satır içindeki tüm hücreleri (td) al

  // Her bir hücrenin içeriğini düzenlenebilir hale getir
  tabloVerileri.forEach(hucre => {
    hucre.contentEditable = true;
  });

  // Değişiklikleri kaydetmek için bir düğme ekle (isteğe bağlı)
  const kaydetButonu = document.createElement("button");
  kaydetButonu.textContent = "Kaydet";
  kaydetButonu.addEventListener("click", kaydetClickiIsle);
  satir.appendChild(kaydetButonu);
}

function kaydetClickiIsle(event) {
  // Düzenlenen verileri sunucuya gönder veya başka bir işlem yap
  const satir = event.target.closest("tr");
  const tabloVerileri = satir.querySelectorAll("td");

  // Hücreleri tekrar düzenlenemez hale getir
  tabloVerileri.forEach(hucre => {
    hucre.contentEditable = false;
  });

  // Kaydet düğmesini kaldır
  satir.removeChild(event.target);
}

function silClickiIsle(event) {
  const onay = confirm("Satırı silmek istediğinize emin misiniz?");
  if (onay) {
    const satir = event.target.closest("tr");
    satir.parentNode.removeChild(satir); // Satırı DOM'dan sil
  }
}
