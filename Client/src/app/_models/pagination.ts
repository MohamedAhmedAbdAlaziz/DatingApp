export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  total: number;
}
export class PaginationResult<T> {
  result: T;
  pagination: Pagination;
}
