
const uri = 'https://localhost:44362/api/Employee';

let Emps = [];

function getItems() {
    debugger;

    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));

}

function addItem() {
    debugger;
    const addNameTextbox = document.getElementById('add-name');
    const addbirthDataTextbox = document.getElementById('add-birthData');
    const addTitleTextbox = document.getElementById('add-Title');
    const addHiringDataTextbox = document.getElementById('add-HiringData');
    const addDepartmentIdTextbox = document.getElementById('add-DepartmentId');
    

    var myHeaders = new Headers();
    myHeaders.append("accept", "*/*");

    var formdata = new FormData();
    formdata.append("Name", addNameTextbox.value.trim());
    formdata.append("BirthDate", addbirthDataTextbox.value.trim());
    formdata.append("Title", addTitleTextbox.value.trim());
    formdata.append("HiringDate", addHiringDataTextbox.value.trim());
    formdata.append("DepartmentId", addDepartmentIdTextbox.value);

    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: formdata,
        redirect: 'follow'
    };

    fetch("https://localhost:44362/api/Employee/AddEmployee", requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addbirthDataTextbox.value = '';
            addTitleTextbox.value = '';
            addHiringDataTextbox.value = '';
            addDepartmentIdTextbox.value = '';
        })
        .catch(error => console.log('error', error));
}

 
function deleteItem(identifier) {
    debugger;
    fetch(`${uri}/${identifier}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(uniqueIdentifier ) {
    const item = Emps.find(item => item.uniqueIdentifier === uniqueIdentifier );
  
    document.getElementById('edit-name').value = item.name; 
    document.getElementById('edit-Title').value = item.title;
    document.getElementById('edit-DepartmentId').value = item.departmentId;
    document.getElementById('edit-identifier').value = item.uniqueIdentifier;
    document.getElementById('editForm').style.display = 'block';
}

 

function updateItem() {
    debugger;

    var myHeaders = new Headers();
    myHeaders.append("accept", "*/*");
    
    var formdata = new FormData();
    const itemId = document.getElementById('edit-identifier').value;
    formdata.append("Name", document.getElementById('edit-name').value.trim());    
    formdata.append("BirthDate", document.getElementById('edit-birthData').value);
    formdata.append("Title", document.getElementById('edit-Title').value.trim());
    formdata.append("HiringDate", document.getElementById('edit-HiringData').value);
    formdata.append("DepartmentId", document.getElementById('edit-DepartmentId').value);
 
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
            getItems(); //To Update the table with New Data..

        })
        .catch(error => console.log('error', error));
    closeInput(); //Remove Edit Div
    return false;

}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Employee' : 'Employees';

    document.getElementById('counter').innerText = `Total : ${itemCount} ${name}`;
}

function _displayItems(data) {
    console.log(data);

    const tBody = document.getElementById('Emps');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');



    data.forEach(item => {


        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.uniqueIdentifier})`);
        editButton.setAttribute("class", "btn btn-warning");

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.uniqueIdentifier})`);
        deleteButton.setAttribute("class", "btn btn-danger");

 
        let tr = tBody.insertRow();


        let td1 = tr.insertCell(0);

        let textNode = document.createTextNode(item.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.birthDate);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.title);
        td3.appendChild(textNode3);


        let td4 = tr.insertCell(3);
        let textNode4 = document.createTextNode(item.hiringDate);
        td4.appendChild(textNode4);


        let td5 = tr.insertCell(4);
        let textNode5 = document.createTextNode(item.department.name);
        td5.appendChild(textNode5);

         


        let td6 = tr.insertCell(5);

        td6.appendChild(editButton);


        let td7 = tr.insertCell(6);
        td7.appendChild(deleteButton);
    });

    Emps = data;
}