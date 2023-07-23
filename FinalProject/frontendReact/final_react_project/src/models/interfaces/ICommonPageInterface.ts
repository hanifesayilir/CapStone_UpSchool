
export default interface ICommonPageInterface<T> {
    items: T[],
    pageNumber: number,
    totalPages: number,
    totalCount: number,
    hasPreviousPage: boolean,
    hasNextPage:boolean

}
