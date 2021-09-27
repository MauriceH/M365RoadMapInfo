import {atom, selector} from "recoil";
import {FeatureSlim} from "../model/feature";
import featureListData from "./featureListData";

export const featureListSearchValue = atom<string>({key: 'featureListSearchValue', default: ''});

export const featureListProductFilter = atom<string[]>({key: 'featureListProductFilter', default: []});

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
        const products = get(featureListProductFilter);


        return features.filter(feature => {
            let isOk = true;
            if(titleSearchValue != '') {
                isOk = feature.description.toLowerCase().includes(titleSearchValue);
            }
            if (!isOk) return false
            if (products.length > 0) {
                isOk = false;
                const cat = feature.tagCategories?.find(cat => cat.category == 'Product')
                if (cat == null) {
                    return false
                } else {
                    for (let i = 0; i < cat.tags.length; i++) {
                        const tag = cat.tags[i];
                        for (let j = 0; j < products.length; j++) {
                            if (products[j] == tag) {
                                isOk = true;
                                break;
                            }
                        }
                        if (isOk) break;
                    }
                }
            }
            if (!isOk) return false
            return isOk
        });
    }
});

export default featureListFiltered;