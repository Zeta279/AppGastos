$(function(){
    $("#probar").on("click", function(){
        $.get("Probando", function(data){
            console.log(data);
        });
    });
});

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
        if(x.value == 2 && x.selected){
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