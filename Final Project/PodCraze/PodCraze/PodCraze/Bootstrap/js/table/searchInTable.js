const searchInput = document.getElementById("searchInput");
const dataTable = document.getElementById("dataTable").getElementsByTagName("tbody")[0].getElementsByTagName("tr");

searchInput.addEventListener("keyup", function () {
    const searchTerm = searchInput.value.toLowerCase();

    for (let i = 0; i < dataTable.length; i++) {
        let rowVisible = false;
        const columns = dataTable[i].getElementsByTagName("td");

        for (let j = 0; j < columns.length; j++) {
            const columnText = columns[j].textContent.toLowerCase();
            if (columnText.includes(searchTerm)) {
                rowVisible = true;
                break;
            }
        }

        dataTable[i].style.display = rowVisible ? "" : "none";
    }
});