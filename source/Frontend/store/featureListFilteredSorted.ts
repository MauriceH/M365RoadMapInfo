import {selector} from "recoil";
import {FeatureSlim} from "../model/feature";
import featureListFiltered from "./featureListFiltered";




export const featureListFilteredSorted = selector<FeatureSlim[]>({
    key: 'featureListFilteredSorted',
    get: ({get}) => {
        const features = get(featureListFiltered);
        //sort
        return features;
    }
});


export default featureListFilteredSorted;