﻿
// Funcion alerta con formulario
function mostrarFormulario() {
    cargarSecciones();

    Swal.fire({
        title: 'Ingrese los detalles de su platillo',
        html:
            '<input id="nombre" class="swal2-input" placeholder="Nombre">' +
            '<textarea id="descripcion" class="swal2-textarea" placeholder="Descripción"></textarea>' +
            '<input id="precio" class="swal2-input" type="text" placeholder="Precio">' +
            '<select id="opciones" class= "swal2-select" > ' +
            '<option value="" disabled selected>Selecciona el tipo de platillo</option>' +
            '</select>' +
            '<input type="file" id="imagen" class="swal2-file" accept=".png, .jpg">' +
            '<img id="imagenActual" src="" style="max-width: 200px; max-height: 200px;" />',
        showCancelButton: true,
        confirmButtonText: 'Añadir',
        cancelButtonText: 'Cancelar',
        preConfirm: () => {
            const nombre = Swal.getPopup().querySelector('#nombre').value;
            const descripcion = Swal.getPopup().querySelector('#descripcion').value;
            let precio = Swal.getPopup().querySelector('#precio').value;
            const opcionSeleccionada = Swal.getPopup().querySelector('#opciones').value;
            const imagen = Swal.getPopup().querySelector('#imagen').files[0];

            // Validar que todos los campos estén llenos
            if (nombre === '' || descripcion === '' || precio === '' || opcionSeleccionada === '' || imagen === undefined) {
                Swal.showValidationMessage('Todos los campos son requeridos.');
                return false;
            }

            // Validar que el precio sea numérico y formatearlo
            if (!isNaN(parseFloat(precio)) && isFinite(precio)) {
                // Formatear el precio como dinero
                precio = parseFloat(precio).toFixed(2);
                precio = '$' + precio.toString();
            } else {
                // Mostrar mensaje de error si el precio no es numérico
                Swal.showValidationMessage('El precio debe ser un valor numérico.');
                return false;
            }

            const formData = new FormData();
            formData.append('nombre', document.getElementById('nombre').value);
            formData.append('descripcion', document.getElementById('descripcion').value);
            formData.append('precio', document.getElementById('precio').value);
            formData.append('opciones', document.getElementById('opciones').value);
            formData.append('imagen', document.getElementById('imagen').files[0]);

            // Enviar datos al controlador
            $.ajax({
                url: url_AgregarPlatillo,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    // Aquí puedes manejar la respuesta del controlador

                    Swal.fire({
                        icon: 'success',
                        title: 'Platillo agregado correctamente',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    // Recargar la tabla después de agregar un platillo
                    tablaPla.ajax.reload();
                },
                error: function (xhr, status, error) {
                    // En caso de error, mostrar mensaje
                    console.error(xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error al agregar platillo',
                        text: 'Ocurrió un error al intentar agregar el platillo.'
                    });
                }
            });

            // Evitar que el formulario se cierre automáticamente
            return false;
        }
    });
    // Evento change para el input de tipo file
    document.getElementById('imagen').addEventListener('change', function () {
        const reader = new FileReader();
        reader.onload = function (e) {
            const imgElement = document.getElementById('imagenActual');
            imgElement.src = e.target.result;
        };
        reader.readAsDataURL(this.files[0]);
    });
}

// Función para cargar las secciones en el select
function cargarSecciones() {
    // Realizar una solicitud AJAX para obtener las secciones disponibles del servidor
    $.ajax({
        url: url_ObtenerSecciones, // Ruta al controlador que devuelve las secciones
        type: 'GET',
        success: function (response) {
            // Limpiar las opciones actuales del select
            $('#opciones').empty();

            // Agregar una opción predeterminada al select
            $('#opciones').append($('<option>', {
                value: '',
                text: 'Selecciona el tipo de platillo',
                disabled: true,
                selected: true
            }));

            // Agregar cada sección como una opción en el select
            response.forEach(function (seccion) {
                $('#opciones').append($('<option>', {
                    value: seccion.Id,
                    text: seccion.Nombre
                }));
            });
        },
        error: function (xhr, status, error) {
            // En caso de error, mostrar mensaje
            console.error(xhr.responseText);
            Swal.fire({
                icon: 'error',
                title: 'Error al cargar las secciones',
                text: 'Ocurrió un error al intentar cargar las secciones disponibles.'
            });
        }
    });
}

function mostrarFormularioEdit(Id_platillo) {
    // Hacer una llamada AJAX para obtener los detalles del platillo
    $.ajax({
        url: url_detallarPlatillo,
        type: 'GET',
        data: { Id_platillo: Id_platillo },
        success: function (response) {
            if (response) {
                // Llenar el modal con los detalles del platillo
                cargarSeccionesPla(Id_platillo); // Llamar a la función cargarSecciones para rellenar el select de opciones


                Swal.fire({
                    title: 'Modificacion de platillo',
                    html:

                        '<input id="nombre" class="swal2-input" placeholder="Nombre" value="' + response.Nombre + '">' +
                        '<textarea id="descripcion" class="swal2-textarea" placeholder="Descripción">' + response.Descripcion + '</textarea>' +
                        '<input id="precio" class="swal2-input" type="text" placeholder="Precio" value="' + response.precio + '">' +
                        '<br />' +
                        '<select id="opciones" class="swal2-select">' +
                        '<option value="" disabled>Selecciona el tipo de platillo</option>' +
                        '</select>' +
                        '<img id="imagenActual" src="' + response.Imagen + '" style="max-width: 200px; max-height: 200px;" />' +
                        '<input type="file" id="imagen" class="swal2-file" accept=".png, .jpg">',
                    showCancelButton: true,
                    confirmButtonText: 'Guardar',
                    cancelButtonText: 'Cancelar',
                    preConfirm: () => {
                        const nombre = Swal.getPopup().querySelector('#nombre').value;
                        const descripcion = Swal.getPopup().querySelector('#descripcion').value;
                        let precio = Swal.getPopup().querySelector('#precio').value;
                        const opcionSeleccionada = Swal.getPopup().querySelector('#opciones').value;
                        const imagen = Swal.getPopup().querySelector('#imagen').files[0];

                        // Validar que todos los campos estén llenos
                        if (nombre === '' || descripcion === '' || precio === '' || opcionSeleccionada === '') {
                            Swal.showValidationMessage('Todos los campos son requeridos.');
                            return false;
                        }

                        // Validar que el precio sea numérico y formatearlo
                        if (!isNaN(parseFloat(precio)) && isFinite(precio)) {
                            // Formatear el precio como dinero
                            precio = parseFloat(precio).toFixed(2);
                            precio = '$' + precio.toString();
                        } else {
                            // Mostrar mensaje de error si el precio no es numérico
                            Swal.showValidationMessage('El precio debe ser un valor numérico.');
                            return false;
                        }

                        const formData = new FormData();
                        formData.append('Id_platillo', Id_platillo);
                        formData.append('nombre', document.getElementById('nombre').value);
                        formData.append('descripcion', document.getElementById('descripcion').value);
                        formData.append('precio', document.getElementById('precio').value);
                        formData.append('opciones', document.getElementById('opciones').value);
                        formData.append('imagen', document.getElementById('imagen').files[0]);

                        // Enviar datos al controlador
                        $.ajax({
                            url: url_actualizarPlatillo,
                            type: 'POST',
                            data: formData,
                            processData: false,
                            contentType: false,
                            success: function (response) {
                                // Aquí puedes manejar la respuesta del controlador
                                // Por ejemplo, mostrar un mensaje de éxito o redirigir a otra página
                                console.log(response);
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Platillo modificado correctamente',
                                    showConfirmButton: false,
                                    timer: 1500
                                });
                                // Recargar la tabla después de actualizar un platillo
                                tablaPla.ajax.reload();
                            },
                            error: function (xhr, status, error) {
                                // En caso de error, mostrar mensaje
                                console.error(xhr.responseText);
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error al modificar platillo',
                                    text: 'Ocurrió un error al intentar modificar el platillo.'
                                });
                            }
                        });

                        // Evitar que el formulario se cierre automáticamente
                        return false;
                    }
                });
                // Evento change para el input de tipo file
                document.getElementById('imagen').addEventListener('change', function () {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        const imgElement = document.getElementById('imagenActual');
                        imgElement.src = e.target.result;
                    };
                    reader.readAsDataURL(this.files[0]);
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

function cargarSeccionesPla(Id_platillo) {
    // Realizar una solicitud AJAX para obtener las secciones disponibles del servidor
    $.ajax({
        url: url_obtenerSecciones2,
        type: 'GET',
        data: { Id_platillo: Id_platillo },
        success: function (response) {
            // Limpiar las opciones actuales del select
            $('#opciones').empty();

            // Agregar una opción predeterminada al select
            $('#opciones').append($('<option>', {
                value: '',
                text: 'Puedes seleccionar otra sección',
                disabled: true,
                selected: true
            }));

            // Iterar sobre las secciones devueltas en la respuesta
            $.each(response.Secciones, function (index, seccion) {
                $('#opciones').append($('<option>', {
                    value: seccion.Id,
                    text: seccion.Nombre
                }));
            });

            // Acceder al platillo
            var platillo = response.Platillo;

            // Verificar si el ID del platillo coincide con el ID pasado como parámetro
            if (Id_platillo == platillo.Id_platillo) {
                // Establecer el valor del select con el tipo del platillo
                $('#opciones').val(platillo.Tipo);
            }
        },
        error: function (xhr, status, error) {
            // En caso de error, mostrar mensaje
            console.error(xhr.responseText);
            Swal.fire({
                icon: 'error',
                title: 'Error al cargar las secciones',
                text: 'Ocurrió un error al intentar cargar las secciones disponibles.'
            });
        }
    });
}

function eliminar(Id_platillo) {
    // Mostrar un mensaje de confirmación antes de eliminar el platillo
    Swal.fire({
        title: '¿Estás seguro?',
        text: 'Una vez eliminado, no podrás recuperar este platillo.',
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
                Id_platillo: Id_platillo
            };
            $.ajax({
                url: url_eliminarPlatillo,
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (response) {
                    // Manejar la respuesta del controlador
                    console.log(response);
                    Swal.fire({
                        icon: 'success',
                        title: 'Platillo eliminado correctamente',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    // Recargar la tabla después de eliminar el platillo
                    tablaPla.ajax.reload();
                },
                error: function (xhr, status, error) {
                    // Mostrar mensaje de error en caso de fallo
                    console.error(xhr.responseText);
                    Swal.fire({
                        icon: 'error',
                        title: 'Error al eliminar platillo',
                        text: 'Ocurrió un error al intentar eliminar el platillo.'
                    });
                }
            });
        }
    });
}