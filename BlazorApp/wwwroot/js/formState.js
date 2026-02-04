window.formState = {
    save: function (key, value) {
        try {
            localStorage.setItem(key, value);
        } catch (e) {
            console.warn('localStorage not available:', e);
        }
    },
    load: function (key) {
        try {
            return localStorage.getItem(key);
        } catch (e) {
            console.warn('localStorage not available:', e);
            return null;
        }
    },
    remove: function (key) {
        try {
            localStorage.removeItem(key);
        } catch (e) {
            console.warn('localStorage not available:', e);
        }
    }
};

// Download file function for CSV export
window.downloadFile = function (filename, base64Content) {
    const link = document.createElement('a');
    link.href = 'data:text/csv;charset=utf-8;base64,' + base64Content;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
