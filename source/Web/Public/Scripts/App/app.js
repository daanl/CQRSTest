(function ($, hubConnection) {
    'use strict';

    var app = angular.module('CQRS.Example', []).
        config(function ($routeProvider) {
            $routeProvider.
                when('/', {
                    controller: 'OrderController',
                    templateUrl: '/templates/list.html'
                }).
                when('/:id', {
                    controller: 'DetailController',
                    templateUrl: '/templates/detail.html'
                }).
                otherwise({ redirectTo: '/' });
        });
    
    app.factory('orderHub', function () {
        return $.connection.orderHub;
    });

})(jQuery, hubConnection);
