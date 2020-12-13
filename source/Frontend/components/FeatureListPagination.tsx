import {useRecoilState, useRecoilValue} from "recoil";
import {featureListPage} from "../store/FeatureListPaging";
import React, {useCallback} from "react";
import {featureListLastPage} from "../store/featureListFilteredSortedPaged";


export const FilterListPagination = () => {
    const lastPage = useRecoilValue(featureListLastPage);
    const [page, setPage] = useRecoilState(featureListPage);
    const nextPage = useCallback(() => {
        const newPage = Math.min(page + 1, lastPage);
        setPage(newPage)
    }, [page, lastPage])
    const previousPage = useCallback(() => {
        const newPage = Math.max(page - 1, 1);
        setPage(newPage)
    }, [page, lastPage])
    return (
        <>
            <button onClick={previousPage}>Prev</button>
            <div style={{width: '35px', display: 'inline-block', textAlign: 'center'}}>{page}</div>
            von
            <div style={{width: '35px', display: 'inline-block', textAlign: 'center'}}>{lastPage}</div>
            <button onClick={nextPage}>Next</button>
        </>
    )
}