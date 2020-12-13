import {atom} from "recoil";


export const featureListRequestFiles = atom<string[]>({key: 'featureListRequestFiles', default: []});

export default featureListRequestFiles;