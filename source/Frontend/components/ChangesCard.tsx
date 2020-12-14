import Feature, {Change} from "../model/feature";
import {Card, CardHeader, Chip} from "@material-ui/core";
import {formatDate} from "./date";
import React from "react";
import AddIcon from '@material-ui/icons/Add';
import RemoveIcon from '@material-ui/icons/Remove';

interface ChangeProps {
    changes: Change[]
}


const FeatureChangeEntry = ({change}: { change: Change }) => {


    if (change.property.toLowerCase().startsWith('tag')) {
        const Icon = change.oldValue === 'add' ? <AddIcon/> : <RemoveIcon/>
        return (<Chip icon={Icon} label={change.newValue} size={'small'} style={{margin: '0 2px'}}/>)
    }

    return (
        <>
            <div style={{'marginLeft': '20px'}}>
                <label style={{width: '30px', display: 'inline-block'}}>New:</label><span
                style={{'marginLeft': '20px'}}>{change.newValue}</span>
            </div>
            <div style={{'marginLeft': '20px'}}>
                <label style={{width: '30px', display: 'inline-block'}}>Old:</label><span
                style={{'marginLeft': '20px', color:'#777777'}}><i>{change.oldValue}</i></span>
            </div>
        </>
    );
}


export const FeatureChange = ({changes}: ChangeProps) => {
    return (
        <div
            style={{'marginLeft': '20px', display: 'flex', flexDirection: 'row'}}>
            <span>{changes[0].property}:</span>
            <div>
                {changes.map(change => {
                    return (<FeatureChangeEntry change={change} key={change.property + change.oldValue + change.newValue }/>)
                })}
            </div>
        </div>
    );
};


interface Props {
    feature: Feature
}

interface ChangeGroup {
    property: string
    changes: Change[];
}

function group(changes: Change[]): ChangeGroup[] {
    let groups: ChangeGroup[]
    groups = []
    changes.map(change => {
        let group = groups.find(value => value.property == change.property)
        if (group == null) {
            group = {property: change.property, changes: []}
            groups.push(group)
        }
        group.changes.push(change)
    })
    return groups;
}

export const ChangesCard = ({feature}: Props) => {


    return (
        <Card>
            <CardHeader title={'Changes'}/>
            <div style={{padding: '0px 30px 30px 30px', marginBottom: '-10px'}}>
                {feature.changes?.map(changeset => {
                    return (<div key={changeset.date} style={{marginBottom: '20px'}}>
                        <b>{formatDate(changeset.date)} - {changeset.type}</b>
                        <div style={{'marginLeft': '20px'}}>
                            {group(changeset.changes).map(group => {
                                return (
                                    <FeatureChange changes={group.changes} key={changeset.date + group.property + group.changes.length}/>
                                );
                            })}
                        </div>
                    </div>)
                })}
            </div>
        </Card>
    );
}

export default ChangesCard;