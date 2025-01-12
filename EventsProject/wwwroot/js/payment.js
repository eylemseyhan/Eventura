// Kart formatları ve validasyonları için yardımcı fonksiyonlar
const formatters = {
    cardNumber: (value) => {
        const v = value.replace(/\s+/g, '').replace(/[^0-9]/gi, '');
        const matches = v.match(/\d{4,16}/g);
        const match = matches && matches[0] || '';
        const parts = [];

        for (let i = 0, len = match.length; i < len; i += 4) {
            parts.push(match.substring(i, i + 4));
        }

        if (parts.length) {
            return parts.join(' ');
        } else {
            return value;
        }
    },

    expiryDate: (value) => {
        const v = value.replace(/\s+/g, '').replace(/[^0-9]/gi, '');
        if (v.length >= 2) {
            return v.slice(0, 2) + '/' + v.slice(2, 4);
        }
        return v;
    }
};

// Kart önizleme güncelleme
function updateCardPreview() {
    const cardNumber = document.getElementById('cardNumber')?.value || '';
    const cardHolderName = document.getElementById('cardHolderName')?.value || '';
    const expiryDate = document.getElementById('expiryDate')?.value || '';

    if (document.getElementById('cardNumberPreview')) {
        document.getElementById('cardNumberPreview').textContent =
            cardNumber ? cardNumber.replace(/\d(?=\d{4})/g, "*") : "**** **** **** ****";
    }

    if (document.getElementById('cardHolderNamePreview')) {
        document.getElementById('cardHolderNamePreview').textContent =
            cardHolderName || "Kart Sahibinin Adı";
    }

    if (document.getElementById('expiryDatePreview')) {
        document.getElementById('expiryDatePreview').textContent =
            expiryDate || "MM/YY";
    }
}

// Kart numarası formatı
function formatCardNumber(event) {
    const input = event.target;
    input.value = formatters.cardNumber(input.value);
    updateCardPreview();
}

// Son kullanma tarihi formatı
function formatExpiryDate(event) {
    const input = event.target;
    let value = input.value.replace(/\D/g, '').slice(0, 4);

    if (value.length >= 2) {
        value = value.slice(0, 2) + '/' + value.slice(2);
    }

    input.value = value;
    updateCardPreview();
}

// Kayıtlı kartlar modalını yükle
function loadSavedCardModal() {
    fetch("/Payment/GetSavedCards")
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(savedCards => {
            let savedCardsHtml = '<h5>Kayıtlı Kartlarınız</h5>';

            if (!savedCards || savedCards.length === 0) {
                savedCardsHtml += '<p>Kayıtlı kart bulunmamaktadır.</p>';
            } else {
                savedCards.forEach(card => {
                    const formattedExpiryDate = formatExpiryDateString(card.expiryDate);
                    const maskedCardNumber = card.cardNumber.replace(/\d(?=\d{4})/g, "*");

                    savedCardsHtml += `
                        <div class="card mb-3">
                            <div class="card-body">
                                <div>Kart Numarası: ${maskedCardNumber}</div>
                                <div>Kart Sahibi: ${card.cardHolderName}</div>
                                <div>Son Kullanma Tarihi: ${formattedExpiryDate}</div>
                                <button class="btn btn-primary mt-2" 
                                    onclick="selectSavedCard('${card.savedCardId}', '${card.cardNumber}', '${card.cardHolderName}', '${card.expiryDate}')">
                                    Bu Kartla Ödeme Yap
                                </button>
                            </div>
                        </div>
                    `;
                });
            }

            const savedCardsList = document.getElementById('savedCardsList');
            if (savedCardsList) {
                savedCardsList.innerHTML = savedCardsHtml;
            }

            // Bootstrap 5 modal işlemleri
            const savedCardModal = new bootstrap.Modal(document.getElementById('savedCardModal'));
            savedCardModal.show();
        })
        .catch(error => {
            console.error('Kayıtlı kartlar yüklenirken hata:', error);
            alert('Kayıtlı kartlar yüklenirken bir hata oluştu. Lütfen tekrar deneyin.');
        });
}

// Tarih formatı için yardımcı fonksiyon
function formatExpiryDateString(dateString) {
    if (dateString.includes('-')) {
        const [year, month] = dateString.split('-');
        return `${month}/${year.slice(-2)}`;
    }
    return dateString;
}

// Kayıtlı kart seçimi
function selectSavedCard(savedCardId, cardNumber, cardHolderName, expiryDate) {
    const form = document.querySelector('form');
    if (!form) return;

    // Hidden input güncelleme/ekleme
    let hiddenInput = document.getElementById('selectedCardId');
    if (!hiddenInput) {
        hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.id = 'selectedCardId';
        hiddenInput.name = 'selectedCardId';
        form.appendChild(hiddenInput);
    }
    hiddenInput.value = savedCardId;

    // Form alanlarını doldur
    const formattedExpiryDate = formatExpiryDateString(expiryDate);
    const formattedCardNumber = formatters.cardNumber(cardNumber);

    const fields = {
        'cardNumber': formattedCardNumber,
        'cardHolderName': cardHolderName,
        'expiryDate': formattedExpiryDate
    };

    Object.entries(fields).forEach(([id, value]) => {
        const element = document.getElementById(id);
        if (element) {
            element.value = value;
            element.disabled = true;
        }
    });

    // Kartı kaydet checkbox'ını gizle
    const saveCardDiv = document.querySelector('.form-check');
    if (saveCardDiv) {
        saveCardDiv.style.display = 'none';
    }

    // Önizlemeyi güncelle
    updateCardPreview();

    // Modal'ı kapat
    const savedCardModal = bootstrap.Modal.getInstance(document.getElementById('savedCardModal'));
    if (savedCardModal) {
        savedCardModal.hide();
    }
}

// Sayfa yüklendiğinde input event listener'ları ekle
document.addEventListener('DOMContentLoaded', function () {
    const inputs = {
        'cardNumber': formatCardNumber,
        'expiryDate': formatExpiryDate,
        'cardHolderName': () => updateCardPreview()
    };

    Object.entries(inputs).forEach(([id, handler]) => {
        const element = document.getElementById(id);
        if (element) {
            element.addEventListener('input', handler);
        }
    });
}); 