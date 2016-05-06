﻿(function () {

    "use strict";
    angular.module("app-trips")
    .controller("tripsController", tripsController);


    function tripsController($http) {
        var vm = this;
        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;


        $http.get("/api/trips")
            .then(function (response) {
                angular.copy(response.data, vm.trips);
                

            }, function (error) {
                vm.errorMessage = "Fail to load" + error;
            })
            .finally(function () {
                vm.isBusy = false;
            })


        vm.addTrip = function () {
            vm.trips.push({ name: vm.newTrip.name, created: new Date() });
            //vm.newTrip.name = "";

            vm.newTrip = {};
        };
    }
})();