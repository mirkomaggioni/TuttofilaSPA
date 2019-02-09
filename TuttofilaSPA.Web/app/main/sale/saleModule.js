(function (window, angular) {
  'use-strict';
  angular.module('saleModule', ['ui.router'])
    .config(function ($stateProvider) {
      $stateProvider
        .state('home.sale',
          {
            url: '/sale',
            templateUrl: 'app/main/sale/sale.html',
            controller: 'saleCtrl'
          })
        .state('home.sala',
          {
            url: '/sala/:id',
            templateUrl: 'app/main/sale/sala.html',
            controller: 'salaCtrl'
          });
    })
    .factory('saleService', function ($http) {
      return {
        list: function () {
          return $http.get("/odata/Sale");
        },
        detail: function (id) {
          return $http.get("/odata/Sale(" + id + ")");
        },
        create: function (sala) {
          var req = {
            method: 'POST',
            url: '/odata/Sale',
            headers: {
              'Content-Type': 'application/json'
            },
            data: sala
          };

          return $http(req);
        },
        save: function (sala) {
          var req = {
            method: 'PATCH',
            url: "/odata/Sale(" + sala.Id + ")",
            headers: {
              'Content-Type': 'application/json'
            },
            data: sala
          };

          return $http(req);
        },
        delete: function (id) {
          return $http.delete("/odata/Sale(" + id + ")");
        }
      };
    })
    .controller('saleCtrl', function ($scope, $state, saleService) {

      $scope.new = function () {
        $state.go("home.sala", { id: null });
      };

      $scope.detail = function (id) {
        $state.go("home.sala", { id: id });
      };

      saleService.list().then(function (result) {
        $scope.Sale = result.data.value;
      });
    })
    .controller('salaCtrl', function ($scope, $state, $stateParams, saleService) {

      $scope.save = function () {
        if ($scope.Sala.Id === undefined) {
          saleService.create($scope.Sala).then(function (result) {
            $scope.Sala = result.data;
          });
        } else {
          saleService.save($scope.Sala).then(function () { });
        };
      };

      $scope.delete = function () {
        saleService.delete($scope.Sala.Id).then(function () {
          $state.go("home.sale");
        });
      };

      $scope.close = function () {
        $state.go("home.sale");
      };

      if ($stateParams.id === '') {
        $scope.Sala = { Nome: '' };
      } else {
        saleService.detail($stateParams.id).then(function (result) {
          $scope.Sala = result.data;
        });
      }
    });
})(window, window.angular);
