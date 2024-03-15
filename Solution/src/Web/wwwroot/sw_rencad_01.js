/* IMPORTANT:
 Update version every time a new version of the application is deployed
 */
assetCacheName = "v0.0.0";

self.addEventListener('install', function (event) {
    event.waitUntil(
        caches.open(assetCacheName).then(function (cache) {
            return cache.addAll([

                // Páginas
                
                // CSS

                // Script

                // Icones
                '/img/icons/access_denied.svg',
                '/img/icons/error_404.svg',
                '/img/icons/excel.png',

                // Logos e favicons
                '/favicon.ico',
                '/img/logos/apple-touch-icon.png',
                '/img/logos/android-chrome-192x192.png',
                '/img/logos/favicon-32x32.png',
                '/img/logos/favicon-16x16.png',
                '/img/logos/logo_white-syngenta.png',
                '/manifest.json',

                // Libs:

                //multi select

                //toastr
                '/lib/toastr/toastr.css',
                '/lib/toastr/toastr.js.map',
                '/lib/toastr/toastr.min.js',

                //bootbox
                '/lib/bootbox/bootbox.all.min.js',

                //Bootstrap
                '/lib/bootstrap/dist/css/bootstrap.min.css',
                '/lib/bootstrap/dist/js/bootstrap.bundle.min.js',

                //Jquery
                '/lib/jquery-validation/dist/additional-methods.js',
                '/lib/jquery-validation/dist/jquery.validate.js',
                '/lib/jquery-validation/dist/additional-methods.min.js',
                '/lib/jquery-validation/dist/jquery.validate.min.js',
                '/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
                '/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js',

                //font-awesome
                '/lib/fontawesome-free-5.15.2-web/css/all.min.css',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-brands-400.svg',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-brands-400.eot',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-brands-400.ttf',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-brands-400.woff',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-brands-400.woff2',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-regular-400.eot',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-regular-400.svg',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-regular-400.ttf',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-regular-400.woff',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-regular-400.woff2',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-solid-900.eot',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-solid-900.svg',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-solid-900.ttf',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-solid-900.woff',
                '/lib/fontawesome-free-5.15.2-web/webfonts/fa-solid-900.woff2',
            ]);
        })
    );
});

self.addEventListener("activate", event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== assetCacheName) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

self.addEventListener('fetch', event => {
    event.respondWith(
        fetch(event.request).catch(() => {
            return caches.match(event.request);
        })
    );
});

self.addEventListener('push', function (e) {
    var data = e.data.json();
    var options = {
        body: data.Body,
        icon: data.Image,
        vibrate: [100, 50, 100],
        data: {
            dateOfArrival: Date.now(),
            link: data.Link,
        }
    };
    e.waitUntil(
        self.registration.showNotification(data.Title, options)
    );
});

self.addEventListener('notificationclick', function (e) {
    var notification = e.notification;
    var action = e.action;
    var data = notification.data;

    if (action === 'close') {
        notification.close();
    } else {
        clients.openWindow(data.link);
        notification.close();
    }
});