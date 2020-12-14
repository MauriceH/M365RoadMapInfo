import Feature from "../model/feature";
import {Grid, Typography} from "@material-ui/core";
import {LabeledValue} from "./LabeldValue";
import {formatDate} from "./date";
import React from "react";
import {styled} from "@material-ui/styles";

export interface Props {
    feature : Feature
}

export function FeatureInfoLabels({feature} : Props) {
    return <Grid container>
        <Grid item md={6} xs={12}>
            <div style={{minWidth:'180px'}}>
                <LabeledValue label={'Added:'}>
                    {formatDate(feature.addedToRoadmap)}
                </LabeledValue>
                <LabeledValue label={'Modified:'}>
                    {formatDate(feature.lastModified)}
                </LabeledValue>
            </div>
        </Grid>
        <Grid item md={6} xs={12}>
            <div style={{minWidth:'180px'}}>
                <LabeledValue label={'Status:'}>
                    {feature.status}
                </LabeledValue>
                <LabeledValue label={'Release:'}>
                    {feature.release}
                </LabeledValue>
            </div>
        </Grid>
    </Grid>;
}