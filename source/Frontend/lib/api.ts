const createGetRequestOptions = () : RequestInit =>{
    const authHeader = new Headers();
    const buff = Buffer.from(process.env.BACKEND_USER + ":" + process.env.BACKEND_PASS);
    const base64data = buff.toString('base64');
    authHeader.append("Authorization", "Basic " + base64data);
    return {
        method: 'GET',
        headers: authHeader,
        redirect: 'follow'
    };
}
export default createGetRequestOptions;

