import {atom, selector} from "recoil";
import {FeatureSlim} from "../model/feature";
import featureListFiltered from "./featureListFiltered";


export interface Sorting {
    key: keyof FeatureSlim
    order: SortOrder
}

export type SortOrder = 'asc' | 'desc'

export const featureListSorting = atom<Sorting>({
    key: 'featureListSorting',
    default: {key: "lastModified", order: "asc"}
});


export const featureListFilteredSorted = selector<FeatureSlim[]>({
    key: 'featureListFilteredSorted',
    get: ({get}) => {
        const features = get(featureListFiltered);
        const sorting = get(featureListSorting);
        if((features?.length ?? 0) <= 0) return  features;
        return [...features].sort((a, b) => {
            const aValue = a[sorting.key] ?? ''
            const bValue = b[sorting.key] ?? ''
            let sortValue = 0
            if (aValue < bValue) {
                sortValue =  -1;
            } else if (aValue > bValue) {
                sortValue =  1;
            }
            return sortValue * (sorting.order == "asc" ? 1 : -1);
        });
    }
});


export default featureListFilteredSorted;