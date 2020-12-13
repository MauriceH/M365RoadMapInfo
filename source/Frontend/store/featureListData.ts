import {atom, selector} from "recoil";
import {FeatureSlim} from "../model/feature";


export const featureListData = atom<FeatureSlim[]>({key: 'featureListData', default: []});

export default featureListData;


