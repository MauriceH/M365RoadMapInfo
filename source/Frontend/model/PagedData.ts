

export interface PagedData<T> {
    items: T[],
    meta: {
        page: number,
        lastPage: number
    }
}