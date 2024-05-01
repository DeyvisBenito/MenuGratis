function mostrar_descr(Id_platillo) {
    // Hacer una llamada AJAX para obtener los detalles del platillo
    $.ajax({
        url: url_detallePlatillo,
        type: 'GET',
        data: { Id_platillo: Id_platillo },
        success: function (response) {
            if (response) {
                // Llenar el modal con los detalles del platillo
                Swal.fire({
                    title: 'Descripcion del platillo',
                    html:
                        '<div> <label id="nombre" class="swal2-input">' + response.Nombre + '</label> </div>' +
                        '<div> <textarea id="descripcion" class="swal2-textarea" readonly>' + response.Descripcion + '</textarea> </div>' +
                        '<div> <label id="precio" class="swal2-input"> Q. ' + response.precio + '</label> </div>' +
                        '<div> <img id="imagenActual" src="' + response.Imagen + '" style="max-width: 200px; max-height: 200px;" /> </div>',
                    confirmButtonText: 'Salir',

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