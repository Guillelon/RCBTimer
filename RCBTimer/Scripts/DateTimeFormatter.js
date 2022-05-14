// Creamos array con los meses del año
const monthsEs = ['ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO', 'SEPT', 'OCT', 'NOV', 'DIC'];
// Creamos array con los días de la semana
const weekDaysEs = ['domingo', 'lunes', 'martes', 'miércoles', 'jueves', 'viernes', 'sábado'];

function FormattedTime(date) {
    var hh = date.getHours();
    var m = date.getMinutes();
    var s = date.getSeconds();
    var dd = "AM";
    var h = hh;
    if (h >= 12) {
        h = hh - 12;
        dd = "PM";
    }
    if (h == 0) {
        h = 12;
    }
    m = m < 10 ? "0" + m : m;

    var time = h + ":" + m + " " + dd;
    
    return time;
}