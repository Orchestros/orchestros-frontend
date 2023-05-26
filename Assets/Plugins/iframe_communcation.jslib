mergeInto(LibraryManager.library, {
    sendMessageToParent: function(message) {
        window.top.postMessage(
            Pointer_stringify(message),
            "*"
        );
    },
    registerMessageCallback: function() {
        console.log("Registering message callback");
        window.addEventListener(
            "message",
            (event) => {
                globalUnityInstance.SendMessage(
                    "ExportManager",
                    "OnTriggerSave",
                );
            },
            false
        );
    }
});
