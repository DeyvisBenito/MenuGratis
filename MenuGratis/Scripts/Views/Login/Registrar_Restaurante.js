document.getElementById("formulario2").addEventListener("submit", function (event) {
    var pais = document.getElementById("fname").value;
    var departamento = document.getElementById("dept").value;
    var municipio = document.getElementById("city").value;
    var zona = document.getElementById("zone").value;
    var calle = document.getElementById("calle").value;

    // Verificar que todos los campos estén completos y sean válidos
    if (pais.trim() === "" || departamento.trim() === "" || municipio.trim() === "" || zona.trim() === "" || calle.trim() === "") {
        alert("Por favor completa todos los campos antes de continuar.");
        event.preventDefault(); // Detener el envío del formulario
        return false;
    }

    // Si todos los campos son válidos, permitir el envío del formulario
    return true;
});