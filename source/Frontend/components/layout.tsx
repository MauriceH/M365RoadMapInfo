import Head from 'next/head'
import styles from './layout.module.css'
import utilStyles from '../styles/utils.module.css'
import Link from 'next/link'

export const siteTitle = 'M365 RoadMapInfo'

export default function Layout({
                                   children,
                                   home
                               }: {
    children: React.ReactNode
    home?: boolean
}) {
    return (
        <div className={styles.container}>
            <Head>
                <title>{siteTitle}</title>
                <meta
                    name="viewport"
                    content="minimum-scale=1, initial-scale=1, width=device-width"
                />
            </Head>
            <main>{children}</main>
            {!home && (
                <div className={styles.backToHome}>
                    <Link href="/">
                        <a>← Back to home</a>
                    </Link>
                </div>
            )}
        </div>
    )
}