import {useRecoilState, useRecoilValue} from "recoil";
import React, {useCallback} from "react";
import availableProducts from "../store/availableProducts";
import {Chip, PropTypes, Typography} from "@material-ui/core";
import {featureListProductFilter} from "../store/featureListFiltered";


export const ProductSelect = () => {
    const products = useRecoilValue(availableProducts);
    const [selectedProducts, setSelectedProducts] = useRecoilState(featureListProductFilter);
    const clickHandler = useCallback(product => {
        if(selectedProducts.find(p=>p==product) == null) {
            setSelectedProducts(old => {
                const newVar = [...old, product];
                return newVar;
            })
        } else {
            setSelectedProducts(old => {
                const prods = new Array<string>()
                old.forEach(oldp=>{
                    if(oldp == product) return;
                    prods.push(oldp)
                })
                return prods;
            })
        }
    },[selectedProducts, setSelectedProducts]);
    return (
        <div style={{display: 'flex', flexWrap:'wrap'}}>
            <Typography style={{width:'100%'}}>Products</Typography>
            {
                products.map(p => {
                    const isActive = selectedProducts.find(sp=> sp == p) != null;
                    const color : Exclude<PropTypes.Color, 'inherit'> = isActive ?  'primary' : 'default'
                    return (
                        <Chip key={p} label={p} size="small" style={{margin:'2px'}} color={color} clickable onClick={()=>clickHandler(p)} />
                    )
                })
            }

        </div>
    )
}