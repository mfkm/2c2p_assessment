(function () {
    var app = angular.module('uploader', []);

    app.controller('UploadController', ["$scope", "$http", '$rootScope', 'apiURL', function ($scope, $http, $rootScope, apiURL) {
        console.log("UploadController logged on.");

        $scope.finalResult = {
            IsSuccess: false,
            Message: null
        };

        $scope.uploadFile = function (fileInput) {
            console.log("UserController.uploadFile triggered.");
            if (fileInput == undefined || fileInput == null) {
                $scope.finalResult.Message = 'Invalid file.';
                return;
            }
            if (fileInput.filesize > 1024000) {
                $scope.finalResult.Message = 'The maximum allowed file size is 1MB.';
                return;
            }
            var fileNameArr = fileInput.filename.split('.');
            if (fileNameArr.length < 2) {
                $scope.finalResult.Message = 'Unknown format!';
                return;
            }
            var input = {
                Base64String: fileInput.base64,
                FileExtension: fileNameArr[fileNameArr.length - 1],
                FileSize: fileInput.filesize
            };
            $http({
                method: 'POST',
                url: apiURL + '/api/file/upload',
                data: JSON.stringify(input),
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            }).then(function onSuccess(response) {
                $scope.finalResult = response.data;
            }).catch(function onError(response) {
                console.log(response);
                $rootScope.spinnerShow = false;
            });
        };

    }]);

})();
