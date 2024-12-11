function toggleProjecterFields(role) {
    var projecterFields = document.getElementById("projecterFields");
    if (role === "Projecter") {
        projecterFields.style.display = "block";
    } else {
        projecterFields.style.display = "none";
    }
}