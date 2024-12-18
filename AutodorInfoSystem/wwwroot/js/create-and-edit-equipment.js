function fetchSimilarNames() {
    const nameInput = document.getElementById('equipmentName').value;
    const suggestionsList = document.getElementById('suggestions');
    const priceInput = document.getElementById('equipmentPrice');

    if (nameInput.length === 0) {
        suggestionsList.style.display = 'none';
        priceInput.value = ''; // Очищаем цену, если поле пустое
        return;
    }

    fetch(`/Equipments/GetSimilarNames?name=${encodeURIComponent(nameInput)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            suggestionsList.innerHTML = '';
            if (data.length > 0) {
                suggestionsList.style.display = 'block';
                data.forEach(item => {
                    const li = document.createElement('li');
                    li.className = 'list-group-item list-group-item-action';
                    li.textContent = item.name; // Предполагается, что item содержит объект с полем name
                    li.dataset.price = item.price; // Сохраняем цену в атрибуте data-price
                    li.onclick = function () {
                        document.getElementById('equipmentName').value = item.name;
                        priceInput.value = item.price; // Заполняем цену
                        suggestionsList.style.display = 'none';
                        priceInput.setAttribute('readonly', 'readonly'); // Делаем поле цены недоступным для редактирования
                    };
                    suggestionsList.appendChild(li);
                });
            } else {
                suggestionsList.style.display = 'none';
                priceInput.removeAttribute('readonly'); // Делаем поле цены доступным для редактирования, если нет подсказок
            }
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

// Сбросить поле цены, если пользователь вводит новое название
document.getElementById('equipmentName').addEventListener('input', function () {
    const priceInput = document.getElementById('equipmentPrice');
    priceInput.removeAttribute('readonly'); // Делаем поле цены доступным для редактирования
    priceInput.value = ''; // Очищаем цену
});
