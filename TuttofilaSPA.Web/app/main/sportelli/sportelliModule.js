﻿(function (window, angular) {
  'use-strict';
  angular.module('sportelliModule', ['ui.router', 'saleModule', 'serviziModule'])
    .config(function ($stateProvider) {
      $stateProvider
        .state('home.sportelli',
          {
            url: '/sportelli',
            templateUrl: 'app/main/sportelli/sportelli.html',
            controller: 'sportelliCtrl'
          });
    })
    .factory('sportelliService', function ($http) {
      return {
        restituisciServiziChiamati: function (salaId) {
          return $http.get("/api/Sportelli/RestituisciServiziChiamati?salaId=" + salaId);
        },
        chiamaServizio: function (servizio) {
          return $http.post("/api/Sportelli/ChiamaServizio", servizio);
        }
      };
    })
    .controller('sportelliCtrl', function ($scope, $interval, saleService, serviziService, sportelliService) {
      $scope.chiama = function () {
        sportelliService.chiamaServizio($scope.servizio);
      };

      saleService.list().then(function (result) {
        $scope.Sale = result.data.value;
      });

      serviziService.list().then(function (result) {
        $scope.Servizi = result.data.value;
      });

      $interval(function () {
        if (!angular.isUndefinedOrNull($scope.salaId)) {
          sportelliService.restituisciServiziChiamati($scope.salaId);
        }
      }, 2000);
    });
})(window, window.angular);
