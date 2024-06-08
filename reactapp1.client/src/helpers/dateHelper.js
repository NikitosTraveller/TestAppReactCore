import moment from 'moment';

export default function formatDate(date, format) {
    if (!date) {
        return "";
    }
    return moment(date).format(format);
}