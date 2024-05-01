
function guardarSeccion(nombreSeccion) {
    // Verificar si el nombre de la sección está vacío
    if (nombreSeccion.trim() === '') {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'El nombre de la sección es obligatorio.'
        });
        return; // Salir de la función si el nombre de la sección está vacío
    }

    // Abrir SweetAlert2
    Swal.fire({
        title: '¿Estás seguro?',
        text: '¿Deseas guardar la sección?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Guardar',
        cancelButtonText: 'Cancelar',
        showLoaderOnConfirm: true,
        preConfirm: function () {
            return new Promise(function (resolve, reject) {
                // Enviar los datos al controlador a través de Ajax
                $.ajax({
                    type: 'POST', // Método de envío de datos
                    url: url_AgregarSeccion, // Ruta al controlador
                    data: {
                        nombreSeccion: nombreSeccion // Datos que se envían al controlador
                    },
                    success: function (response) {
                        if (response.success) {
                            resolve(response); // Resolver la promesa con la respuesta del servidor
                        } else {
                            reject('La sección ya existe.'); // Rechazar la promesa si la sección ya existe
                        }
                    },
                    error: function (xhr, status, error) {
                        reject('Error al guardar la sección.'); // Rechazar la promesa en caso de error
                    }
                });
            });
        }
    }).then(function (result) {
        // Manejar el resultado de la promesa
        if (result.value) {
            Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: 'La sección se guardó correctamente.',
                showConfirmButton: false,
                timer: 1500,
            }).then((result) => {
                // Función para redireccionar después de que termine el temporizador
                setTimeout(() => {
                    $('#ModalS').modal('hide');
                    window.location.href = url_listarPla;
                });
            });
        }
    }).catch(function (error) {
        // Manejar el error si las validaciones fallan
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: error
        });
    });
}

function eliminarSeccion(Id_se) {
    // Mostrar un mensaje de confirmación antes de eliminar el platillo
    Swal.fire({
        title: '¿Estás seguro?',
        text: 'Una vez eliminado, se eliminaran todos los platillos de esta sección',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Si el usuario confirma la eliminación, enviar la solicitud al controlador
            var data = {
                Id_se: Id_se
            };
            $.ajax({
                url: url_eliminarSec,
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (response) {
                    // Manejar la respuesta del controlador
                    console.log(response);
                    Swal.fire({
                        icon: 'success',
                        title: 'Sección eliminada correctamente',
                        showConfirmButton: false,
                        timer: 1500,
                    });
                    // Redirigir a la vista después de un breve retraso
                    setTimeout(function () {
                        window.location.href = url_listarPla;
                    }, 1500);
                },
                error: function (xhr, status, error) {
                    // Mostrar mensaje de error en caso de fallo
                    console.error(xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error al eliminar la sección',
                        text: 'Ocurrió un error al intentar eliminar la sección.'
                    });
                }
            });
        }
    });
}


function editarSeccion(Id_se) {
    // Hacer una llamada AJAX para obtener los detalles del platillo
    $.ajax({
        url: url_detallarseccion,
        type: 'GET',
        data: { Id_se: Id_se },
        success: function (response) {
            if (response) {
                Swal.fire({
                    title: 'Modificacion de Sección',
                    html:
                        '<label for="nombre"> Nombre: </label>' +
                        '<input id="nombre" class="swal2-input" placeholder="Nombre" value="' + response.Nombre + '">',
                    showCancelButton: true,
                    confirmButtonText: 'Guardar',
                    cancelButtonText: 'Cancelar',
                    preConfirm: () => {
                        const nombre = Swal.getPopup().querySelector('#nombre').value;
                        const nombreOrig = response.Nombre;

                        // Validar que el campo esté lleno
                        if (nombre === '') {
                            Swal.showValidationMessage('El nombre de la sección es requerido.');
                            return false;
                        }

                        // Validar si ya existe una sección con el mismo nombre
                        const formDataCheck = new FormData();
                        formDataCheck.append('nombre', nombre);

                        $.ajax({
                            url: url_validarSec,
                            type: 'POST',
                            data: formDataCheck,
                            processData: false,
                            contentType: false,
                            success: function (result) {
                                if (!result.success) {
                                    // Mostrar mensaje de error si ya existe una sección con el mismo nombre
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Error al editar sección',
                                        text: 'Ya existe una sección con el mismo nombre.'
                                    });
                                    return false; // Evitar que el formulario se cierre automáticamente
                                }

                                // Si no hay secciones con el mismo nombre, proceder con la edición
                                const formData = new FormData();
                                formData.append('Id_se', Id_se);
                                formData.append('nombre', nombre);
                                formData.append('nombreOrig', nombreOrig);

                                // Enviar datos al controlador
                                $.ajax({
                                    url: url_editSeccion,
                                    type: 'POST',
                                    data: formData,
                                    processData: false,
                                    contentType: false,
                                    success: function (response) {
                                        // Manejar la respuesta del controlador
                                        console.log(response);
                                        Swal.fire({
                                            icon: 'success',
                                            title: 'Sección modificada',
                                            showConfirmButton: false,
                                            timer: 1500
                                        });
                                        // Redirigir a la vista después de un breve retraso
                                        setTimeout(function () {
                                            window.location.href = url_listarPla;
                                        }, 1500);
                                    },
                                    error: function (xhr, status, error) {
                                        // Mostrar mensaje de error en caso de fallo
                                        console.error(xhr.responseText);
                                        Swal.fire({
                                            icon: 'error',
                                            title: 'Error al editar sección',
                                            text: 'Ocurrió un error al intentar editar la sección.'
                                        });
                                    }
                                });
                            },
                            error: function (xhr, status, error) {
                                // Mostrar mensaje de error en caso de fallo
                                console.error(xhr.responseText);
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error al validar nombre de sección',
                                    text: 'Ocurrió un error al validar el nombre de la sección.'
                                });
                            }
                        });

                        // Evitar que el formulario se cierre automáticamente
                        return false;
                    }
                });
            } else {
                // Mostrar mensaje de error si no se encontraron detalles del platillo
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'No se encontraron detalles del platillo.'
                });
            }
        },
        error: function (xhr, status, error) {
            // Mostrar mensaje de error en caso de fallo de la llamada AJAX
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al obtener los detalles del platillo.'
            });
        }
    });
}
