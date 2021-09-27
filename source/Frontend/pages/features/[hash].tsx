import Layout from '../../components/layout'
import {getAllFeatureHashes, getFeatureData} from '../../lib/features'
import Head from 'next/head'
import Feature from "../../model/feature";
import {ParsedUrlQuery} from "querystring";
import {Card, CardContent, CardHeader, Chip, Theme, Typography} from "@material-ui/core";
import {GetStaticPaths, GetStaticProps} from "next";
import React from "react";
import {FeatureInfoLabels} from "../../components/FeatureInfoLabels";
import {LabeledValue} from "../../components/LabeldValue";
import {styled} from "@material-ui/styles";
import ChangesCard from "../../components/ChangesCard";

const MainContainer = styled('div')(() => ({
    alignSelf: 'center',
    display: 'flex',
    maxWidth: '1500px',
    margin: '-10px',
    flexDirection: 'column',
}))

const TopCardsContainer = styled('div')(({theme}) => ({
    display: 'flex',
    flexDirection: 'column',
    [(theme as Theme).breakpoints.up("md")]: {
        flexDirection: 'row',
    }
}))

const MainCardContainer = styled('div')(({theme}) => ({
    margin: '10px',
    [(theme as Theme).breakpoints.up("md")]: {
        //flex: '1 0 600px',
    }
}))

const TagCardContainer = styled('div')(({theme}) => ({
    margin: '0px',
    [(theme as Theme).breakpoints.up("md")]: {
        minWidth: '350px',
        paddingLeft: '20px'
    }
}))

const ChangesCardContainer = styled('div')(({theme}) => ({
    margin: '10px',
    [(theme as Theme).breakpoints.up("md")]: {}
}))

const TagList = styled('ul')(({theme}) => ({
    margin: '0 0',
    [(theme as Theme).breakpoints.up("md")]: {
        maxWidth: '280px'
    }
}))


interface Props {
    feature: Feature
}


export const FeatureInfo = ({feature}: Props) => (
    <Layout>
        <Head>
            <title>#{feature.no} - {feature.description}</title>
        </Head>

        <MainContainer style={{marginLeft: 'auto', marginRight: 'auto'}}>

            <MainCardContainer>
                <Card>
                    <TopCardsContainer>
                        <div>
                            <CardHeader title={'#' + feature.no + ' - ' + feature.description}/>
                            <CardContent>
                                <FeatureInfoLabels feature={feature}/>
                                <div style={{marginTop: '5px'}}>
                                    <Typography variant={"h6"}>Description:</Typography>
                                    <Typography variant={"body1"}>{feature.details}</Typography>
                                </div>
                                {feature.moreInfo &&
                                <div style={{marginTop: '15px'}}>
                                  <LabeledValue label={'MoreInfo:'}>
                                    <a href={feature.moreInfo}>Follow Link</a>
                                  </LabeledValue>
                                </div>
                                }
                            </CardContent>
                        </div>
                        <TagCardContainer>
                            <CardHeader title={'Tags'}/>
                            <CardContent>
                                <div>
                                    {feature.tagCategories?.map(cat => {
                                        return (<div key={cat.category}>
                                            <span><b>{cat.category}</b></span>
                                            <TagList>
                                                {cat.tags.map(tag => {
                                                    return (<Chip key={tag} label={tag} size={'small'} style={{margin:  '3px'}}/>);
                                                })}
                                            </TagList>
                                        </div>);
                                    })}
                                </div>
                            </CardContent>
                        </TagCardContainer>
                    </TopCardsContainer>
                </Card>
            </MainCardContainer>

            <ChangesCardContainer>
                <ChangesCard feature={feature}/>
            </ChangesCardContainer>
        </MainContainer>
    </Layout>
)

interface Parameter extends ParsedUrlQuery {
    hash: string
}

export const getStaticPaths: GetStaticPaths = async () => {
    const hashes = await getAllFeatureHashes()
    const paths: Array<string | { params: ParsedUrlQuery; locale?: string }> = hashes.map(hash => {
        return {
            params: {hash: hash}
        }
    });
    return {
        paths: paths,
        fallback: 'blocking'
    }
}

export const getStaticProps: GetStaticProps<Props, Parameter> = async (context) => {
    const featureData: Feature = await getFeatureData(context?.params?.hash ?? '');
    return {
        props: {
            feature: featureData
        }
    }
}

export default FeatureInfo;