export default function ParseDate(date : string) : string {
    return date.split('T')[0]
}