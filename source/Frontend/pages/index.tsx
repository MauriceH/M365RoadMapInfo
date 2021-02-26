import Head from 'next/head'
import Layout, {siteTitle} from '../components/layout'
import utilStyles from '../styles/utils.module.css'
import Link from 'next/link'
import PaginationResult from "../model/PaginationResult";
import {FeatureSlim} from "../model/feature";
import {useCallback, useEffect} from "react";
import {useRecoilState, useRecoilValue, useSetRecoilState} from "recoil";
import featureListData from "../store/featureListData";
import {featureListFilteredSortedPaged} from "../store/featureListFilteredSortedPaged";
import {FilterListPagination} from "../components/FeatureListPagination";
import {TitleSearch} from "../components/TitleSearch";
import {Card, CardHeader, Table, TableBody, TableCell, TableHead, TableRow, TableSortLabel} from "@material-ui/core";
import {formatDate} from "../components/date";
import {GetStaticProps} from "next";
import {featureListPage} from "../store/FeatureListPaging";
import {featureListSorting} from "../store/featureListFilteredSorted";
import createGetRequestOptions from "../lib/api";
import {ProductSelect} from "../components/ProductSelect";


interface Props {
    backendHost: []
    user?: string,
    pass?: string
}

type ColumnDefKey = keyof FeatureSlim | 'product' | 'platform';

interface ColumnDef {
    key: ColumnDefKey,
    label: string,
    width: string
}


export default function Home({backendHost, user, pass}: Props) {
    const setFeatureData = useSetRecoilState(featureListData);
    const setFeatureListPage = useSetRecoilState(featureListPage);
    const [listSorting, setListSorting] = useRecoilState(featureListSorting);
    const featureData = useRecoilValue(featureListFilteredSortedPaged);

    useEffect(() => {
        if (featureData?.items?.length ?? 0 > 0) return;
        setFeatureListPage(0)
        const loading = async () => {
            try {

                const userPass = {user: user, pass: pass};
                let response = await fetch(backendHost + '/RoadMap?totalCount=true', createGetRequestOptions(userPass));
                let data = await response.json() as PaginationResult<FeatureSlim>

                const totalCount = data.meta.totalCount ?? 0

                const loads = Math.ceil(totalCount / data.meta.pageSize);

                let features = [] as FeatureSlim[];
                for (let i = 1; i < loads; i++) {
                    response = await fetch(backendHost + '/RoadMap?page=' + i, createGetRequestOptions(userPass));
                    data = await response.json() as PaginationResult<FeatureSlim>
                    if (data.meta.items == 0) continue;
                    //const jsonContent = JSON.stringify(data.items);
                    features = [...features, ...data.items]
                }
                setFeatureData(features);
            } catch (e) {
                console.error(e);
                throw e;
            }
        }
        loading();
    }, [featureData, backendHost, setFeatureData, setFeatureListPage])


    const sortHandler = useCallback((columnKey: ColumnDefKey) => {
        if (columnKey == 'product' || columnKey == 'platform') return;
        if (listSorting.key == columnKey) {
            setListSorting((old) => {
                return {key: old.key, order: old.order == "asc" ? "desc" : "asc"}
            })
        } else {
            setListSorting({key: columnKey, order: "asc"})
        }
    }, [listSorting])


    const columns: ColumnDef[] = [
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
                <div style={{marginLeft: '1em'}}>
                  <TitleSearch/>
                  <ProductSelect/>
                </div>

                <div>
                  <FilterListPagination/>
                </div>
                <Table size={'small'}>
                  <TableHead>
                    <TableRow>
                        {columns.map(column => {
                            return (
                                <TableCell key={column.key} style={{width: column.width}}>
                                    <TableSortLabel
                                        active={listSorting.key == column.key}
                                        onClick={() => sortHandler(column.key)}
                                        direction={listSorting.order}>
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

    return {
        props: {
            backendHost: process.env.BACKEND_HOST,
            user: process.env.BACKEND_USER,
            pass: process.env.BACKEND_PASS,
        },
    }
}