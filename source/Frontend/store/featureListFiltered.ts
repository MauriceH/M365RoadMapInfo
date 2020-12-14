import {atom, selector} from "recoil";
import {FeatureSlim} from "../model/feature";
import featureListData from "./featureListData";

export const featureListSearchValue = atom<string>({key: 'featureListSearchValue', default: ''});

export const featureListFilteredTotalCount = selector<number>({
    key: 'featureListFilteredTotalCount',
    get: ({get}) => {
        const features = get(featureListFiltered);
        return features.length;
    }
});

export const featureListFiltered = selector<FeatureSlim[]>({
    key: 'featureListFiltered',
    get: ({get}) => {
        const features = get(featureListData);
        const titleSearchValue = get(featureListSearchValue)?.toLowerCase() ?? '';

        return features.filter(feature => {
            return feature.description.toLowerCase().includes(titleSearchValue)
        });
    }
});

export default featureListFiltered;