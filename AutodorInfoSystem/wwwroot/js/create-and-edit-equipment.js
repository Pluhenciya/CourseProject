function fetchSimilarNames() {
    const nameInput = document.getElementById('equipmentName').value;
    const suggestionsList = document.getElementById('suggestions');
    const priceInput = document.getElementById('equipmentPrice');

    if (nameInput.length === 0) {
        suggestionsList.style.display = 'none';
        priceInput.value = ''; // ������� ����, ���� ���� ������
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
                    li.textContent = item.name; // ��������������, ��� item �������� ������ � ����� name
                    li.dataset.price = item.price; // ��������� ���� � �������� data-price
                    li.onclick = function () {
                        document.getElementById('equipmentName').value = item.name;
                        priceInput.value = item.price; // ��������� ����
                        suggestionsList.style.display = 'none';
                        priceInput.setAttribute('readonly', 'readonly'); // ������ ���� ���� ����������� ��� ��������������
                    };
                    suggestionsList.appendChild(li);
                });
            } else {
                suggestionsList.style.display = 'none';
                priceInput.removeAttribute('readonly'); // ������ ���� ���� ��������� ��� ��������������, ���� ��� ���������
            }
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

// �������� ���� ����, ���� ������������ ������ ����� ��������
document.getElementById('equipmentName').addEventListener('input', function () {
    const priceInput = document.getElementById('equipmentPrice');
    priceInput.removeAttribute('readonly'); // ������ ���� ���� ��������� ��� ��������������
    priceInput.value = ''; // ������� ����
});
