$(function () {
    var profesionalId = "",
        fechaAtencion = null;
    var cita = {};
    var model = {

        ObtenerParametros: function () {
            profesionalId = $("#profesionalId").val();

            var temp = $("#fechaAtencion").val();

            fechaAtencion = temp.substr(6, 2) + "/" + temp.substr(4, 2) + "/" + temp.substr(0, 4);
        },

        ListarDatos: function () {

            util.Ajax("../../ListarCitasProfesional", JSON.stringify({ profesionalId: profesionalId, fechaAtencion: fechaAtencion }),
            function (data) {
                var lista = [];
                var evento = {};
                if (data != null) {
                    lista = data;
                    console.log(lista);
                }
                else {
                    util.MsgAlert("No se encontraron registros");
                }
                $('#calendario').fullCalendar('addEventSource', lista)
            });
        },

        ObtenerCita: function (id) {
            util.Ajax("../../ObtenerCita", JSON.stringify({ citaId: id }),
            function (data) {
                var item = data.obj;
                model.LimpiarDatosFormulario();
                if (item != null) {
                    $("#txtIdentificador").val(item.Id);
                    $("#txtEstado").val(item.Estado);
                    debugger;
                    $("#txtObservaciones").val(item.Observaciones);
                    $("#txtClienteId").val(item.ClienteId);
                    $("#txtDni").val(item.ClienteDni);
                    $("#txtApellidosNombres").val(item.ClienteApellidos + ' ' + item.ClienteNombres);
                    $("#txtApellidos").val(item.Apellidos);

                    var f = new Date(parseInt(item.FechaAtencion.substr(6)));
                    var d = f.getDate();
                    var m = f.getMonth();
                    if (d < 10) d = '0' + d;
                    if (m < 10) m = '0' + m;
                    $("#txtFechaAtencion").datepicker("option", "dateFormat", "dd/mm/yy");
                    $("#txtFechaAtencion").datepicker("setDate", new Date(f.getFullYear(), m, d));

                    $("#txtHoraInicio").val(util.FormatJsonTimeSpan(item.HoraInicio));
                    $("#txtHoraFin").val(util.FormatJsonTimeSpan(item.HoraFin));
                    $("#formularioRegistrar").dialog("open");
                }
            });
        },

        ConfigurarCalendario: function () {
            var date = new Date()
            var d = date.getDate(),
                m = date.getMonth(),
                y = date.getFullYear()
            $('#calendario').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                buttonText: {
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Dia'
                },
                height: 500,
                editable: false,
                eventClick: function (calEvent, jsEvent, view) {
                    model.ObtenerCita(calEvent.citaId);
                }
            });
        },

        LimpiarDatosFormulario: function () {
            $("#txtIdentificador").val("-1");
            $("#txtClienteId").val("-1");
            $("#txtEstado").val("PENDIENTE");
            $("#txtObservaciones").val("");
            $("#txtDni").val("");
            $("#txtApellidosNombres").val("");
            $("#txtFechaAtencion").val("");
            $("#txtHoraInicio").val("");
            $("#txtHoraFin").val("");
        },

        NuevaCita: function () {
            model.LimpiarDatosFormulario();
            $("#formularioRegistrar").dialog("open");
        },

        Cancelar: function () {
            $("#formularioRegistrar").dialog("close");
        },

        RecogerDatosFormulario: function () {
            cita = {
                Id: $("#txtIdentificador").val(),
                ProfesionalId: $("#profesionalId").val(),
                Profesional: $("#txtProfesional").val(),
                ClienteId: $("#txtClienteId").val(),
                Cliente: $("#txtApellidosNombres").val(),
                FechaAtencion: $("#txtFechaAtencion").val(),
                HoraInicio: $("#txtHoraInicio").val(),
                HoraFin: $("#txtHoraFin").val(),
                Estado: $("#txtEstado").val(),
                Observaciones: $("#txtObservaciones").val(),
                Activo: true
            };
            debugger;
        },

        Guardar: function () {
            model.RecogerDatosFormulario();

            util.Ajax("../../GrabarCita", JSON.stringify({ item: cita }),
            function (data) {
                var resultado = data.obj;

                if (resultado.Correcto == true) {
                    $('#calendario').fullCalendar('removeEvents');
                    model.ListarDatos();
                    model.LimpiarDatosFormulario();
                    if (cita.Id > 0) {
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

        AutocompletarCliente: function () {

            var asignaValores = function (object) {
                $("#txtDni").val(object.Dni);
                $("#txtClienteId").val(object.Id);
                $("#txtApellidosNombres").val(object.Nombre);
            };

            var asignaValoresEnBlanco = function (value) {
                $("#txtDni").val(value);
                $("#txtClienteId").val("-1");
                $("#txtApellidosNombres").val("");
            };
            util.Autocompletar("txtDni", 2, 150, '../../BuscarCliente', asignaValores, null, null, asignaValoresEnBlanco);
        },

        MarcarMenu: function () {

            $("#otpGestionCita")
                .addClass("active")
                .addClass("menu-open");

            $("#optRegistrarCita")
                .addClass("active");
        },

        Inicio: function () {
            this.MarcarMenu();

            this.ObtenerParametros();

            $("#btnNuevaCita").button();
            $("#btnNuevaCita").on("click", this.NuevaCita);

            $("#btnGuardar").button();
            $("#btnGuardar").on("click", this.Guardar);

            $("#btnCancelar").button();
            $("#btnCancelar").on("click", this.Cancelar);

            this.AutocompletarCliente();

            //$("#txtDni").on("keyup", function () { return model.AutocompletarCliente(); });

            $('#txtFechaAtencion').datepicker({ autoclose: true });

            util.Dialog("formularioRegistrar", 'Registrar Cita', 600, 'auto', 10, null, false);

            this.ConfigurarCalendario();
            this.ListarDatos();
            $("#contenidoPagina").css('display', 'block');
        }
    }

    model.Inicio();
});