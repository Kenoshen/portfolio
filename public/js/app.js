var app = angular.module("portfolioApp", ["ngRoute", "ngAnimate"]);

app.controller("RootCtrl", function($scope, $rootScope, $location, $anchorScroll) {
	console.log($location.path());
    $rootScope.selectedTab = $location.path();
    $rootScope.home = function() {
        $location.path("");
        $rootScope.selectedTab = $location.path();
    };
    $rootScope.portfolio = function() {
        $location.path("portfolio");
        $rootScope.selectedTab = $location.path();
    };
    $rootScope.resume = function() {
        $location.path("resume");
        $rootScope.selectedTab = $location.path();
    };
    $rootScope.contact = function() {
        $location.path("contact");
        $rootScope.selectedTab = $location.path();
    };
    $rootScope.goToAnchor = function(anchor){
        if ($location.hash() !== anchor) {
          // set the $location.hash to `newHash` and
          // $anchorScroll will automatically scroll to it
          $location.hash(anchor);
        } else {
          // call $anchorScroll() explicitly,
          // since $location.hash hasn't changed
          $anchorScroll();
        }
    };
});

app.config(function($routeProvider) {
    $routeProvider.when('/portfolio', {
        templateUrl : 'html/portfolio.html', 
		controller: 'RootCtrl'
    }).when('/resume', {
        templateUrl : 'html/resume.html', 
		controller: 'RootCtrl'
    }).when('/contact', {
        templateUrl : 'html/contact.html', 
		controller: 'RootCtrl'
    }).otherwise({
        templateUrl : 'html/home.html', 
		controller: 'RootCtrl'
    });
});

app.animation('.reveal-animation', function() {
  return {
    enter: function(element, done) {
      element.css('display', 'none');
      element.fadeIn(500, done);
      return function() {
        element.stop();
      }
    },
    leave: function(element, done) {
      element.fadeOut(500, done)
      return function() {
        element.stop();
      }
    }
  }
})
