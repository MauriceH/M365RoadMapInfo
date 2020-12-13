import {selector} from "recoil";
import {FeatureSlim} from "../model/feature";
import featureListFilteredSorted from "./featureListFilteredSorted";
import {PagedData} from "../model/PagedData";
import {featureListPage, featureListPageSize} from "./FeatureListPaging";


export const featureListLastPage = selector<number>({
    key: 'featureListLastPage',
    get: ({get}) => {
        const features = get(featureListFilteredSorted);
        const pageSize = get(featureListPageSize);
        return Math.ceil(features.length / pageSize);
    }
});

export const featureListFilteredSortedPaged = selector<PagedData<FeatureSlim>>({
    key: 'featureListFilteredSortedPaged',
    get: ({get}) => {
        try {
            const features = get(featureListFilteredSorted);
            const page = get(featureListPage);
            const pageSize = get(featureListPageSize);

            const startIndex = (page - 1) * pageSize;
            const endIndex = Math.min(startIndex + (pageSize - 1), features.length - 1);
            const pagedFeatures = features.slice(startIndex, endIndex)

            const lastPage = get(featureListLastPage)

            return {
                items: pagedFeatures,
                meta: {
                    page: page,
                    lastPage: lastPage
                }
            };
        } catch (e) {
            console.error('pagination selector',e);
            throw e;
        }
    }
});
