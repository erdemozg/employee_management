export const dateHelper = {
  dateToRFCDateTimeString,
  dateToRFCDateString
};

function f(n) {    // Format integers to have at least two digits.
  return n < 10 ? '0' + n : n;
}

function dateToRFCDateTimeString(date){
  if (!date) {
    return ""
  }

  return date.getUTCFullYear()    + '-' +
        f(date.getUTCMonth() + 1) + '-' +
        f(date.getUTCDate())      + 'T' +
        f(date.getUTCHours())     + ':' +
        f(date.getUTCMinutes())   + ':' +
        f(date.getUTCSeconds())   + 'Z';
}

function dateToRFCDateString(date) {
  console.log(date)
  if (!date) {
    return ""
  }
  
  return date.getUTCFullYear()    + '-' +
        f(date.getUTCMonth() + 1) + '-' +
        f(date.getUTCDate())      + 'T' +
        '00'                      + ':' +
        '00'                      + ':' +
        '00'                      + 'Z';
}
