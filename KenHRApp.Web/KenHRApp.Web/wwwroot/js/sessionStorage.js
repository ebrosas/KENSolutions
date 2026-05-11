window.hrmsSessionStorage = {

    set: function (key, value) {

        sessionStorage.setItem(
            key,
            JSON.stringify(value));
    },

    get: function (key) {

        const item = sessionStorage.getItem(key);

        if (!item)
            return null;

        return JSON.parse(item);
    },

    remove: function (key) {

        sessionStorage.removeItem(key);
    }
};