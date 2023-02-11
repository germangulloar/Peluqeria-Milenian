$(function () {
    var profesional = {};
    var EliminarFilaSeleccionada = "";
    var indiceTipoServicio = 0,
        profesionalIdEliminar = 0;
    var listaTipoServicios = new Array(),
        listaTipoServiciosEliminadas = new Array();
    var model = {

        //#region Listado


        RecogerDatosFormulario: function () {

            listaTipoServicios = [];

            $("#tbListadoTipoServicios tr").each(function (index, fila) {
                listaTipoServicios.push({
                    Id: $(fila).find("input[name=txtId]").val(),
                    TipoServicioId: $(fila).find("input[name=txtTipoServicioId]").val(),
                    TipoServicio: $(fila).find("input[name=txtTipoServicio]").val(),
                    TipoServicioDescripcion: '',
                    Activo: true,
                });
            });

            listaTipoServicios = listaTipoServicios.concat(listaTipoServiciosEliminadas);

            var fechaNacimiento = util.GetDate($("#txtFechaNacimiento").val());

            profesional = {
                Id: $("#txtIdentificador").val(),
                Nombres: $("#txtNombres").val(),
                Apellidos: $("#txtApellidos").val(),
                Dni: $("#txtDni").val(),
                Direccion: $("#txtDireccion").val(),
                Correo: $("#txtCorreo").val(),
                Telefono: $("#txtTelefono").val(),
                Sexo: $('input[name=rbSexo]:checked').val(),
                NumeroColegiatura: $("#txtNumColegiatura").val(),
                FechaNacimiento: (fechaNacimiento == null) ? '' : fechaNacimiento,
                Activo: true,
                ListaTipoServicios: listaTipoServicios
            };
        },

        CargarLista: function () {
            util.Ajax("ListarProfesionales", JSON.stringify({}),
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
                { dataField: 'Dni', caption: 'DNI', width: '15%', alignment: 'center' },
                { dataField: 'Telefono', caption: 'Telefono', width: '15%', alignment: 'center' },
                { dataField: 'NumeroColegiatura', caption: 'N° Colegiatura', width: '10%', alignment: 'center' },
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
                            .on('click', function () { model.EliminarProfesional(id) })
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

        EliminarProfesional: function (id) {
            $("#formularioEliminarProfesional").dialog("open");
            profesionalIdEliminar = id;
        },

        Eliminar: function () {
            profesional = {
                Id: profesionalIdEliminar,
                Nombres: '',
                Apellidos: '',
                Dni: '',
                Direccion: '',
                Correo: '',
                Telefono: '',
                Sexo: '',
                NumeroColegiatura: '',
                FechaNacimiento: '',
                Activo: false,
                ListaTipoServicios: null
            };

            util.Ajax("EliminarProfesional", JSON.stringify({ item: profesional }),
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
                $("#formularioEliminarProfesional").dialog("close");
            });
        },

        CancelarEliminarProfesional: function () {
            ProfesionalIdEliminar = 0;
            $("#formularioEliminarProfesional").dialog("close");
        },

        //#endregion Listado

        Guardar: function () {
            model.RecogerDatosFormulario();

            util.Ajax("GrabarProfesional", JSON.stringify({ item: profesional }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    model.CargarLista();
                    model.LimpiarDatosFormulario();
                    if (profesional.Id > 0) {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    else {
                        util.MsgInfo(resultado.Mensaje);
                    }
                    $("#formularioRegistrar").dialog("close");
                }
                else {
                    util.MsgAlert(resultado.Mensaje);
                }
            });
        },

        LimpiarDatosFormulario: function () {
            $("#tbListadoTipoServicios").empty();

            listaTipoServicios = [];
            listaTipoServiciosEliminadas = [];
            $("#txtIdentificador").val("-1");
            $("#txtNombres").val("");
            $("#txtApellidos").val("");
            $("#txtDni").val("");
            $("#txtFechaNacimiento").val("");
            $("#txtDireccion").val("");
            $("#txtCorreo").val("");
            $("#txtTelefono").val("");
            $("#txtNumColegiatura").val("");
            $("#rbSexoMasculino").prop("checked", true);
            indiceTipoServicio = 0;
        },

        Nuevo: function () {
            model.LimpiarDatosFormulario();
            $("#formularioRegistrar").dialog("open");
        },

        Cancelar: function () {
            $("#formularioRegistrar").dialog("close");
        },

        Obtener: function (id) {

            util.Ajax("ObtenerProfesional", JSON.stringify({ profesionalId: id }),
            function (data) {
                var item = data.obj;
                if (item != null) {
                    $("#txtIdentificador").val(item.Id);
                    $("#txtDni").val(item.Dni);
                    $("#txtNumColegiatura").val(item.NumeroColegiatura);
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
                    $("#txtCorreo").val(item.Correo);
                    $("#txtTelefono").val(item.Telefono);
                    $("#txtDireccion").val(item.Direccion);
                    $("#formularioRegistrar").dialog("open");
                    listaTipoServiciosEliminadas = [];
                    if (item.ListaTipoServicios != null)
                        model.ImprimirListaTipoServicios(item.ListaTipoServicios);
                }
            });
        },

        LimpiarListaTipoServicio: function () {
            indiceTipoServicio = 0;
            $("#tbListadoTipoServicios").html(
                            '<tr id="trTipoServicio0">' +
                                '<td style="width:10%" align="center">' +
                (indiceTipoServicio + 1) +
                                '</td>' +
                                '<td style="width:80%">' +
                                    '<input type="hidden" name="txtId" value="0" />' +
                                    '<input type="hidden" name="txtTipoServicioId" value="" />' +
                                    '<input type="text" name="txtTipoServicio" id="txtTipoServicio_0" value="" class="control-generate" />' +
                                '</td>' +
                                '<td style="width:10%" align="center">' +
                                    '<a title="Eliminar TipoServicio" id="btnEliminarTipoServicio0" name="btnEliminarTipoServicio" style="cursor:pointer"><i class="fa fa-fw fa-trash"></i></a>' +
                                '</td>' +
                            '</tr>');

            $("#btnEliminarTipoServicio0").on("click", function () { return model.EliminarFilaTipoServicio(this); });
            model.AutocompletarTipoServicio("txtTipoServicio_0", 0);
            indiceTipoServicio++;
        },

        AgregarTipoServicio: function () {
            if (indiceTipoServicio == 0) {
                model.LimpiarListaTipoServicio();
            }
            else {
                $("#tbListadoTipoServicios tr:eq(0)").clone().appendTo("#tbListadoTipoServicios");
                $("#tbListadoTipoServicios tr:last td:eq(0)").html(indiceTipoServicio + 1);
                $("#tbListadoTipoServicios tr:last td:eq(1)").find("input[name=txtId]").val("0");
                $("#tbListadoTipoServicios tr:last td:eq(1)").find("input[name=txtTipoServicioId]").val("");
                $("#tbListadoTipoServicios tr:last td:eq(1)").find("input[name=txtTipoServicio]").val("").attr("id", "txtTipoServicio_" + indiceTipoServicio);
                model.AutocompletarTipoServicio("txtTipoServicio_" + indiceTipoServicio, indiceTipoServicio);
                $("#tbListadoTipoServicios tr:last td:eq(2)").find("a[name=btnEliminarTipoServicio]").on("click", function () { return model.EliminarFilaTipoServicio(this); }).css('display', 'inline').attr("id", "btnEliminarTipoServicio" + indiceTipoServicio);
                $("#tbListadoTipoServicios tr:last").attr("id", "trTipoServicio" + indiceTipoServicio);
                indiceTipoServicio += 1;

                if (indiceTipoServicio > 5) $("#tblCabecera").css('width', '97.8%');
                else $("#tblCabecera").css('width', '100%');
            }
        },

        ImprimirListaTipoServicios: function (lista) {
            if (lista == null) return;
            var strHtml = "";
            indiceTipoServicio = 0;
            for (var i = 0; i < lista.length; i++, indiceTipoServicio++) {
                strHtml += '<tr id="trTipoServicio' + i + '">' +
                                '<td style="width:10%" align="center">' +
                                    (i + 1) +
                                '</td>' +
                                '<td style="width:80%">' +
                                    '<input type="hidden" name="txtId" value="' + lista[i].Id + '" />' +
                    '<input type="hidden" name="txtTipoServicioId" value="' + lista[i].TipoServicioId + '" />' +
                    '<input type="text" name="txtTipoServicio" id="txtTipoServicio_' + i + '" value="' + lista[i].TipoServicio + '" class="control-generate" />' +
                                '</td>' +
                                '<td style="width:10%" align="center">' +
                    '<a title="Eliminar Servicio" id="btnEliminarTipoServicio' + i + '" name="btnEliminarTipoServicio" style="cursor:pointer"><i class="fa fa-fw fa-trash"></i></a>' +
                                '</td>' +
                            '</tr>';
            }
            $("#tbListadoTipoServicios").html(strHtml);

            for (var j = 0; j < lista.length; j++) {
                model.AutocompletarTipoServicio("txtTipoServicio_" + j, j);
                $("#btnEliminarTipoServicio" + j + "").on("click", function () { return model.EliminarFilaTipoServicio(this); });
            }
        },

        AutocompletarTipoServicio: function (control, fila) {
            var asignaValores = function (d) {
                $("#tbListadoTipoServicios tr:eq(" + fila + ") td:eq(1)").find("input[name=txtTipoServicioId]").attr('value', d.Id);
                $("#" + control).val(d.Nombre);
            };

            var asignaValoresEnBlanco = function (value) {
                $("#tbListadoTipoServicios tr:eq(" + fila + ") td:eq(1)").find("input[name=txtTipoServicioId]").attr('value', "0");
                $("#" + control).val(value);
            };
            util.Autocompletar(control, 2, 150, 'BuscarTipoServicio', asignaValores, null, null, asignaValoresEnBlanco);
        },

        EliminarFilaTipoServicio: function (control) {
            var tr = $(control).parents().eq(1),
                id = tr.children()[1].children.txtId.value;


            EliminarFilaSeleccionada = control;

            if (id == 0 || id == "") {
                var indiceFila = tr.index();
                $(control).closest('tr').remove();
                indiceTipoServicio--;

                if (indiceTipoServicio > 5) $("#tblCabecera").css('width', '97.8%');
                else $("#tblCabecera").css('width', '100%');

                model.OrdenarCorrelativo("tbListadoTipoServicios", indiceFila);
                model.OrdenarControlesTipoServicios(indiceFila, { posColumna: 1, name: "txtTipoServicio" });
            } else {
                $("#formularioEliminarTipoServicio").dialog("open");
            }
        },

        OrdenarCorrelativo: function (tabla, indiceFila, control) {
            var totalFilas = $("#" + tabla + " tr").length;

            if (indiceFila < totalFilas) {
                var n = (totalFilas - indiceFila);
                for (var i = 0; i < n ; i++) {
                    if (control != undefined)
                        $("#" + tabla + " tr:eq(" + (indiceFila + i) + ") td:eq(0)").find("label[name=" + control + "]").html((indiceFila + 1 + i));
                    else
                        $("#" + tabla + " tr:eq(" + (indiceFila + i) + ") td:eq(0)").html((indiceFila + 1 + i));
                }
            }
        },

        OrdenarControlesTipoServicios: function (indiceFila, control1) {
            console.log("indice fila: " + indiceFila);
            var tabla = "tbListadoTipoServicios";
            var totalFilas = $("#" + tabla + " tr").length;
            if (indiceFila < totalFilas) {
                var n = (totalFilas - indiceFila);
                for (var i = 0; i < n ; i++) {
                    $("#" + tabla + " tr:eq(" + (indiceFila + i) + ")").attr("id", "trTipoServicio" + (indiceFila + i));
                    $("#" + tabla + " tr:eq(" + (indiceFila + i) + ")").find("a[name=btnEliminarTipoServicio]").attr("id", "btnEliminarTipoServicio" + (indiceFila + i));
                    if (control1 != undefined) {
                        $("#" + tabla + " tr:eq(" + (indiceFila + i) + ") td:eq(" + control1.posColumna + ")").find("input[name=" + control1.name + "]").attr("id", control1.name + "_" + (indiceFila + i));
                        model.AutocompletarTipoServicio(control1.name + "_" + (indiceFila + i), (indiceFila + i));
                    }
                }
            }
            console.log("indice fila: " + indiceFila);
            console.log("total filas: " + totalFilas);
        },

        EliminarTipoServicio: function () {
            if (EliminarFilaSeleccionada != undefined) {

                var tr = $(EliminarFilaSeleccionada).parents().eq(1);

                listaTipoServiciosEliminadas.push({
                    Id: tr.children()[1].children.txtId.value,
                    TipoServicioId: tr.children()[1].children.txtTipoServicioId.value,
                    TipoServicio: tr.children()[1].children.txtTipoServicio.value,
                    TipoServicioDescripcion: '',
                    Activo: false,
                });

                var indiceFila = tr.index();
                $(EliminarFilaSeleccionada).closest('tr').remove();

                model.OrdenarCorrelativo("tbListadoTipoServicios", indiceFila);
                model.OrdenarControlesTipoServicios(indiceFila, { posColumna: 1, name: "txtTipoServicio" });
                indiceTipoServicio--;

                if (indiceTipoServicio > 5) $("#tblCabecera").css('width', '97.8%');
                else $("#tblCabecera").css('width', '100%');

                $("#formularioEliminarTipoServicio").dialog("close");
            }
        },

        CancelarEliminarTipoServicio: function () {
            $("#formularioEliminarTipoServicio").dialog("close");
        },

        MarcarMenu: function () {

            $("#optMantenimiento")
                .addClass("active")
                .addClass("menu-open");

            $("#optProfesionales")
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

            $("#btnEliminarTipoServicio").button();
            $("#btnEliminarTipoServicio").on("click", this.EliminarTipoServicio);

            $("#btnCancelarEliminarTipoServicio").button();
            $("#btnCancelarEliminarTipoServicio").on("click", this.CancelarEliminarTipoServicio);

            $("#btnAgregarTipoServicio").button();
            $("#btnAgregarTipoServicio").on("click", this.AgregarTipoServicio);

            $("#btnEliminarProfesional").button();
            $("#btnEliminarProfesional").on("click", this.Eliminar);

            $("#btnCancelarEliminarProfesional").button();
            $("#btnCancelarEliminarProfesional").on("click", this.CancelarEliminarProfesional);

            $('#txtFechaNacimiento').datepicker({ autoclose: true });


            util.Dialog("formularioRegistrar", 'Profesional', 800, 540, 10, null, false);
            util.Dialog("formularioEliminarTipoServicio", "Eliminar Servicio", 400, 161, 10, null, false);
            util.Dialog("formularioEliminarProfesional", "Eliminar Profesional", 400, 161, 10, null, false);

            $("#div-body").css("height", 140);

            this.ConfigurarGrilla();
            this.CargarLista();
            listaTipoServiciosEliminadas = [];
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