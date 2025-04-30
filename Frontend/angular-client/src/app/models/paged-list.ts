export interface PagedList<T> {
    items: T[];
    totalCount: number;
    currentPage: number;
    pageSize: number;
}