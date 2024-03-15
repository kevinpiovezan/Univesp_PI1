const tipos = [
    {tipo: 1, descricao: "Balanco e DRE", combo: ["Balanço", "DRE", "Balanço e DRE"]},
    {tipo: 3, descricao: "IR com Recibo dos Sócios", combo: ["IR", "Recibo", "IR com Recibo"]},
    {tipo: 4, descricao: "Imposto de Renda com recibo", combo: ["IR", "Recibo", "IR com Recibo"]},
    {tipo: 8, descricao: "CAR", combo: null},
    { tipo: 10, descricao: "RG/CPF/CNPJ", combo: ["RG", "CPF", "CNPJ"]},
    { tipo: 11, descricao: "Registro do Comerciante", combo: null},
];

let dropElems = document.getElementsByClassName('drop-section');

;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    for (elem of dropElems)
        elem.addEventListener(eventName, preventDefaults, false)
})

function preventDefaults(e) {
    e.preventDefault()
    e.stopPropagation()
}


for (elem of dropElems) {
    elem.addEventListener('drop', async function (ev) {
        let dt = ev.dataTransfer
        let files = dt.files
        await lerArquivoAsync(files[0], $(this).data('id-arquivo'), $(this).data('id-cliente'), $(this).data('id-ciclo'), $(this).data('ano'),  $(this).data('obrigatorio'));
    }, false)
}

       
function adicionarDownload(arquivo_base64, extensao, nome, idArquivo, idAluno, idCiclo, obrigatorio, comentario = null, tipo = "", ano = 0) {
    var arquivo = {
        "base64": arquivo_base64,
        "extensao": extensao,
        "descricao": nome,
        ano: parseInt(ano),
        comentario: comentario,
        tipo: tipo
    };
    toggleLoading();
    $.ajax({
        type: "POST",
        url: `/arquivo/adicionar-arquivo/${idCiclo}/${idArquivo}/${idAluno}/${obrigatorio}`,
        contentType: "application/json",
        data: JSON.stringify(arquivo)
    }).done(function (data) {
        toggleLoading();
        if (data) {
            toastr.success("Arquivo adicionado com sucesso!");
            location.reload();
        }
        return;

    }).fail(function (xhr, status, error) {
        toggleLoading();
        toastr.error(xhr.responseText);
    });
}

function downloadArquivo(idArquivo) {
    toggleLoading()
    $.ajax({
        type: "GET",
        url: `/arquivo/obter-arquivo/${idArquivo}`,
    }).done(function (data) {
        toggleLoading()
        if (!data) {
        toastr.error("Nenhum arquivo encontrado");
        } else {
            var link = document.createElement("a");
            document.body.appendChild(link);
            link.setAttribute("type", "hidden");
            link.href = "data:text/plain;base64," + data.base64;
            link.download = `${data.descricao}.${data.extensao}`;
            link.click();
            document.body.removeChild(link);
        }
    }).fail(function (xhr, status, error) {
        toggleLoading()
        toastr.error(xhr.responseText);
    });
}

function downloadMultiplos(idArquivo, idCiclo, idAluno, ano, nomeArquivo) {
    toggleLoading()
    $.ajax({
        type: "GET",
        url: `/arquivo/obter-arquivos-multiplos/${idArquivo}/${idCiclo}/${idAluno}/${ano}`,
    }).done(function (data) {
        toggleLoading()
        if (!data) {
            toastr.error("Nenhum arquivo encontrado");
        } else {
            var link = document.createElement("a");
            document.body.appendChild(link);
            link.setAttribute("type", "hidden");
            link.href = "data:text/plain;base64," + data;
            link.download = `${nomeArquivo}.zip`;
            link.click();
            document.body.removeChild(link);
        }
    }).fail(function (xhr, status, error) {
        toggleLoading()
        toastr.error(xhr.responseText);
    });
}

async function anexarArquivos(input, idArquivo, idAluno, idCiclo, ano, obrigatorio = true) {
    var arquivos = input.files;
    if (arquivos) {
        for (const arquivo of arquivos) {
            await lerArquivoAsync(arquivo, idArquivo, idAluno, idCiclo, ano, obrigatorio);
        }
    }
}

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

async function anexarMultiplos(input, idArquivo, idAluno, idCiclo, ano,title, obrigatorio = true) {
    let body = document.getElementById("body-multiplos");
    console.log(title);
    document.getElementById("modal-multiplos-title").innerHTML = title;
    body.innerHTML = "";
    var arquivos = input.files;
    var socios = "";
    let valores = ""; 
    let combo = "";
    let tipo = tipos.find(f => f.tipo === idArquivo);
    if(tipo.combo != null){
        for (const opcao of tipo.combo){
            combo += `<option value="${opcao}"> ${opcao} </option>`;
        }
    }
    if (arquivos) {
        let i = 0;
        for (const arquivo of arquivos) {
            if (tipo.descricao === "IR com Recibo dos Sócios") {
                socios = `<div class="col-lg-6 d-flex align-items-center">
                    <label>Nome do sócio: </label>
                </div>
                <div class="col-lg-6">
                    <input id="cpf_cnpj_${i}" type="text" class="form-control cpf_cnpj"/>
                </div>`;
            } else if (tipo.descricao === "RG/CPF/CNPJ") {
                socios = `<div class="col-lg-6 d-flex align-items-center">
                    <label>Nome: </label>
                </div>
                <div class="col-lg-6">
                    <input id="cpf_cnpj_${i}" type="text" class="form-control cpf_cnpj"/>
                </div>`;
            } else if (tipo.descricao === "CAR") {
                socios = `<div class="col-lg-6 d-flex align-items-center">
                    <label>Matrícula do CAR e Município/UF: </label>
                </div>
                <div class="col-lg-6">
                    <input id="cpf_cnpj_${i}" type="text" class="form-control cpf_cnpj"/>
                </div>`;
            }
            else if (tipo.descricao === "Registro do Comerciante") {
                socios = `<div class="col-lg-6 d-flex align-items-center">
                    <label>Número do Registro: </label>
                </div>
                <div class="col-lg-6">
                    <input id="cpf_cnpj_${i}" type="text" class="form-control cpf_cnpj"/>
                </div>`;
            }
            if(tipo.combo != null){
                valores = `<div class="col-lg-6 d-flex align-items-center">
                        <label for="combo">Tipo de documento: </label>
                    </div>
                    <div class="col-lg-6 d-flex align-items-center">
                        <select class="form-control" id="combo-${i}">${combo}</select>
                    </div>`;   
            } else if (idArquivo != 8 && idArquivo != 10 && idArquivo != 11 ){
                valores= `<div class="col-lg-6 d-flex align-items-center">
                        <label for="combo">Inscrição e nome da fazenda: </label>
                    </div>
                    <div class="col-lg-6 d-flex align-items-center">
                        <input class="form-control" type="text" id="combo-${i}"/>
                    </div>`;
            }
            let base64 = await toBase64(arquivo);
            body.innerHTML += `<div class="form-group col-lg-12 my-2">
                    <label>Nome do arquivo: ${arquivo.name}</label>
                </div>
                ${socios}
                ${valores}
                <button class="d-none enviar" onclick="lerArquivoAsync('${base64}', ${idArquivo}, ${idAluno}, ${idCiclo}, ${ano}, ${obrigatorio},'${arquivo.name.replaceAll(" ","")}', ${i})"></button>`
            i++;
        }
        body.innerHTML += `<div class="col-lg-12">
                    <button id="validaMultiplos" class="btn-primary btn mt-3" onclick="validaMultiplos()">Enviar arquivos</button>
                </div>`
    }
    $("#modalMultiplos").modal();
}

function validaMultiplos(){
    let dados = document.querySelectorAll(".cpf_cnpj");
    for(let i = 0; i < dados.length; i++){
        if(dados[i].value == "" || dados[i].value == null){
            toastr.error("Todos os campos 'Nome dos Sócios' devem estar preenchidos");
            return;
        }
    }
    $(".enviar").click();
    document.getElementById("validaMultiplos").disabled = true;
    return;
}

async function reutilizarArquivo(idArquivo, idAluno, idCiclo, ano) {
    toggleLoading();
    await $.ajax({
        type: "POST",
        url: `/arquivo/reutilizar-arquivo/${idArquivo}/${idAluno}/${idCiclo}/${ano}`,
    }).done(function (data) {
        toggleLoading()
        toastr.success("Arquivo reutilizado com sucesso!")
        location.reload();
    }).fail(function (xhr, status, error) {
        toggleLoading()
        toastr.error(xhr.responseText);
    });
}

const formatosValidos = arquivo => {
    return /\.pdf$/i.test(arquivo.name);
}

function lerArquivoAsync(arquivo, idArquivo, idAluno, idCiclo, ano, obrigatorio, nome, index) {
    debugger;
    if (idArquivo == "" || idArquivo == 0)
        return toatr.error("Tipo de arquivo inválido");
    var re = /(?:\.([^.]+))?$/;
    if (nome != null && nome.includes("pdf")){
        let comentario = $("#cpf_cnpj_"+index).val();
        let tipo = $("#combo-"+index).val();
        var ext = re.exec(nome)[1];
        adicionarDownload(arquivo, ext, nome, idArquivo, idAluno, idCiclo, obrigatorio, comentario, tipo, ano);
    } else if(formatosValidos(arquivo)) {
        var ext = re.exec(arquivo.name)[1];
        let reader = new FileReader();
    reader.onload = () => {
        adicionarDownload(reader.result, ext, arquivo.name, idArquivo, idAluno, idCiclo, obrigatorio, null, null, ano);
    };
    reader.readAsDataURL(arquivo);
    } else {
        alert(FORMATO_INVALIDO_MSG);
        return;
    }
}

const FORMATO_INVALIDO_MSG = 'Formato de arquivo inválido.';
const ARQUIVO_NAO_INSERIDO_MSG = 'Não foi possível inserir o arquivo.';
const ARQUIVO_NAO_OBTIDO_MSG = 'Não foi possível obter os arquivos.';