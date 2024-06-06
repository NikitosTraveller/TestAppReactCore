import moment from 'moment';

export default function DateFormatter(date, format) {
    if (!date) {
        return "";
    }
    return moment(date).format(format);
}