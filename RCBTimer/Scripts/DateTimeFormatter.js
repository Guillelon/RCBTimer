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

function FormatAMPM(date)
{
    let hours = date.getHours();
    let minutes = date.getMinutes();
    let ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    hours = hours.toString().padStart(2, '0');
    minutes = minutes.toString().padStart(2, '0');
    let strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function FormattedDate(date) {
    return weekDaysEs[date.getDay()] + ', ' + date.getDate() + ' de ' + monthsEs[date.getMonth()] + ' de ' + date.getUTCFullYear();
}

function ToTimestamp(time) {
    var myMoment = moment.duration(time); 
    return myMoment.hours().toString().padStart(2, '0') + ":"
        + myMoment.minutes().toString().padStart(2, '0');
}   

function SubtractMinutesToTimestamp(time, minutes) {
    var otherMoment = moment.duration(time); 
    otherMoment.subtract(minutes, 'm');
    return otherMoment.hours().toString().padStart(2, '0') + ":"
        + otherMoment.minutes().toString().padStart(2, '0');
}
