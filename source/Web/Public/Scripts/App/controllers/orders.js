(function () {
    'use strict';

    var app = angular.module('CQRS.Example');

    app.controller(
        'OrderController',
        function ($scope, orderHub) {

            orderHub.server.orders().
                done(function (data) {
                    console.log(data);
                });
          }
    );
})();