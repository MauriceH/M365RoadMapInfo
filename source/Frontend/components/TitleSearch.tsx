import {useRecoilState, useSetRecoilState} from "recoil";
import React, {useCallback} from "react";
import {featureListSearchValue} from "../store/featureListFiltered";
import {featureListPage} from "../store/FeatureListPaging";


export const TitleSearch = () => {
    const [searchValue, setSearchValue] = useRecoilState(featureListSearchValue);
    const setPage = useSetRecoilState(featureListPage);

    const onSearch = useCallback((value) => {
        setPage(0)
        setSearchValue(value)
    }, [setSearchValue])
    return (
        <>
            <input type='text' onChange={e => onSearch(e.target.value)} value={searchValue}/>
        </>
    )
}