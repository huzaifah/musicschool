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
