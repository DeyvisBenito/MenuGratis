// Función para permitir solo números en el campo de teléfono
function onlyNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        event.preventDefault();
    }
}

document.getElementById("formulario1").addEventListener("submit", function (event) {
    var nombre = document.getElementById("fname").value;
    var apellido = document.getElementById("lname").value;
    var telefono = document.getElementById("phone").value;
    var sexo = document.getElementById("sex").value;
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;
    var confirmar = document.getElementById("confirm").value;
    var restaurante = document.getElementById("restaurant").value;

    // Verificar que todos los campos estén completos y sean válidos
    if (nombre.trim() === "" || apellido.trim() === "" || telefono.trim() === "" || sexo.trim() === "" || email.trim() === "" || password.trim() === "" || confirmar.trim() === "" || restaurante.trim() === "") {
        alert("Por favor completa todos los campos antes de continuar.");
        event.preventDefault(); // Detener el envío del formulario
        return false;
    }

    // Verificar si las contraseñas coinciden
    if (password !== confirmar) {
        alert("Las contraseñas no coinciden. Por favor verifícalas.");
        event.preventDefault(); // Detener el envío del formulario
        return false;
    }

    // Verificar si el campo de email tiene un formato válido
    if (!validateEmail(email)) {
        alert("Por favor ingresa un email válido.");
        event.preventDefault(); // Detener el envío del formulario
        return false;
    }

    // Si todos los campos son válidos, permitir el envío del formulario
    return true;
});

function validateEmail(email) {
    //Modificar despues
    return true;
}