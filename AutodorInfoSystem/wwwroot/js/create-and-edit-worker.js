function fetchSimilarNames() {
    const nameInput = document.getElementById('workerName').value;
    const suggestionsList = document.getElementById('suggestions');
    const priceInput = document.getElementById('workerSalary');

    if (nameInput.length === 0) {
        suggestionsList.style.display = 'none';
        priceInput.value = ''; // ������� ����, ���� ���� ������
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
                    li.textContent = item.name; // ��������������, ��� item �������� ������ � ����� name
                    li.dataset.price = item.salary; // ��������� ���� � �������� data-salary
                    li.onclick = function () {
                        document.getElementById('workerName').value = item.name;
                        priceInput.value = item.salary; // ��������� ����
                        suggestionsList.style.display = 'none';
                        priceInput.setAttribute('readonly', 'readonly'); // ������ ���� ���� ����������� ��� ��������������
                    };
                    suggestionsList.appendChild(li);
                });
            } else {
                suggestionsList.style.display = 'none';
                priceInput.removeAttribute('readonly'); // ������ ���� ���� ��������� ��� ��������������, ���� ��� ���������
            }
        });
}

// �������� ���� ����, ���� ������������ ������ ����� ��������
document.getElementById('workerName').addEventListener('input', function () {
    const priceInput = document.getElementById('workerSalary');
    priceInput.removeAttribute('readonly'); // ������ ���� ���� ��������� ��� ��������������
    priceInput.value = ''; // ������� ����
});