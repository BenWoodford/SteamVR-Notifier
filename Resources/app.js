angular.module('svrNotifier', [
    'svrNotifier.controllers',
    'svrNotifier.services',
    'svrNotifier.directives',
    'svrNotifier.filters',
    'svrNotifier.factories',
]).
config(['$compileProvider', function ($compileProvider) {
    $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|steam):/);
}]);

angular.module('svrNotifier.controllers', []);
angular.module('svrNotifier.services', []);
angular.module('svrNotifier.directives', []);
angular.module('svrNotifier.filters', []);
angular.module('svrNotifier.factories', []);

angular.module('svrNotifier.controllers').controller('NotificationsController', ['$window', function ($window) {
    var ctrlr = this;
    this.notifications = [];

    $window.newNotification = function (title, message) {
        ctrlr.notifications.push({
            title: title,
            message: message
        });

        $window.notifications.sendNotification(title + "\r\n" + message, null, 3000);
    }

    $window.newNotification("Facebook", "Joe Bloggs liked your photo");
    $window.newNotification("Twitter", "@gabelnewell followed you");
    $window.newNotification("Gabe Newell", "Hey man, love your work. I'd like to give you a 50% share in Valve Software, and a $50,000,000 grant for cool VR stuff");
}]);