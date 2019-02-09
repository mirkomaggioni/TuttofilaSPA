(function (window, angular) {
  'use-strict';
  angular.module('serviziModule', ['ui.router', 'saleModule'])
    .config(function ($stateProvider) {
      $stateProvider
        .state('home.servizi',
          {
            url: '/servizi',
            templateUrl: 'app/main/servizi/servizi.html',
            controller: 'serviziCtrl'
          })
        .state('home.servizio',
          {
            url: '/servizio/:id',
            templateUrl: 'app/main/servizi/servizio.html',
            controller: 'servizioCtrl'
          });
    })
    .factory('serviziService', function ($http) {
      return {
        list: function () {
          return $http.get("/odata/Servizi?$expand=Sala");
        },
        detail: function (id) {
          return $http.get("/odata/Servizi(" + id + ")");
        },
        create: function (servizio) {
          var req = {
            method: 'POST',
            url: '/odata/Servizi',
            headers: {
              'Content-Type': 'application/json'
            },
            data: servizio
          };

          return $http(req);
        },
        save: function (servizio) {
          var req = {
            method: 'PATCH',
            url: "/odata/Servizi(" + servizio.Id + ")",
            headers: {
              'Content-Type': 'application/json'
            },
            data: servizio
          };

          return $http(req);
        },
        delete: function (id) {
          return $http.delete("/odata/Servizi(" + id + ")");
        }
      };
    })
    .controller('serviziCtrl', function ($scope, $state, serviziService) {

      $scope.new = function () {
        $state.go("home.servizio", { id: null });
      };

      $scope.detail = function (id) {
        $state.go("home.servizio", { id: id });
      };

      serviziService.list().then(function (result) {
        $scope.Servizi = result.data.value;
      });
    })
    .controller('servizioCtrl', function ($scope, $state, $stateParams, serviziService, saleService) {

      $scope.save = function () {
        if ($scope.Servizio.Id === undefined) {
          serviziService.create($scope.Servizio).then(function (result) {
            $scope.Servizio = result.data;
          });
        } else {
          serviziService.save($scope.Servizio).then(function () { });
        }
      };

      $scope.delete = function () {
        serviziService.delete($scope.Servizio.Id).then(function () {
          $state.go("home.servizi");
        });
      };

      $scope.close = function () {
        $state.go("home.servizi");
      };

      saleService.list().then(function (result) {
        $scope.Sale = result.data.value;
      });

      if ($stateParams.id === '') {
        $scope.Servizio = { Nome: '' };
      } else {
        serviziService.detail($stateParams.id).then(function (result) {
          $scope.Servizio = result.data;
        });
      }
    });
})(window, window.angular);
