import Metadata from "./Metadata";

export interface PaginationResult<T> {
    items: T
    meta: Metadata
}
export default PaginationResult