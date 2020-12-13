import Head from 'next/head'
import Layout, {siteTitle} from '../components/layout'
import utilStyles from '../styles/utils.module.css'
import Link from 'next/link'
import {GetStaticProps} from 'next'
import PaginationResult from "../model/PaginationResult";
import {FeatureSlim} from "../model/feature";
import fs from "fs";
import {useEffect} from "react";
import {useRecoilValue, useRecoilValueLoadable, useSetRecoilState} from "recoil";
import featureListData from "../store/featureListData";
import {featureListFilteredSortedPaged} from "../store/featureListFilteredSortedPaged";
import {FilterListPagination} from "../components/FeatureListPagination";
import {TitleSearch} from "../components/TitleSearch";


interface Props {
    pageFiles: []
}



export default function Home({pageFiles}: Props) {
    const setFeatureData = useSetRecoilState(featureListData);
    const featureData = useRecoilValue(featureListFilteredSortedPaged);

    useEffect(() => {
        if(featureData?.items?.length ?? 0 > 0) return;
        const loading = async () => {
            try {
                const files = pageFiles
                let features = [];
                for (const fileName of files) {
                    const response = await fetch(`features/${fileName}`)
                    const results = await response.json() as FeatureSlim[]
                    features = [...features, ...results]
                }
                setFeatureData(features);
            } catch (e) {
                console.error(e);
                throw e;
            }
        }
        loading();
    }, [featureData])


    return (
        <Layout home>
            <Head>
                <title>{siteTitle}</title>
            </Head>
            {featureData &&
            <section className={`${utilStyles.headingMd} ${utilStyles.padding1px}`}>
              <h2 className={utilStyles.headingLg}>Features</h2>
              <div>
                <TitleSearch />
              </div>
              <div>
                <FilterListPagination />
              </div>
              <ul className={utilStyles.list}>
                  {featureData.items.map(feature => (
                      <div key={feature.no}>
                          No: <Link href={'features/' + feature.valueHash}><a>#{feature.no} - {feature.description}</a></Link>
                      </div>
                  ))}
              </ul>

            </section>

            }
        </Layout>
    )
}

export const getStaticProps: GetStaticProps = async () => {


    let response = await fetch('http://localhost:5000/RoadMap?totalCount=true');
    let data = await response.json() as PaginationResult<FeatureSlim>

    const totalCount = data.meta.totalCount

    const loads = Math.ceil(totalCount / data.meta.pageSize);
    const pagefiles = [];

    for (let i = 1; i < loads; i++) {
        response = await fetch('http://localhost:5000/RoadMap?page=' + i);
        data = await response.json() as PaginationResult<FeatureSlim>
        if (data.meta.items == 0) continue;
        const jsonContent = JSON.stringify(data.items);
        const fileName = 'data-' + data.meta.listHash + '.json';
        let path = 'public/features/' + fileName;
        if (!fs.existsSync(path)) {
            fs.writeFile(path, jsonContent, 'utf8', function (err) {
                if (err) {
                    console.log("An error occured while writing JSON Object to File.");
                    return console.log(err);
                }
                console.log("JSON file has been saved.");
            });
        }

        pagefiles.push(fileName);
    }

    return {
        props: {
            pageFiles: pagefiles
        }
    }
}