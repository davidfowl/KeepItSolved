//cardsController.js

(function () {
	"use strict";

	//getting existing module
	angular.module("app-cards")
		.controller("cardsController", cardsController);

	function cardsController($http) {
		var vm = this;

		vm.cards = [];

		vm.newCard = {};

		vm.errorMessage = "";
		vm.isBusy = true;

		$http.get("/api/cards")
			.then(function (response) {
				//success
				angular.copy(response.data, vm.cards);
			}, function (error) {
				//failure
				vm.errorMessage = "Failed to load data: " + error;
			})
			.finally(function(){
				vm.isBusy = false;
			});

		vm.addCard = function () {
			vm.isBusy = true;
			vm.errorMessage = "";

			$http.post("/api/cards", vm.newCard)
				.then(function (response) {
					//success
					vm.cards.push(response.data);
					vm.newCard = {};
				}, function (error) {
					//failure
					vm.errorMessage = "Failed to save new trip";
				})
				.finally(function () {
					vm.isBusy = false;
				})
		};
	}

})();