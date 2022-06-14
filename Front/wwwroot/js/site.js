const uri = 'https://localhost:44362/api/Department';
let Deps = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    debugger;
    const addNameTextbox = document.getElementById('add-name');
    console.log(addNameTextbox);
    console.log(addNameTextbox.value.trim());

    var myHeaders = new Headers();
    myHeaders.append("accept", "*/*");

    var formdata = new FormData();
    formdata.append("Name", addNameTextbox.value.trim());

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: formdata,
        redirect: 'follow'
    };

    fetch("https://localhost:44362/api/Department", requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.log('error', error))


}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = Deps.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    debugger;

    var myHeaders = new Headers();
    myHeaders.append("accept", "*/*");

    var formdata = new FormData();
    const itemId = document.getElementById('edit-id').value;
    formdata.append("Name", document.getElementById('edit-name').value.trim());

    var requestOptions = {
        method: 'PUT',
        headers: myHeaders,
        body: formdata,
        redirect: 'follow'
    };

    fetch(`${uri}/${itemId}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .then(() => {
            getItems();

        })
        .catch(error => console.log('error', error));
    closeInput(); //Remove Edit Div
    return false;

}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Department' : 'Departments';

    document.getElementById('counter').innerText = `Total : ${itemCount} ${name}`;
}

function _displayItems(data) {
    console.log(data);



    const tBody = document.getElementById('Deps');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');



    data.forEach(item => {


        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);
        editButton.setAttribute("class", "btn btn-warning");

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);
        deleteButton.setAttribute("class", "btn btn-danger");


        let tr = tBody.insertRow();


        let td1 = tr.insertCell(0);

        let textNode = document.createTextNode(item.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.numberOfEmployees);
        td2.appendChild(textNode2);


        let td3 = tr.insertCell(2);

        td3.appendChild(editButton);


        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    Deps = data;
}