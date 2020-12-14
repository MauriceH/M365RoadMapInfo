import Head from 'next/head'
import Layout, {siteTitle} from '../components/layout'
import utilStyles from '../styles/utils.module.css'
import Link from 'next/link'
import PaginationResult from "../model/PaginationResult";
import {FeatureSlim} from "../model/feature";
import fs from "fs";
import {useCallback, useEffect} from "react";
import {useRecoilValue, useSetRecoilState} from "recoil";
import featureListData from "../store/featureListData";
import {featureListFilteredSortedPaged} from "../store/featureListFilteredSortedPaged";
import {FilterListPagination} from "../components/FeatureListPagination";
import {TitleSearch} from "../components/TitleSearch";
import {
    Card,
    CardHeader,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TablePagination,
    TableRow,
    TableSortLabel
} from "@material-ui/core";
import {formatDate} from "../components/date";
import {GetStaticProps} from "next";


interface Props {
    pageFiles: []
}


export default function Home({pageFiles}: Props) {
    const setFeatureData = useSetRecoilState(featureListData);
    const featureData = useRecoilValue(featureListFilteredSortedPaged);

    useEffect(() => {
        if (featureData?.items?.length ?? 0 > 0) return;
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
    }, [featureData])


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

                </div>
                <Table size={'small'}>
                  <TableHead>
                    <TableRow>
                      <TableCell>
                        <TableSortLabel active={false}>
                          No
                        </TableSortLabel>
                      </TableCell>
                      <TableCell>
                        <TableSortLabel active={false}>
                          Description
                        </TableSortLabel>
                      </TableCell>
                      <TableCell>
                        <TableSortLabel active={false}>
                          Status
                        </TableSortLabel>
                      </TableCell>
                      <TableCell>
                        <TableSortLabel active={false}>
                          Last Modified
                        </TableSortLabel>
                      </TableCell>
                      <TableCell>
                        <TableSortLabel active={false}>
                          Product
                        </TableSortLabel>
                      </TableCell>
                      <TableCell>
                        <TableSortLabel active={false}>
                          Platform
                        </TableSortLabel>
                      </TableCell>
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

export const getStaticProps: GetStaticProps = async () =>
{


    let response = await fetch('http://localhost:5000/RoadMap?totalCount=true');
    let data = await response.json() as PaginationResult<FeatureSlim>

    const totalCount = data.meta.totalCount ?? 0

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