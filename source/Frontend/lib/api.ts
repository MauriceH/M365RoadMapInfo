const createGetRequestOptions = ({user,pass}:{user?: string, pass?: string}) : RequestInit =>{
    const authHeader = new Headers();
    const buff = Buffer.from(user + ":" + pass);
    const base64data = buff.toString('base64');
    authHeader.append("Authorization", "Basic " + base64data);
    return {
        method: 'GET',
        headers: authHeader,
        redirect: 'follow'
    };
}
export default createGetRequestOptions;

