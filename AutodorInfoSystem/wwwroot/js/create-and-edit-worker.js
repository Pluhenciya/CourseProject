function fetchSimilarNames() {
    const nameInput = document.getElementById('workerName').value;
    const suggestionsList = document.getElementById('suggestions');
    const priceInput = document.getElementById('workerSalary');

    if (nameInput.length === 0) {
        suggestionsList.style.display = 'none';
        priceInput.value = ''; // Очищаем цену, если поле пустое
        return;
    }

    fetch(`/Workers/GetSimilarNames?name=${nameInput}`)
        .then(response => response.json())
        .then(data => {
            suggestionsList.innerHTML = '';
            if (data.length > 0) {
                suggestionsList.style.display = 'block';
                data.forEach(item => {
                    const li = document.createElement('li');
                    li.className = 'list-group-item list-group-item-action';
                    li.textContent = item.name; // Предполагается, что item содержит объект с полем name
                    li.dataset.price = item.salary; // Сохраняем цену в атрибуте data-salary
                    li.onclick = function () {
                        document.getElementById('workerName').value = item.name;
                        priceInput.value = item.salary; // Заполняем цену
                        suggestionsList.style.display = 'none';
                        priceInput.setAttribute('readonly', 'readonly'); // Делаем поле цены недоступным для редактирования
                    };
                    suggestionsList.appendChild(li);
                });
            } else {
                suggestionsList.style.display = 'none';
                priceInput.removeAttribute('readonly'); // Делаем поле цены доступным для редактирования, если нет подсказок
            }
        });
}

// Сбросить поле цены, если пользователь вводит новое название
document.getElementById('workerName').addEventListener('input', function () {
    const priceInput = document.getElementById('workerSalary');
    priceInput.removeAttribute('readonly'); // Делаем поле цены доступным для редактирования
    priceInput.value = ''; // Очищаем цену
});