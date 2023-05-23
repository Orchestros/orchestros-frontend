var ImageUploaderPlugin = {
  ImageUploaderCaptureClick: function() {
    var fileInput;
    if (document.getElementById("ImageUploaderInput") == null) {
      console.log("Init image uploaded");
      var fileInput = document.createElement('input');
      fileInput.setAttribute('type', 'file');
      fileInput.setAttribute('id', 'ImageUploaderInput');
      fileInput.style.visibility = 'hidden';
      fileInput.onclick = function (event) {
        this.value = null;
      };
      fileInput.onchange = function (event) {
        SendMessage('EventSystem', 'FileSelected', URL.createObjectURL(event.target.files[0]));
      }
      fileInput.onabort = function (event) {
        SendMessage('EventSystem', 'FileSelected', '');
      }
      document.body.appendChild(fileInput);

      console.log("Input created");
    } else {
      fileInput = document.getElementById("ImageUploaderInput");
    }
    var OpenFileDialog = function() {
      fileInput.click();
      document.getElementById('unity-canvas').removeEventListener('click', OpenFileDialog, false);
    };
    
    document.getElementById('unity-canvas').addEventListener('click', OpenFileDialog, false);
  }
};
mergeInto(LibraryManager.library, ImageUploaderPlugin);
