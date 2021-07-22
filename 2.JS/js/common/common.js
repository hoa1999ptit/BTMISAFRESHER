var CommonFn = CommonFn || {};

/**
 * 
 * @param {Xâu javascript date} thisdate 
 * @returns Xâu ở dạng yyyy-MM-dd
 */
CommonFn.formatDateYMD = thisdate => {
    date = new Date(thisdate);
    return date.getFullYear() + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + ("0" + date.getDate()).slice(-2);
};

/**
 * 
 * @param {Xâu javascript date} thisdate 
 * @returns Xâu ở dạng dd/MM/yyyy
 */
CommonFn.formatDateDMY = thisdate => {
    date = new Date(thisdate);
    return ("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();
};
/**
 * 
 * @param {xâu bất kỳ} myinput 
 * @returns Xâu số (dấu ngăn . cách phần nghìn )
 */
CommonFn.formatMoney = myinput => {
    myinput += "";
    if (myinput != null) {
        myinput.replaceAll(".", "")
        let onlynumber = '';
        for (var i = 0; i < myinput.length; i++) {
            if (!isNaN(parseInt(myinput[i], 10))) {
                onlynumber += myinput[i];
            }
        }
        return (Number(onlynumber).toLocaleString('vi'));
    }
    return 0;
};

CommonFn.formatNumber = myinput => {
    myinput += "";
    if (myinput != null) {
        myinput.replaceAll(".", "")
        let onlynumber = '';
        for (var i = 0; i < myinput.length; i++) {
            if (!isNaN(parseInt(myinput[i], 10))) {
                onlynumber += myinput[i];
            }
        }
        return onlynumber;
    }
    return 0;
};

CommonFn.validName = myname => {
    if (myname.length == 0) return -1;
    for (var i = 0; i < myname.length; i++) {
        if (!isNaN(parseInt(myname[i], 10))) {
            return 0;
        }
    }
    return 1;
}
CommonFn.validEmail = myemail => {
    if (myemail.length == 0) return -1;
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (re.test(String(myemail).toLowerCase()) == true) return 0;
    return 1;
}
CommonFn.validIdentity = myid => {
    if (myid.length == 0) return -1;
    if (myid.length == 9 || myid.length == 13) {
        for (var i = 0; i < myid.length; i++) {
            if (isNaN(parseInt(myid[i], 10))) {
                return 0;
            }
        }
    } else {
        return -2;
    }
    return 1;
}
CommonFn.validPhoneNumber = myphone => {
    if (myphone.length == 0) return -1;
    if (myphone == 10) {
        if (isNaN(myphone)) {
            return 0;
        }
    } else {
        return -2;
    }
    return 1;
}