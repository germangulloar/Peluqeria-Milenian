$(function () {
    var cliente = {};
    var clienteIdEliminar = 0;
    var model = {

        //#region Listado


        RecogerDatosFormulario: function () {

            var fechaNacimiento = util.GetDate($("#txtFechaNacimiento").val());

            cliente = {
                Id: $("#txtIdentificador").val(),
                Nombres: $("#txtNombres").val(),
                Apellidos: $("#txtApellidos").val(),
                Dni: $("#txtDni").val(),
                Direccion: $("#txtDireccion").val(),
                Telefono: $("#txtTelefono").val(),
                Sexo: $('input[name=rbSexo]:checked').val(),
                FechaNacimiento: (fechaNacimiento == null) ? '' : fechaNacimiento,
                Activo: true
            };
        },

        CargarLista: function () {
            util.Ajax("ListarClientes", JSON.stringify({}),
            function (data) {
                var lista = [];
                if (data != null) {
                    lista = data;
                }
                else {
                    util.MsgAlert("No se encontraron registros");
                }
                $("#grilla").dxDataGrid("instance").option({ 'dataSource': lista });
            });
        },

        ConfigurarGrilla: function () {
            util.Grid("grilla",
            [
                {
                    dataField: 'Apellidos'
                    , caption: 'Apellidos'
                    , width: '25%'
                    , cellTemplate: function (container, options) {
                        var apellidos = options.value;
                        var label = $('<div />')
                            .css('word-wrap', 'break-word')
                            .text(apellidos)
                            .attr('title', apellidos)
                            .appendTo(container)
                            .parents("td")
                            .css('white-space', 'initial');
                    }
                },
                {
                    dataField: 'Nombres'
                    , caption: 'Nombres'
                    , width: '25%'
                    , cellTemplate: function (container, options) {
                        var nombres = options.value;
                        var label = $('<div />')
                            .css('word-wrap', 'break-word')
                            .text(nombres)
                            .attr('title', nombres)
                            .appendTo(container)
                            .parents("td")
                            .css('white-space', 'initial');
                    }
                },
                {
                    dataField: 'Dni',
                    caption: 'DNI',
                    width: '15%',
                    alignment: 'center',
                },
                {
                    dataField: 'Telefono',
                    caption: 'Telefono',
                    width: '15%',
                    alignment: 'center',
                    allowEditing: false,
                    allowGrouping: false,
                    allowHiding: false,
                    allowReordering: false,
                    allowSorting: false,
                },
                {
                    dataField: 'Activo'
                    , caption: 'Estado'
                    , width: '10%'
                },
                {
                    dataField: 'Id',
                    caption: 'Acciones',
                    width: '10%',
                    dataType: 'string',
                    allowEditing: false,
                    allowFiltering: false,
                    allowGrouping: false,
                    allowHiding: false,
                    allowReordering: false,
                    allowSorting: false,
                    alignment: 'center',
                    cellTemplate: function (container, options) {

                        var id = options.value;

                        var link = $('<a />')
                            .attr('title', 'Editar Profesional')
                            .on('click', function () { model.Obtener(id) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-pencil')
                            .appendTo(link);

                        var link_eliminar = $('<a />')
                            .attr('title', 'Eliminar Profesional')
                            .on('click', function () { model.EliminarCliente(id) })
                            .css('cursor', 'pointer')
                            .css('margin-right', '5px')
                            .appendTo(container);

                        $("<i />")
                            .attr('class', 'fa fa-fw fa-trash')
                            .appendTo(link_eliminar);
                    }
                }
            ],
            $(window).height() - 280, true, true, false);

            $("#grilla").dxDataGrid("instance").option({ selection: { mode: 'none' } });
            $("#grilla").dxDataGrid("instance").option({
                pager: {
                    showInfo: true,
                    infoText: 'Página {0} de {1} ({2} registros)',
                    showNavigationButtons: true,
                    showPageSizeSelector: false,
                    visible: true
                },
                paging: {
                    pageSize: 30,
                    enabled: true
                }
            });

            var grilla = $("#grilla").dxDataGrid("instance");

            if (grilla != null && $(window).height() >= 490) {
                grilla.option({ height: $(window).height() - 280 });
                grilla.resize();
            }

        },

        EliminarCliente: function (id) {
            $("#formularioEliminarCliente").dialog("open");
            clienteIdEliminar = id;
        },

        Eliminar: function () {
            debugger;
            console.log("!");
            cliente = {
                Id: clienteIdEliminar,
                Nombres: '',
                Apellidos: '',
                Dni: '',
                Direccion: '',
                Telefono: '',
                Sexo: '',
                FechaNacimiento: '',
                Activo: false
            };

            util.Ajax("EliminarCliente", JSON.stringify({ item: cliente }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    model.CargarLista();
                    util.MsgInfo(resultado.Mensaje);
                }
                else {
                    util.MsgAlert(resultado.Mensaje);
                }
                profesionalIdEliminar = 0;
                $("#formularioEliminarCliente").dialog("close");
            });
        },

        CancelarEliminarCliente: function () {
            clienteIdEliminar = 0;
            $("#formularioEliminarCliente").dialog("close");
        },

        //#endregion Listado

        Guardar: function () {
            model.RecogerDatosFormulario();

            util.Ajax("GrabarCliente", JSON.stringify({ item: cliente }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    model.CargarLista();
                    model.LimpiarDatosFormulario();
                    $("#formularioRegistrar").dialog("close");
                    if (cliente.Id > 0) {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    else {
                        util.MsgInfo(resultado.Mensaje);
                    }
                }
                else {
                    util.MsgAlert(resultado.Mensaje);
                }
            });
        },

        LimpiarDatosFormulario: function () {
            $("#txtIdentificador").val("-1");
            $("#txtNombres").val("");
            $("#txtApellidos").val("");
            $("#txtDni").val("");
            $("#txtFechaNacimiento").val("");
            $("#txtDireccion").val("");
            $("#txtTelefono").val("");
            $("#rbSexoMasculino").prop("checked", true);
        },

        Nuevo: function () {
            model.LimpiarDatosFormulario();
            $("#formularioRegistrar").dialog("open");
        },

        Cancelar: function () {
            $("#formularioRegistrar").dialog("close");
        },

        Obtener: function (id) {

            util.Ajax("ObtenerCliente", JSON.stringify({ profesionalId: id }),
            function (data) {
                var item = data.obj;
                if (item != null) {
                    debugger;
                    $("#txtIdentificador").val(item.Id);
                    $("#txtDni").val(item.Dni);
                    $("#txtNombres").val(item.Nombres);
                    $("#txtApellidos").val(item.Apellidos);
                    var f = new Date(parseInt(item.FechaNacimiento.substr(6)));
                    var d = f.getDate();
                    var m = f.getMonth();
                    if (d < 10) d = '0' + d;
                    if (m < 10) m = '0' + m;
                    $("#txtFechaNacimiento").datepicker("option", "dateFormat", "dd/mm/yy");
                    $("#txtFechaNacimiento").datepicker("setDate", new Date(f.getFullYear(), m, d));
                    if (item.Sexo == 'M')
                        $("#rbSexoMasculino").prop("checked", true);
                    else
                        $("#rbSexoFemenino").prop("checked", true);
                    $("#txtTelefono").val(item.Telefono);
                    $("#txtDireccion").val(item.Direccion);
                    $("#formularioRegistrar").dialog("open");
                }
            });
        },

        MarcarMenu: function () {

            $("#optMantenimiento")
                .addClass("active")
                .addClass("menu-open");

            $("#optClientes")
                .addClass("active");
        },

        Inicio: function () {

            this.MarcarMenu();

            $("#btnNuevo").button();
            $("#btnNuevo").on("click", this.Nuevo);

            $("#btnCancelar").button();
            $("#btnCancelar").on("click", this.Cancelar);

            $("#btnGuardar").button();
            $("#btnGuardar").on("click", this.Guardar);

            $("#btnEliminarCliente").button();
            $("#btnEliminarCliente").on("click", this.Eliminar);

            $("#btnCancelarEliminarCliente").button();
            $("#btnCancelarEliminarCliente").on("click", this.CancelarEliminarCliente);

            $('#txtFechaNacimiento').datepicker({ autoclose: true })


            util.Dialog("formularioRegistrar", 'Cliente', 800, 300, 10, null, false);
            util.Dialog("formularioEliminarCliente", "Eliminar Cliente", 400, 161, 10, null, false);


            this.ConfigurarGrilla();
            this.CargarLista();
            $("#contenidoPagina").css('display', 'block');
        }
    }

    model.Inicio();

    window.onresize = function () {
        var grilla = $("#grilla").dxDataGrid("instance");

        if (grilla != null && $(window).height() >= 490) {
            grilla.option({ height: $(window).height() - 280 });
            grilla.resize();
        }
    }
});