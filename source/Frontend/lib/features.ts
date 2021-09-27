import Feature from "../model/feature";
import createGetRequestOptions from "./api";


export const getAllFeatureHashes = async () => {
    const userPass = {user: process.env.BACKEND_USER, pass: process.env.BACKEND_PASS};
    const res = await fetch(process.env.BACKEND_HOST + '/roadmap/features-hashes',createGetRequestOptions(userPass));
    return await res.json() as string[]
}

export const getFeatureData = async (hash: string): Promise<Feature> => {
    const userPass = {user: process.env.BACKEND_USER, pass: process.env.BACKEND_PASS};
    const res = await fetch(process.env.BACKEND_HOST + '/roadmap/features/' + hash,createGetRequestOptions(userPass));
    return await res.json() as Feature
};