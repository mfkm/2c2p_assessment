(function () {
    var app = angular.module('AT', [
        'uploader',
        'naif.base64'
    ]);

    app.factory('apiURL', function () {
        var url = 'https://localhost:7014';
        return url;
    });

    app.run(function ($rootScope, apiURL) {
        $rootScope.apiURL = apiURL;
    });

})();