function toggleLoading() {
    const removeIfExists = () => {
        if (document.querySelector('.loading-spinner')) {
            document.querySelector('.loading-spinner').remove();
            document.querySelector('.gray-bg').remove();
            return true
        }
        return false
    }
    if (removeIfExists() == false) {
        let loading = document.createElement('div');
        let grayBg = document.createElement('div');
        loading.innerHTML = `<i class="fas fa-spinner"></i>`;
        loading.classList.add('loading-spinner');
        grayBg.classList.add('gray-bg');
        document.body.appendChild(grayBg);
        document.body.appendChild(loading);
    }
}


function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

Number.prototype.valorBRL = function () {
    return this.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', minimumFractionDigits: 2 });
};

Number.prototype.milharBRL = function () {
    return this.toLocaleString('pt-BR', { style: 'decimal', maximumFractionDigits: 2 });
};

Number.prototype.valorUSD = function () {
    return this.toLocaleString('en-US', { style: 'currency', currency: 'USD', minimumFractionDigits: 2 });
};

Date.prototype.daysSince = function (otherDate) {
    return Math.round((new Date() - this) / (1000 * 60 * 60 * 24));
}

Date.prototype.toStringBR = function () {
    this.setHours(this.getHours() + 3);
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();

    return `${(dd > 9 ? '' : '0') + dd}/${(mm > 9 ? '' : '0') + mm}/${this.getFullYear()}`;
};

async function maskMoneyNoCampo(id, valor) {
    if (!valor) valor = 0;
    let currency = 'R$';

    $(id).maskMoney({
        prefix: currency + ' ',
        allowNegative: false,
        thousands: '.',
        decimal: ',',
        precision: 2
    });

    $(id).maskMoney('mask', valor);
}

const getBaseURL = () => {
    var getUrl = window.location;
    return getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];
}

const isMobile = () => {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent))
        return true

    if (window.innerWidth < 772)
        return true

    return false;
}

const dateBRtoANSI = (strDate) => {
    date = strDate.split('/');
    if (date.length < 3) return NaN;

    date[2] = date[2].length == 2 ? "20" + date[2] : date[2];

    return `${date[2]}-${date[1]}-${date[0]}`
}

const deleteRow = (ev) => {
    ev.parentElement.parentElement.remove();
}

const obterQueryStringParam = param => {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    return params[param];
}

const loadFormFromQueryString = () => {
    const urlSearchParams = new URLSearchParams(window.location.search);
    const params = Object.fromEntries(urlSearchParams.entries());

    for (let key in params) {
        document.getElementById(key).value = params[key];
    }
}

const toggleSection = (id) => {
    $("#" + id).toggle(500);
    document.getElementById('btn-minimize-' + id).classList.toggle('rotate-90dg');
}

function validURL(str) {
    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    return !!pattern.test(str);
}

if (localStorage.getItem('compras') == null)
    localStorage.setItem('compras', true);
if (localStorage.getItem('credito') == null)
    localStorage.setItem('credito', true);
if (localStorage.getItem('comercial') == null)
    localStorage.setItem('comercial', true);