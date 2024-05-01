
document.addEventListener('DOMContentLoaded', function () {
    const formulario = document.getElementById("formulario1");
    formulario.addEventListener('submit', function (event) {
        // Detener el envío del formulario
        event.preventDefault();

        // Mostrar la alerta de confirmación
        showAlertConfirmar();
    });
});


// Alerta confirmar
function showAlertConfirmar() {
    Swal.fire({
        title: '¿Estás seguro de que quieres actualizar?',
        text: 'Esta acción no se puede deshacer.',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, actualizar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            var nombre = document.getElementById("name").value;
            var apellido = document.getElementById("lname").value;
            var sexo = document.getElementById("sex").value;
            var phone = document.getElementById("phone").value;
            var restaurante = document.getElementById("restaurant").value;

            // Verificar que todos los campos estén completos y sean válidos
            if (nombre.trim() === "" || apellido.trim() === "" || sexo.trim() === "" || phone.trim() === "" || restaurante.trim() === "") {
                Swal.fire({
                    title: 'Campos incompletos',
                    text: 'Por favor completa todos los campos antes de continuar.',
                    icon: 'warning',
                    confirmButtonText: 'OK'
                });
                return false;
            } else {
                // Realizar una solicitud AJAX para enviar los datos del formulario
                $.ajax({
                    url: url_guardarUp,
                    type: 'POST',
                    data: {
                        name: nombre,
                        lname: apellido,
                        sex: sexo,
                        phone: phone,
                        restaurant: restaurante
                    },
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                title: '¡Actualización exitosa!',
                                text: 'Los datos se han actualizado correctamente.',
                                icon: 'success',
                                timer: 800,
                                showConfirmButton: false
                            });

                            // Redirigir a la vista después de un breve retraso
                            setTimeout(function () {
                                window.location.href = url_listarP;
                            }, 800);
                        } else {
                            Swal.fire({
                                title: 'Error en la actualización',
                                text: 'Ha ocurrido un error al intentar actualizar los datos.',
                                icon: 'error'
                            });
                        }
                    },
                    error: function (xhr, status, error) {
                        // Manejar errores de la solicitud AJAX
                        console.error(xhr.responseText);
                        Swal.fire({
                            title: 'Error en la solicitud',
                            text: 'Ha ocurrido un error al intentar enviar los datos del formulario.',
                            icon: 'error'
                        });
                    }
                });
            }
        }
    });
}

// Alerta cancelar
function showAlertCancelar(element) {
    event.preventDefault();
    Swal.fire({
        title: '¿Está seguro de que desea cancelar?',
        icon: 'question',
        showCancelButton: 'true',
        confirmButtonText: 'Si',
        cancelButtonText: 'No',
    }).then((result) => {
        if (result.isConfirmed) {
            // Obtener la URL de redirección del enlace "Cancelar"
            const redirectUrl = element.getAttribute("href");

            // Redirigir a la URL obtenida del enlace "Cancelar"
            if (redirectUrl) {
                window.location.href = redirectUrl;
            }
        }
    });
}
//Alerta de error (añadir logica despues)
function showAlertError() {
    Swal.fire({
        title: 'Ha ocurrido un error en la actualizacion de sus datos :(',
        icon: 'warning',
    })
}


// Función para permitir solo números en el campo de teléfono
function onlyNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        event.preventDefault();
    }
}