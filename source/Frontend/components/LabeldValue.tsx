import {Typography} from "@material-ui/core";
import React from "react";
import {styled} from "@material-ui/styles";


const ValueLabel = styled('b')({
    width: '90px',
    paddingBottom: '8px',
    display: 'inline-block'
})

export interface Props {
    label: string
    children: React.ReactNode
}

export function LabeledValue({label ,children}:Props) {
    return (
        <Typography variant={"body1"}>
            <ValueLabel>{label}</ValueLabel>
            {children}
        </Typography>);
}