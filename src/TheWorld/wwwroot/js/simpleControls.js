(function () {
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);


    function waitCursor() {
        return {
            scope: {
                show:"=displayWhen"
            },
            restricted:"E",
            templateUrl: "/views/waitCursor.html"
        };
    }


})();