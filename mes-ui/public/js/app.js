export function startApp () {
  let editorUiInit = EditorUi.prototype.init;

  EditorUi.prototype.init = function () {
    editorUiInit.apply(this, arguments);
    this.actions.get('export').setEnabled(false);

    // Updates action states which require a backend
    if (!Editor.useLocalStorage) {
      mxUtils.post(OPEN_URL, '', mxUtils.bind(this, function (req) {
        let enabled = req.getStatus() != 404;
       // enabled = true;
        this.actions.get('open').setEnabled(enabled || Graph.fileSupport);
        this.actions.get('import').setEnabled(enabled || Graph.fileSupport);
        this.actions.get('save').setEnabled(enabled);
        this.actions.get('saveAs').setEnabled(enabled);
        this.actions.get('export').setEnabled(enabled);
      }));
    }
  };

  // Adds required resources (disables loading of fallback properties, this can only
  // be used if we know that all keys are defined in the language specific file)
  mxResources.loadDefaultBundle = false;
  let bundle = mxResources.getDefaultBundle(RESOURCE_BASE, mxLanguage) ||
    mxResources.getSpecialBundle(RESOURCE_BASE, mxLanguage);

  // Fixes possible asynchronous requests
  mxUtils.getAll([bundle, STYLE_PATH + '/default.xml'], function (xhr) {
    // Adds bundle text to resources
    mxResources.parse(xhr[0].getText());

    // Configures the default graph theme
    let themes = new Object();
    themes[Graph.prototype.defaultThemeName] = xhr[1].getDocumentElement();

    // Main
    new EditorUi(new Editor(urlParams['chrome'] == '0', themes),document.querySelector("#main"));
    //new EditorUi();
  }, function () {
    document.body.innerHTML = '<center style="margin-top:10%;">Error loading resource files. Please check browser console.</center>';
  });
};
