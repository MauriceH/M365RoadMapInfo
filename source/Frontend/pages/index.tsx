import Head from 'next/head'
import Layout, {siteTitle} from '../components/layout'
import utilStyles from '../styles/utils.module.css'
import Link from 'next/link'
import PaginationResult from "../model/PaginationResult";
import {FeatureSlim} from "../model/feature";
import fs from "fs";
import {useCallback, useEffect} from "react";
import {useRecoilState, useRecoilValue, useSetRecoilState} from "recoil";
import featureListData from "../store/featureListData";
import {featureListFilteredSortedPaged} from "../store/featureListFilteredSortedPaged";
import {FilterListPagination} from "../components/FeatureListPagination";
import {TitleSearch} from "../components/TitleSearch";
import {
    Button,
    Card,
    CardHeader,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TableSortLabel
} from "@material-ui/core";
import {formatDate} from "../components/date";
import {GetStaticProps} from "next";
import {featureListPage} from "../store/FeatureListPaging";
import {featureListSorting} from "../store/featureListFilteredSorted";
import createGetRequestOptions from "../lib/api";


interface Props {
    pageFiles: []
}


export default function Home({pageFiles}: Props) {
    const setFeatureData = useSetRecoilState(featureListData);
    const setFeatureListPage = useSetRecoilState(featureListPage);
    const [listSorting, setListSorting] = useRecoilState(featureListSorting);
    const featureData = useRecoilValue(featureListFilteredSortedPaged);

    useEffect(() => {
        if (featureData?.items?.length ?? 0 > 0) return;
        setFeatureListPage(0)
        const loading = async () => {
            try {
                const files = pageFiles
                let features = [] as FeatureSlim[];
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
    }, [featureData, pageFiles, setFeatureData, setFeatureListPage])

    const testCallback = useCallback(() => {
        setListSorting(oldSorting => ({key: oldSorting.key, order: oldSorting.order == 'asc' ? 'desc' : 'asc'}))
    }, [setListSorting])


    const columns = [
        {key: 'no', label: 'No', width: '10px'},
        {key: 'description', label: 'Description', width: 'auto'},
        {key: 'status', label: 'Status', width: '100px'},
        {key: 'lastModified', label: 'Modified', width: '50px'},
        {key: 'product', label: 'Product', width: '250px'},
        {key: 'platform', label: 'Platform', width: '250px'},
    ]

    return (
        <Layout home>
            <Head>
                <title>{siteTitle}</title>
            </Head>
            {featureData &&
            <section className={`${utilStyles.headingMd} ${utilStyles.padding1px}`}>
              <Card>
                <CardHeader title={'Features'}/>
                <div>
                  <TitleSearch/>
                </div>
                <div>
                  <FilterListPagination/>
                </div>
                <div>
                  <Button onClick={testCallback}>Test</Button>
                </div>
                <Table size={'small'}>
                  <TableHead>
                    <TableRow>
                        {columns.map(column => {
                            return (
                                <TableCell key={column.key} style={{width: column.width}}>
                                    <TableSortLabel active={listSorting.key == column.key}>
                                        {column.label}
                                    </TableSortLabel>
                                </TableCell>
                            );
                        })}
                    </TableRow>
                  </TableHead>
                  <TableBody>
                      {featureData.items.map(feature => (
                          <TableRow
                              hover
                              tabIndex={-1}
                              key={feature.no}
                          >
                              <TableCell scope="row">
                                  <Link href={'features/' + feature.valueHash}>{feature.no}</Link>
                              </TableCell>
                              <TableCell scope="row">
                                  {feature.description}
                              </TableCell>
                              <TableCell scope="row">
                                  {feature.status}
                              </TableCell>
                              <TableCell scope="row">
                                  {formatDate(feature.lastModified)}
                              </TableCell>
                              <TableCell scope="row">
                                  {feature?.tagCategories?.find(cat => cat.category == 'Product')?.tags.join(", ")}
                              </TableCell>
                              <TableCell scope="row">
                                  {feature?.tagCategories?.find(cat => cat.category == 'Platform')?.tags.join(", ")}
                              </TableCell>
                          </TableRow>
                      ))}
                  </TableBody>
                </Table>
              </Card>
            </section>

            }
        </Layout>
    )
}

export const getStaticProps: GetStaticProps = async () => {


    let response = await fetch(process.env.BACKEND_HOST + '/RoadMap?totalCount=true',createGetRequestOptions());
    let data = await response.json() as PaginationResult<FeatureSlim>

    const totalCount = data.meta.totalCount ?? 0

    const loads = Math.ceil(totalCount / data.meta.pageSize);
    const pagefiles = [];

    for (let i = 1; i < loads; i++) {
        response = await fetch(process.env.BACKEND_HOST + '/RoadMap?page=' + i, createGetRequestOptions());
        data = await response.json() as PaginationResult<FeatureSlim>
        if (data.meta.items == 0) continue;
        const jsonContent = JSON.stringify(data.items);
        const fileName = 'data-' + data.meta.listHash + '.json';
        const path = 'public/features/' + fileName;
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