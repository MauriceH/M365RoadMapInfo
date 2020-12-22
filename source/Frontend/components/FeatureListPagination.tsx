import {useRecoilState, useRecoilValue} from "recoil";
import {featureListPage, featureListPageSize} from "../store/FeatureListPaging";
import React from "react";
import {TablePagination} from "@material-ui/core";
import {featureListFilteredTotalCount} from "../store/featureListFiltered";


export const FilterListPagination = () => {
    const totalCount = useRecoilValue(featureListFilteredTotalCount);
    const [page, setPage] = useRecoilState(featureListPage);
    const [pageSize, setPageSize] = useRecoilState(featureListPageSize);
    return (
        <TablePagination
            rowsPerPageOptions={[5, 10, 20, 50]}
            component="div"
            count={totalCount}
            rowsPerPage={pageSize}
            page={page}
            onChangePage={(e,p)=>setPage(p)}
            onChangeRowsPerPage={(e)=>{setPageSize(parseInt(e.target.value, 10))}}
        />
    )
}