(function (window, angular) {
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

      var connection = $.hubConnection();
      var hub = connection.createHubProxy("SportelliHub");
      hub.on("aggiornaServizi", function (serviziChiamati) {
        // check sala
        for (var i = 0; i < serviziChiamati.length; i++) {
          if (serviziChiamati[i].SalaId === $scope.salaId) {
            $scope.ServiziChiamati.push(serviziChiamati[i]);
          }
        }
        $scope.$apply();
      });
      connection.start();

      $scope.ServiziChiamati = [];

      $scope.chiama = function () {
        sportelliService.chiamaServizio($scope.servizio);
      };

      saleService.list().then(function (result) {
        $scope.Sale = result.data.value;
      });

      serviziService.list().then(function (result) {
        $scope.Servizi = result.data.value;
      });

      /*
      $interval(function () {
        if ($scope.salaId) {
          sportelliService.restituisciServiziChiamati($scope.salaId).then(function (result) {
            for (var i = 0; i < result.data.length; i++) {
              $scope.ServiziChiamati.push(result.data[i]);
            }
          });
        }
      }, 2000);
      */

    });
})(window, window.angular);
