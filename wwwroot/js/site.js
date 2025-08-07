$(function(){
    $("#buscar").on("click", function(){
        let filtro = "Descripcion";
        let busqueda = $("#input-busqueda input").val();

        $.get("api/ingresos/filter?filtro=" + filtro + "&Busqueda1=" + busqueda, function(ingresos){
            mostrarDatos(ingresos);
        });
    });
});

// Datos de la tabla
function mostrarDatos(ingresos){
    $("#table-ingresos tbody tr").remove();
    let lista = $("#table-ingresos tbody");
    for(let ingreso of ingresos){
        // Fila
        let fila = $("<tr>");

        // ID
        fila.attr("id", ingreso.id_ingreso);

        // Mostrar descripción
        fila.on("click", function(){
            mostrarDesc(ingreso.id_ingreso);
        });

        // Fecha
        let fechaObj = new Date(ingreso.fecha);
        let fechaFormateada = String(fechaObj.getDate()) + "/" +
            String(fechaObj.getMonth() + 1) + "/" +
            fechaObj.getFullYear();

        let fecha = $("<td class='table-fecha'>" + fechaFormateada + "</td>");
        fila.append(fecha);

        // Empresa
        let empresa;
        if(ingreso.id_empresa == null){
            empresa = $("<td class='table-empresa'>-</td>");
        }
        else{
            if(ingreso.id_empresa == 1){
                empresa = $(
                    `<td class="fila-correo fila-empresa table-empresa">
                        <section>
                            <img src="img/correoarg.jpg">
                            <p>`+ ingreso.empresa.nombre + `</p>
                        </section>
                    </td>`
                )
            }
            else if(ingreso.id_empresa == 2){
                empresa = $(
                    `<td class="fila-andreani fila-empresa table-empresa">
                        <section>
                            <img src="img/andreani.png">
                            <p>` + ingreso.empresa.nombre + `</p>
                        </section>
                    </td>`
                );
            }
            else{
                empresa = $(
                    `<td class="fila-mercado-libre fila-empresa table-empresa">
                        <section>
                            <img src="img/mercado-libre.png">
                            <p>` + ingreso.empresa.nombre + `</p>
                        </section>
                    </td>`
                );
            }
        }

        fila.append(empresa);

        // Tipo
        let tipo = $("<td class='table-tipo'>" + ingreso.tipo.nombre + "</td>");
        fila.append(tipo);

        // Monto
        let monto;
        let montoFormateado = Math.abs(parseFloat(ingreso.monto)).toLocaleString("es-AR", { minimumFractionDigits: 2, maximumFractionDigits: 2 });

        if(ingreso.monto > 0){
            monto = $("<td class='ingreso-positivo table-monto'>+ $ " + montoFormateado + "</td>");
        }
        else{
            monto = $("<td class='ingreso-negativo table-monto'>- $ " + montoFormateado + "</td>");
        }

        fila.append(monto);

        // Descripción
        let descripcion;

        if(ingreso.descripcion == ""){
            descripcion = $("<td class='table-descripcion'><i class='sin-desc'>Sin descripción</i></td>");
        }
        else{
            if(ingreso.descripcion.length > 20){
                descripcion = $("<td class='table-descripcion'><p class='desc-corta'>" + String(ingreso.descripcion).slice(0, 20) + "</p><p class='desc-larga'>" + ingreso.descripcion + "</p></td>");
            }
            else{
                descripcion = $("<td class='table-descripcion'><p class='desc-corta'>" + ingreso.descripcion + "</p></td>");
            }
        }

        fila.append(descripcion);

        // Acciones
        let acciones = $(`<td class="table-acciones">
            <a href="#" onclick="editarDatoTabla(` + ingreso.id_ingreso + `, '` + String(ingreso.fecha).slice(0, 10) + `', ` + ingreso.id_empresa + `, ` + ingreso.id_tipo + `, ` + ingreso.monto + `, '` + ingreso.descripcion + `')">
                <svg viewBox="0 0 16 16">
                    <use xlink:href="img/pen.svg"/>
                </svg>
            </a>
            <form method="post" asp-page-handler="Eliminar" style="display:inline;">
                <input type="hidden" name="id" value="` + ingreso.id_ingreso + `" />
                <button type="submit" class="boton-borrar">
                    <svg viewBox="0 0 16 16">
                        <use xlink:href="img/trash.svg"/>
                    </svg>
                </button>
            </form>
            </td>`);

        fila.append(acciones);

        lista.append(fila);
    }
}

// Ocultar el form de nuevo ingreso
function ocultarIngresoNuevo(){
    formulario.style.display = "none";
}

// Mostrar el form de nuevo ingreso
function mostrarIngresoNuevo(){
    tituloFormulario.textContent = "Nuevo ingreso";
    let fechaActual = new Date();

    // Datos del form por defecto
    id.value = 0;
    fecha.value = fechaActual.toISOString().slice(0, 10);
    monto.value = "";
    tipo.children[0].selected = true;
    empresaSelect.children[1].selected = true;
    descripcion.value = "";

    tipoCheck();

    // Cambio de botones
    document.getElementById("boton-ingresar").style.display = "inline-block";
    document.getElementById("boton-editar").style.display = "none";

    // Mostrar el formulario
    formulario.style.display = "block";
}

// Mostrar el formulario para editar
function editarDatoTabla(id_ingreso, fechaForm, id_empresa, id_tipo, montoForm, descripcionForm){
    id.value = id_ingreso;
    fecha.value = fechaForm;
    tipo.value = id_tipo;
    monto.value = parseFloat(montoForm).toFixed(2);
    descripcion.value = descripcionForm;

    tipoCheck();

    if(id_empresa != null){
        empresaSelect.value = id_empresa;
    }

    // Cambio de titulo del formulario
    tituloFormulario.textContent = "Editando ingreso";

    // Cambio de botones
    document.getElementById("boton-editar").style.display = "inline-block";
    document.getElementById("boton-ingresar").style.display = "none";
    
    // Mostrar el formulario
    formulario.style.display = "block";
}

// Controlar el campo "empresa" al seleccionar el tipo
function tipoCheck(){
    for(let x of tipo.children){
        if(x.value == 4 && x.selected){
            empresaSelect.children[0].selected = true;
            empresaSelect.disabled = true;
            return;
        }
    }

    if(empresaSelect.disabled){
        empresaSelect.children[1].selected = true;
    }
    empresaSelect.disabled = false;
}

// Página de ingresos
function cargarPagina(numPagina){
    let parametros = new URLSearchParams(window.location.search);
    let url = window.location.href;
    url = url.split("?")[0];
    parametros.set("pagina", numPagina);
    window.location.href = url + "?" + parametros.toString();
}

function paginaSiguiente(){
    let parametros = new URLSearchParams(window.location.search);
    let url = window.location.href;
    url = url.split("?")[0];

    if(parametros.has("pagina")){
        parametros.set("pagina", parseInt(parametros.get("pagina")) + 1);
    }
    else{
        parametros.set("pagina", 2);
    }

    window.location.href = url + "?" + parametros.toString();
}

function paginaAtras(){
    let parametros = new URLSearchParams(window.location.search);
    let url = window.location.href;
    url = url.split("?")[0];

    if(parametros.has("pagina")){
        if(parametros.get("pagina") == "1"){
            return;
        }
        else{
            parametros.set("pagina", parseInt(parametros.get("pagina")) - 1);
        }
    }
    else{
        return;
    }

    window.location.href = url + "?" + parametros.toString();
}

// Mostrar descripción
function mostrarDesc(id){
    for(let x of tabla.getElementsByClassName("desc-larga")){
        if(x.style.display == "block"){
            x.style.display = "none";
        }
    }

    for(let x of tabla.getElementsByClassName("desc-corta")){
        if(x.style.display == "none"){
            x.style.display = "block";
        }
    }

    let ing = document.getElementById(id);
    let descCorta = ing.getElementsByClassName("desc-corta");
    let descLarga = ing.getElementsByClassName("desc-larga");

    if(descLarga.length > 0){
        descCorta[0].style.display = "none";
        descLarga[0].style.display = "block";
    }
}

// Obtener los elementos
const formulario = document.getElementById("ingresar-form");
const tituloFormulario = document.getElementById("titulo-form");
const tabla = document.getElementById("table-ingresos");
const id = document.getElementById("id-ingreso");
const tipo = document.getElementById("tipo");
const fecha = document.getElementById("fecha");
const empresaSelect = document.getElementById("empresa");
const monto = document.getElementById('monto');
const descripcion = document.getElementById('descripcion');

// Limitar el monto a solo dos decimas
monto.addEventListener('blur', function() {
    let valor = this.value;
    if(valor){
        if(valor < -100000){
            valor = -100000;
        }
        if(valor > 100000){
            valor = 100000;
        }
        this.value = parseFloat(valor).toFixed(2);
    }
});