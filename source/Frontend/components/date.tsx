import { parseISO, format } from 'date-fns'

export function formatDate(dateString:string) {
  try {
    const date = parseISO(dateString)
    return format(date, 'dd.LL.yyyy')
  } catch (e) {
    return '-'
  }
}

export default function Date({ dateString }: { dateString: string }) {
  const date = parseISO(dateString)
  return <time dateTime={dateString}>{format(date, 'dd.LL.yyyy')}</time>
}