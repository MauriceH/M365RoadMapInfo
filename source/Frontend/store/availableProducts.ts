import {selector} from "recoil";
import featureListData from "./featureListData";

export const availableProducts = selector<string[]>({
    key: 'availableProducts',
    get: ({get}) => {
        const features = get(featureListData);
        const set = new Set<string>();
        features.map(f => {
            f.tagCategories?.find(cat => cat.category == 'Product')?.tags.map(p => {
                return set.add(p);
            });
        });
        const products = Array.from(set.values());
        products.sort()
        return products;
    }
});

export default availableProducts;